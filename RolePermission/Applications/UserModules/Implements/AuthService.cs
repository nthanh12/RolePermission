using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RolePermission.Applications.UserModules.Abstracts;
using RolePermission.Applications.UserModules.Dtos;
using RolePermission.Domains.Entities;
using RolePermission.Infrastructures.Persistances;
using RolePermission.Shared.Consts.Exceptions;
using RolePermission.Shared.Exceptions;
using RolePermission.Shared.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace RolePermission.Applications.UserModules.Implements
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        
        public AuthService(ApplicationDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }
        public TokenApiDto Login(UserLoginDto userInput)
        {
            _logger.LogInformation($"{nameof(Login)}: input = { JsonSerializer.Serialize(userInput)}" );
            var user = _context.Users.FirstOrDefault(x => x.UserName == userInput.Username); 
            if (user == null)
            {
                throw new UserFriendlyException(ErrorCode.UserNotFound);
            }
            if (!PasswordHasher.VerifyPassword(userInput.Password, user.Password))
            {
                throw new UserFriendlyException(ErrorCode.PasswordWrong);
            }
            var newAccessToken = CreateToken(userInput);
            _context.SaveChangesAsync();

            return new TokenApiDto
            {
                AccessToken = newAccessToken
            };
        }

        public void RegisterUser (CreateUserDto user)
        {
            _logger.LogInformation($"{nameof(RegisterUser)}: input = {JsonSerializer.Serialize(user)}");
            var check = _context.Users.FirstOrDefault(x = x.UserName = user.Username);
            if (check != null)
            {
                throw new UserFriendlyException(ErrorCode.UsernameIsExist);
            }
            if (user.Password.Length < 6)
            {
                throw new UserFriendlyException(ErrorCode.PasswordMustBeLongerThanSixCharacter);
            }
            if (!(Regex.IsMatch(user.Password, "[a-z]") && Regex.IsMatch(user.Password, "[A-Z]") && Regex.IsMatch(user.Password, "[0-9]")))
            {
                throw new UserFriendlyException(ErrorCode.TypeofPasswordMustBeNumberOrString);            
            }
            if (!Regex.IsMatch(user.Password, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                throw new UserFriendlyException(ErrorCode.PasswordMustBeContainsSpecifyCharacter);

            _context.Users.Add(new User
            {
                UserName = user.Username,
                Password = PasswordHasher.HassPassword(user.Password),
                UserType = user.UserType
            });
            _context.SaveChanges();
        }
        private string CreateToken(UserLoginDto user)
        {
            var jwtToken = new JwtSecurityTokenHandler();
            var userId = _context.Users.FirstOrDefault(u => u.UserName == user.Username) ?? throw new UserFriendlyException(ErrorCode.UserNotFound);

            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWT")["Key"]);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, $"{userId.Id}"),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("user_type", userId.UserType.ToString()),
                new Claim("user_id", userId.Id.ToString())
            };
            
            var credentials = new SigningCredentials( new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials: credentials
            );
            return jwtToken.WriteToken(token);
        }


    }
}
