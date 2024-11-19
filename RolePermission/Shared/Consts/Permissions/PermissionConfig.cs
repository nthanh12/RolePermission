namespace RolePermission.Shared.Consts.Permissions
{
    public class PermissionConfig
    {
        public static readonly Dictionary<string, PermissionContent> appConfigs = new()
        {
            // Dashboard
            {PermissionKeys.MenuDashboard, new(nameof(PermissionKeys.MenuDashboard), PermissionLabel.MenuDashboard )},

            {PermissionKeys.MenuConfig, new(nameof(PermissionKeys.MenuConfig), PermissionLabel.MenuConfig )},
            {PermissionKeys.MenuRoleManager, new(nameof(PermissionKeys.MenuRoleManager), PermissionLabel.MenuRoleManager )},
            { PermissionKeys.ButtonCreateRole, new(nameof(PermissionKeys.ButtonCreateRole), PermissionLabel.ButtonCreateRole, PermissionKeys.MenuRoleManager)},
            { PermissionKeys.TableRole, new(nameof(PermissionKeys.TableRole), PermissionLabel.TableRole, PermissionKeys.MenuRoleManager)},
            { PermissionKeys.ButtonDetailRole, new(nameof(PermissionKeys.ButtonDetailRole), PermissionLabel.ButtonDetailRole, PermissionKeys.TableRole)},
            { PermissionKeys.ButtonUpdateStatusRole, new(nameof(PermissionKeys.ButtonUpdateStatusRole), PermissionLabel.ButtonUpdateStatusRole, PermissionKeys.TableRole)},
            { PermissionKeys.ButtonUpdateRole, new(nameof(PermissionKeys.ButtonUpdateRole), PermissionLabel.ButtonUpdateRole, PermissionKeys.TableRole)}
        };
    }
}
