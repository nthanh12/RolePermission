namespace RolePermission.Applications.UserModules.Abstracts
{
    public interface IUserServices
    {
        void AddRoleToUser(int roleId, int userId);
        void RemoveRoleFromUser(int RoleId, int userId);
    }
}
