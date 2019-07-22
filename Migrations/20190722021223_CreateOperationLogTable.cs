using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Admin.Migrations
{
    public partial class CreateOperationLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OperationLog",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Username = table.Column<string>(nullable: true),
                    Method = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    Input = table.Column<string>(nullable: true),
                    CreateAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationLog", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationLog");
        }
    }
}
