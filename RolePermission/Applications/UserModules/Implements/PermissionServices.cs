using Microsoft.EntityFrameworkCore;
using RolePermission.Applications.UserModules.Abstracts;
using RolePermission.Applications.UserModules.Dtos.Permission;
using RolePermission.Domains.Entities;
using RolePermission.Infrastructures.Persistances;
using RolePermission.Shared.ApplicationBase.Common;
using RolePermission.Shared.Consts;
using RolePermission.Shared.Consts.Permissions;

namespace RolePermission.Applications.UserModules.Implements
{
    public class PermissionServices : IPermissionServices
    {
        private readonly ILogger<PermissionServices> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        
        public PermissionServices(ILogger<PermissionServices> logger, ApplicationDbContext context, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _context = context;
            _httpContext = httpContext;
        }
        public bool CheckPermission(string[] permissionKeys)
        {
            var currentUserId = _httpContext.GetCurrentUserId();
            var currentUserType = _httpContext.GetCurrentUserType();
            _logger.LogInformation($"{nameof(CheckPermission)}: permissionKeys = {permissionKeys}, userId: {currentUserId}, userType: {currentUserType}");
            return currentUserType == UserTypes.ADMIN || GetListPermissionKeyContains(currentUserId, permissionKeys).Any();
        }

        public List<PermissionDto> FindAll()
        {
            _logger.LogInformation($"{nameof(FindAll)}");
            var result = PermissionConfig.appConfigs.Select(p => new PermissionDto
            {
                PermisisonKey = p.Key,
                PermissionLabel = p.Value.PermissionLabel,
                ParentKey = p.Value.ParentKey ?? ""
            }).ToList();
            return result;
        }

        public string[] GetAllPermissionKeyByApiEndPoint(string api)
        {
            var query = (from apiEndpoint in _context.ApiEndpoints
                         join permissionOfApi in _context.PermissionForApiEndpoints on apiEndpoint.Id equals permissionOfApi.ApiEndpointId
                         join permissionKey in _context.KeyPermission on permissionOfApi.KeyPermissionId equals permissionKey.Id
                         where api.ToLower().Contains(apiEndpoint.Path)
                         select permissionKey.PermissionKey).ToArray<string>();
            return query;
        }

        public List<string> GetPermissionsByCurrentUserId()
        {
            var currentUserId = _httpContext.GetCurrentUserId();
            var currentUserType = _httpContext.GetCurrentUserType();
            _logger.LogInformation($"{nameof(GetPermissionsByCurrentUserId)}: userId: {currentUserId}, userType: {currentUserType}");

            var result = new List<string>();
            if (currentUserType == UserTypes.ADMIN)
            {
                var temp = PermissionConfig.appConfigs.Select(c => c.Key);
                result.AddRange(PermissionConfig.appConfigs.Select(c => c.Key));
            }
            else
            {
                result = (from user in _context.Users
                          join userRole in _context.UserRoles on user.Id equals userRole.UserId
                          join role in _context.Roles on userRole.RoleId equals role.Id
                          join rolePermission in _context.RolePermissions on role.Id equals rolePermission.RoleId
                          into rps
                          from rp in rps
                          where user.Id == currentUserId && !user.Deleted && role.Status == CommonStatus.ACTIVE && !userRole.Deleted
                          select rp.PermissionKey).ToList();
            }
            return result;
        }
        private IQueryable<string?> GetListPermissionKeyContains(
            int userId,
            string[] permissionKeys
        )
        {
            return from userRole in _context.UserRoles
                   join role in _context.Roles on userRole.RoleId equals role.Id
                   join rolePermission in _context.RolePermissions
                       on role.Id equals rolePermission.RoleId
                   where
                       userRole.UserId == userId
                       && !role.Deleted
                       && !userRole.Deleted
                       && role.Status == CommonStatus.ACTIVE
                       && permissionKeys.Contains(rolePermission.PermissionKey)
                   select rolePermission.PermissionKey;
        }
    }
}
