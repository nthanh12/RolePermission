namespace RolePermission.Applications.UserModules.Dtos.Permission
{
    public class PermissionDto
    {
        public string PermisisonKey { get; set; } = null!;
        public string PermissionLabel { get; set; } = null!;
        public string ParentKey { get; set; } = null!;
    }
}
