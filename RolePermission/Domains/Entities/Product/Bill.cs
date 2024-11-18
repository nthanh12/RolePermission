using Microsoft.EntityFrameworkCore;
using RolePermission.Domains.EntityBase;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RolePermission.Domains.Entities.Product
{
    [Table(nameof(Bill))]
    [Index(
        nameof(BillCategoryId),
        nameof(Deleted),
        Name = $"IX_{nameof(Bill)}",
        IsUnique = false
    )]
    public class Bill : IFullAudited
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(512)]
        public string? Description { get; set; }
        public int BillCategoryId { get; set; }
        public BillCategory BillCategory { get; set; } = null!;
        public string BillDetail { get; set; }
        public List<Transaction>? Transactions {  get; set; } 

        #region audit
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        #endregion

    }
}
