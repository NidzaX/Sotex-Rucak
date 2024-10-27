using AutoMapper;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using Sotex.Api.Dto.UserDto;
using Sotex.Api.Infrastructure;
using Sotex.Api.Interfaces;
using Sotex.Api.Model;
using Sotex.Api.Repo;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sotex.Api.Services
{
    public class UsersService : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly ProjectDbContext _dbContext;
        private readonly IConfigurationSection _secretKey;
        private readonly IConfigurationSection _clientId;
        private readonly UserRepo _userRepo;

        public UsersService(IMapper mapper, ProjectDbContext dbContext, IConfiguration config)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _userRepo = new UserRepo(dbContext);
            _secretKey = config.GetSection("ClientSecret");
            _clientId = config.GetSection("ClientId");
        }

        public async Task<User> RegisterGoogleUserAsync(GoogleRegisterDto dto)
        {
            string token = dto.Token;

            try
            {
                GoogleJsonWebSignature.ValidationSettings validationSettings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string>() { _clientId.Value }
                };

                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);
            }
            catch (Exception e)
            {
                throw new Exception("Google authentication failed: " + e.Message);
            }

            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Email))
                throw new Exception("Invalid data");

            User user = _mapper.Map<User>(dto);
            user.Password = "";  // No password since it's a Google login.

            // Ensure no existing user with the same email
            var existingUser = await _userRepo.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new Exception("A user with this email already exists.");
            }

            User retVal = await _userRepo.AddUserAsync(user);
            return retVal;
        }


        public async Task<string> LoginGoogleAsync(string email, string token)
        {
            try
            {
                // Log expected and actual audiences
                Console.WriteLine($"Expected Audience: {_clientId.Value}");

                // Validate Google Token
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { _clientId.Value }
                };

                // Validate the token
                var payload = await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);

                Console.WriteLine($"Actual Audience from Token: {payload.Audience}");

                if (payload.Email != email)
                {
                    throw new UnauthorizedAccessException("Email mismatch.");
                }

                // Check if the user exists in the database
                User user = await _userRepo.FindByEmailAsync(email);
                if (user == null)
                {
                    throw new Exception("User with this email does not exist.");
                }

                // Generate JWT for your app
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey.Value));
                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(
                    issuer: "http://localhost:5105",
                    audience: "http://localhost:4200",
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(20),
                    signingCredentials: signingCredentials
                );

                return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("Login failed: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Login failed: " + ex.Message);
            }
        }


    }
}

