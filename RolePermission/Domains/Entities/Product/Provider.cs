using Microsoft.EntityFrameworkCore;
using RolePermission.Domains.EntityBase;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RolePermission.Domains.Entities.Product
{
    [Table(nameof(Provider))]
    [Index(nameof(BillCategoryId), nameof(Deleted), Name = $"ID_{nameof(Provider)}", IsUnique = false)]
    public class Provider : IFullAudited
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; } = null!;
        [MaxLength(500)]
        public string? Description { get; set; }
        public int BillCategoryId { get; set; }
        public BillCategory BillCategory { get; set; } = null;
        public List<AssetProvider>? AssetProviders { get; set; }

        #region audit
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        #endregion

    }
}
