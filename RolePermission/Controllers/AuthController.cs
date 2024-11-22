using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolePermission.Applications.UserModules.Abstracts;
using RolePermission.Applications.UserModules.Dtos;
using RolePermission.Shared.WebAPIBase;

namespace RolePermission.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(ILogger<AuthController> logger, IAuthService authService) : base (logger)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ApiResponse Login([FromBody] UserLoginDto input)
        {
            try
            {
                return new(_authService.Login(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("register")]
        public ApiResponse Register([FromBody] CreateUserDto input)
        {
            try
            {
                _authService.RegisterUser(input);
                return new();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
