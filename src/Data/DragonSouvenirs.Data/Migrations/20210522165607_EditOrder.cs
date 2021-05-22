using Microsoft.EntityFrameworkCore.Migrations;

namespace DragonSouvenirs.Data.Migrations
{
    public partial class EditOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserFullName",
                table: "Orders",
                newName: "ClientFullName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClientFullName",
                table: "Orders",
                newName: "UserFullName");
        }
    }
}
