using RolePermission.Applications.UserModules.Dtos.Permission;

namespace RolePermission.Applications.UserModules.Abstracts
{
    public interface IPermissionServices
    {
        bool CheckPermission(params string[] permissionKeys);
        List<string> GetPermissionsByCurrentUserId();
        List<PermissionDto> FindAll();
        string[] GetAllPermissionKeyByApiEndPoint(string api);
    }
}
