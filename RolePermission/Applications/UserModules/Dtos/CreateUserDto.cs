using RolePermission.Shared.ApplicationBase.Common.Validations;
using System.ComponentModel.DataAnnotations;
using RolePermission.Shared.Consts;

namespace RolePermission.Applications.UserModules.Dtos
{
    public class CreateUserDto
    {
        private string _username = null!;

        [Required]
        public string Username
        {
            get => _username;
            set => _username = value.Trim();
        }
        private string _password = null!;
        [Required]
        public string Password
        {
            get => _password;
            set => _password = value.Trim();
        }
        [IntegerRange(AllowableValues = new int[] { UserTypes.ADMIN, UserTypes.CUSTOMER })]
        public int UserType {  get; set; }
    }
}
