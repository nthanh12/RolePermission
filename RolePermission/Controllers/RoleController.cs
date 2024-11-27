using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolePermission.Applications.UserModules.Abstracts;
using RolePermission.Applications.UserModules.Dtos.Role;
using RolePermission.Shared.ApplicationBase.Common;
using RolePermission.Shared.Consts.Permissions;
using RolePermission.Shared.WebAPIBase;
using static RolePermission.Shared.Filters.PermissionFilter;

namespace RolePermission.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ApiControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(ILogger logger, IRoleService roleService) : base(logger)
        {
            _roleService = roleService;
        }
        [PermissionFilter(PermissionKeys.MenuRoleManager)]
        [HttpGet("find-all")]
        public async Task<ApiResponse> FindAll([FromQuery] PagingRequestBaseDto input)
        {
            try
            {
                return new(await _roleService.FindAll(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        [PermissionFilter(PermissionKeys.ButtonDetailRole, PermissionKeys.ButtonUpdateRole)]
        [HttpGet("find-by-id/{id}")]
        public async Task<ApiResponse> FindById(int id)
        {
            try
            {
                return new(await _roleService.FindById(id));
            }
            catch(Exception ex)
            {
                return OkException(ex);
            }
        }
        [HttpPost("create")]
        public async Task<ApiResponse> Create([FromBody] CreateRoleDto input)
        {
            try
            {
                await _roleService.Create(input);
                return new();
            }
            catch(Exception ex)
            {
                return OkException(ex);
            }
        }
        [PermissionFilter(PermissionKeys.ButtonUpdateRole)]
        [HttpPut("update")]
        public async Task<ApiResponse> Update([FromBody] UpdateRoleDto input)
        {
            try
            {
                await _roleService.Update(input);
                return new();
            }
            catch(Exception ex)
            {
                return OkException(ex);
            }
        }
        [HttpDelete("delete/{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                await _roleService.Delete(id);
                return new();
            }
            catch(Exception ex)
            {
                return OkException(ex);
            }
        }
        [HttpPut("update-status")]
        public async Task<ApiResponse> UpdateStatus(int id, int status)
        {
            try
            {
                await _roleService.UpdateStatusRole(id, status);
                return new();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
