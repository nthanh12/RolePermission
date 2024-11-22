using RolePermission.Applications.UserModules.Dtos.Role;
using RolePermission.Shared.ApplicationBase.Common;

namespace RolePermission.Applications.UserModules.Abstracts
{
    public interface IRoleService
    {
        public Task<PagingResult<RoleDto>> FindAll(PagingRequestBaseDto input);
        public Task<DetailRoleDto> FindById(int id);
        public Task Create(CreateRoleDto input);
        public Task Update(UpdateRoleDto input);
        public Task Delete(int id);
        public Task UpdateStatusRole(int id, int status);
    }
}
