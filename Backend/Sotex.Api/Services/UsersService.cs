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
                GoogleJsonWebSignature.ValidationSettings validationSettings = new GoogleJsonWebSignature.ValidationSettings();
                validationSettings.Audience = new List<string>() { _clientId.Value };

                GoogleJsonWebSignature.Payload payload = Task.Run(() => GoogleJsonWebSignature.ValidateAsync(token, validationSettings)).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            if (email == "")
                throw new Exception("Invalid data");

            User user = await _userRepo.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("User with that email doesn't exists");

            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim("username", user.Username));

            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey.Value));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:5105", //url servera koji je izdao token
                claims: claims, //claimovi
                expires: DateTime.Now.AddMinutes(20), //vazenje tokena u minutama
                signingCredentials: signinCredentials //kredencijali za potpis
            );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

    }
}

