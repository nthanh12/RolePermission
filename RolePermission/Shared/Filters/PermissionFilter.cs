using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RolePermission.Applications.UserModules.Abstracts;
using RolePermission.Shared.ApplicationBase.Common;
using RolePermission.Shared.Consts;
using RolePermission.Shared.Consts.Exceptions;

namespace RolePermission.Shared.Filters
{
    public class PermissionFilter
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class PermissionFilterAttribute : Attribute, IAuthorizationFilter
        {
            private readonly string[] _permissions;
            private IPermissionServices? _permissionService;
            private IHttpContextAccessor _httpContext;

            public PermissionFilterAttribute(params string[] permissions)
            {
                _permissions = permissions;
            }
            private void GetServices(AuthorizationFilterContext filterContext)
            {
                _permissionService =
                    filterContext.HttpContext.RequestServices.GetRequiredService<IPermissionServices>();
                _httpContext =
                    filterContext.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();
            }
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                GetServices(context);
                var userType = _httpContext!.GetCurrentUserType();
                if (userType == UserTypes.ADMIN)
                {
                    return;
                }
                bool isGrant = _permissionService!.CheckPermission(_permissions);
                var permissionQueryParam = context
                    .HttpContext.Request.Query[QueryParamKeys.Permission]
                    .ToString()
                    .Trim();
                if (
                    !string.IsNullOrEmpty(permissionQueryParam)
                    && isGrant
                    && !_permissionService.CheckPermission(permissionQueryParam)
                    && _permissions.Contains(permissionQueryParam)
                )
                {
                    isGrant = false;
                }
                if (!isGrant)
                {
                    context.Result = new UnauthorizedObjectResult(
                        new { message = ErrorMessage.UserNotHavePermission });
                }
            }
        }
    }
}
