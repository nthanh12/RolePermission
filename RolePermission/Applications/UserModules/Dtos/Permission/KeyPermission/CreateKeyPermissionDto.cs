﻿using System.ComponentModel.DataAnnotations;

namespace RolePermission.Applications.UserModules.Dtos.Permission.KeyPermission
{
    public class CreateKeyPermissionDto
    {
        public string PermissionKey { get; set; } = null!;
        [MaxLength(50)]
        public string? PermissionLabel { get; set; }
        public int? ParentId { get; set; }
        [Range(1, int.MaxValue)]
        public int OrderPriority { get; set; }
    }
}
