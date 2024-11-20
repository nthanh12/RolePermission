using RolePermission.Applications.UserModules.Dtos;

namespace RolePermission.Applications.UserModules.Abstracts
{
    public interface IAuthService
    {
        public TokenApiDto Login(UserLoginDto loginDto);

        //public TokenApiDto RefreshToken(TokenApiDto input);

        public void RegisterUser(CreateUserDto createUserDto);
    }
}
