using Microsoft.EntityFrameworkCore.Migrations;

namespace DragonSouvenirs.Data.Migrations
{
    public partial class AddFullNameToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserFullName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserFullName",
                table: "Orders");
        }
    }
}
