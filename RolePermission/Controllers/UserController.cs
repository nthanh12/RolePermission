using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolePermission.Applications.UserModules.Abstracts;
using RolePermission.Shared.WebAPIBase;

namespace RolePermission.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        private readonly IUserServices _userService;
        public UserController(ILogger<UserController> logger, IUserServices userService) : base(logger)
        {
            _userService = userService;
        }

        [HttpPost("add-role-to-user")]
        public ApiResponse AddRoleToUser (int roleId, int userId)
        {
            try
            {
                _userService.AddRoleToUser(roleId, userId);
                return new();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("remove-role-to-user")]
        public ApiResponse RemoveRoleToUser (int roleId, int userId) 
        {
            try
            {
                _userService.RemoveRoleFromUser(roleId, userId);
                return new();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
