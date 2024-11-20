using RolePermission.Domains.Schemas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RolePermission.Domains.Entities
{
    [Table(nameof(PermissionForApiEndpoint), Schema = DbSchemas.Auth)]
    public class PermissionForApiEndpoint
    {
        [Key]
        public int Id { get; set; }
        public int KeyPermissionId { get; set; }
        public KeyPermission KeyPermission { get; set; } = null!;
        public int ApiEndpointId { get; set; }
        public ApiEndpoint ApiEndpoint { get; set; } = null!;
        public bool IsAuthenticate { get; set; }
    }
}
