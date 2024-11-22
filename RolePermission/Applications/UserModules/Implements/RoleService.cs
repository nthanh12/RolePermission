using Microsoft.EntityFrameworkCore;
using RolePermission.Applications.UserModules.Abstracts;
using RolePermission.Applications.UserModules.Dtos.Role;
using RolePermission.Domains.Entities;
using RolePermission.Infrastructures.Persistances;
using RolePermission.Shared.ApplicationBase.Common;
using RolePermission.Shared.Consts;
using RolePermission.Shared.Consts.Exceptions;
using RolePermission.Shared.Consts.Permissions;
using RolePermission.Shared.Exceptions;
using System.Text.Json;

namespace RolePermission.Applications.UserModules.Implements
{
    public class RoleService : IRoleService
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context, ILogger<RoleService> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Create(CreateRoleDto input)
        {
            _logger.LogInformation($"{nameof(Create)}: input = {JsonSerializer.Serialize(input)}");

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var newRole = new Role()
                    {
                        Name = input.Name,
                        Status = CommonStatus.ACTIVE
                    };
                    _context.Roles.Add(newRole);
                    _context.SaveChanges();

                    var newRoleId = newRole.Id;

                    if (input.PermissionKeys?.Count() > 0)
                    {
                        var newRolePermissions = input.PermissionKeys.Select(x => new Domains.Entities.RolePermission()
                        {
                            PermissionKey = x,
                            RoleId = newRoleId
                        });
                        _context.RolePermissions.AddRange(newRolePermissions);
                    }
                    _context.SaveChanges();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)}: id = {id}");
            var role = _context.Roles.FirstOrDefault(x => x.Id == id) ?? throw new UserFriendlyException(ErrorCode.RoleNotFound);
            role.Deleted = true;
            await _context.SaveChangesAsync();

        }

        public async Task<PagingResult<RoleDto>> FindAll(PagingRequestBaseDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");
            var query = _context.Roles.AsNoTracking()
                .Where(r => !r.Deleted
                    && (string.IsNullOrEmpty(input.Keyword) || r.Name.Contains(input.Keyword)));

            var totalItems = await query.CountAsync();
            var items = await query.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Status = r.Status,
            }).ToListAsync();

            if (input.PageSize != -1)
            {
                items.Skip(input.GetSkip())
                    .Take(input.PageSize);
            }

            var result = new PagingResult<RoleDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            return result;
        }

        public async Task<DetailRoleDto> FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: id = {id}");
            var roleResult = await _context.Roles
                                            .Include(r => r.RolePermissions)
                                            .Where(r => !r.Deleted && r.Id == id && r.Status == CommonStatus.ACTIVE)
                                            .Select(c => new DetailRoleDto
                                            {
                                                Id = c.Id,
                                                Name = c.Name,
                                                PermissionKeys = c.RolePermissions.Where(rp => !rp.Deleted).Select(rp => rp.PermissionKey).Distinct().ToList(),
                                            })
                                            .FirstOrDefaultAsync()
                            ?? throw new UserFriendlyException(ErrorCode.RoleNotFound);
            return new DetailRoleDto
            {
                Id = roleResult.Id,
                Name = roleResult.Name,
                PermissionKeys = roleResult.PermissionKeys
            };
        }

        public async Task Update(UpdateRoleDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == input.Id) ?? throw new UserFriendlyException(ErrorCode.RoleNotFound);
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    role.Name = input.Name;
                    _context.SaveChanges();
                    var currentRolePermissions = _context.RolePermissions.Where(x => !x.Deleted && x.RoleId == input.Id)
                                                                        .Select(e => e.PermissionKey).ToList();

                    // List permission cần xóa
                    var removeRolePermissions = currentRolePermissions.Except(input.PermissionKeys).ToList();
                    await _context.RolePermissions.Where(rp => rp.RoleId == input.Id && removeRolePermissions.Contains(rp.PermissionKey)).ExecuteDeleteAsync();
                    _context.SaveChanges();

                    var roleForUpdate = input.PermissionKeys.Select(c => new Domains.Entities.RolePermission
                    {
                        RoleId = role.Id,
                        PermissionKey = c
                    });
                    _context.RolePermissions.AddRange(roleForUpdate);

                    _context.SaveChanges();
                    await transaction.CommitAsync();

                }
                catch (Exception ex) 
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }    

        }

        public async Task UpdateStatusRole(int id, int status)
        {
            _logger.LogInformation($"{nameof(UpdateStatusRole)}: Id = {id}, status = {status}");
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id && !r.Deleted && r.Status == CommonStatus.ACTIVE)
                ?? throw new UserFriendlyException(ErrorCode.RoleNotFound);
            role.Status = status;
            await _context.SaveChangesAsync();
        }
    }
}
