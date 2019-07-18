using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MvcMovie.Models;

namespace MvcMovie.Migrations
{
    public partial class addIDToRoleMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<RoleMenu>("ID", "RoleMenu").Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
            migrationBuilder.AddPrimaryKey("PK_ROLE_MENU_ID", "RoleMenu", "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey("PK_ROLE_MENU_ID", "RoleMenu");
            migration
        }
    }
}
