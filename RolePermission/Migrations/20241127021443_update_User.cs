using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RolePermission.Migrations
{
    /// <inheritdoc />
    public partial class update_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "auth",
                table: "User",
                newName: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                schema: "auth",
                table: "User",
                newName: "Name");
        }
    }
}
