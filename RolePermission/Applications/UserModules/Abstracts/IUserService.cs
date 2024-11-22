namespace RolePermission.Applications.UserModules.Abstracts
{
    public interface IUserService
    {
        void AddRoleToUser(int roleId, int userId);
        void RemoveRoleFromUser(int RoleId, int userId);
    }
}
