using Microsoft.EntityFrameworkCore;
using RolePermission.Domains.EntityBase;
using RolePermission.Domains.Schemas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RolePermission.Domains.Entities
{
    [Table(nameof(KeyPermission), Schema = DbSchemas.Auth)]
    [Index(nameof(ParentId), nameof(Deleted), nameof(OrderPriority), Name = $"IX_{nameof(KeyPermission)}")]
    public class KeyPermission : IFullAudited
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string PermissionKey { get; set; } = null!;
        [MaxLength(50)]
        public string? PermissionLabel { get; set; }
        public int? ParentId { get; set; }
        public KeyPermission? Parent {  get; set; }
        public int OrderPriority { get; set; }
        public List<KeyPermission>? Children { get; set; }

        #region audit
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        #endregion
    }
}
