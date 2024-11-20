namespace RolePermission.Shared.Consts.Permissions
{
    public class PermissionContent
    {
        public string PermissionKey { get; set; }
        public string PermissionLabel { get; set; }
        public string? ParentKey { get; set; }

        public PermissionContent(string permissionKey, string permissionLabel, string? parentKey = null) 
        {
            PermissionKey = permissionKey;
            PermissionLabel = permissionLabel;
            ParentKey = parentKey;
        }
    }
}
