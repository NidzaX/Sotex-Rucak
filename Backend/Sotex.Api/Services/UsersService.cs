using AutoMapper;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
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
            _secretKey = config.GetSection("JwtSettings:SecretKey");
            _clientId = config.GetSection("Authentication:Google:ClientId");
        }

        public async Task<User> RegisterGoogleUserAsync(GoogleRegisterDto dto)
        {
            string token = dto.Token;

            // Decode the token payload without validation to check expiration.
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadToken(token) as JwtSecurityToken;
            if (jwtToken == null)
            {
                throw new Exception("Invalid token format.");
            }

            // Check if the token is expired
            var exp = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            if (long.TryParse(exp, out long expValue))
            {
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expValue).UtcDateTime;
                if (expirationTime < DateTime.UtcNow)
                {
                    throw new Exception("Google authentication failed: JWT has expired.");
                }
            }

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

            var existingUser = await _userRepo.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return existingUser;
            }

            User user = _mapper.Map<User>(dto);
            user.Password = "";  // No password since it's a Google login.

            User retVal = await _userRepo.AddUserAsync(user);
            return retVal;
        }



        public async Task<string> LoginGoogleAsync(string email, string token)
        {
            try
            {
                GoogleJsonWebSignature.ValidationSettings validationSettings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string>() { _clientId.Value }
                };

                // Validate the token
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);

                if (string.IsNullOrWhiteSpace(email))
                    throw new Exception("Invalid email.");

                User user = await _userRepo.FindByEmailAsync(email);
                if (user == null)
                    throw new Exception("User with that email doesn't exist.");

                // Generate token for your application
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("username", user.Username));
                claims.Add(new Claim("id", user.Id.ToString()));
                SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey.Value));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokenOptions = new JwtSecurityToken(
                    issuer: "http://localhost:5105",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(20),
                    signingCredentials: signinCredentials
                );

                return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            }
            catch (Exception e)
            {
                // Log the exception (consider using a logging framework)
                throw new Exception($"Login failed: {e.Message}");
            }
        }



    }
}

