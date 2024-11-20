using RolePermission.Applications.UserModules.Abstracts;
using RolePermission.Applications.UserModules.Dtos;

namespace RolePermission.Applications.UserModules.Implements
{
    public class AuthService : IAuthService
    {
        public TokenApiDto Login(UserLoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public void RegisterUser(CreateUserDto createUserDto)
        {
            throw new NotImplementedException();
        }
    }
}
