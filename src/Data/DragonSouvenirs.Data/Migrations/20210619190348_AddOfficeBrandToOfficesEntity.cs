using Microsoft.EntityFrameworkCore.Migrations;

namespace DragonSouvenirs.Data.Migrations
{
    public partial class AddOfficeBrandToOfficesEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OfficeBrand",
                table: "Offices",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfficeBrand",
                table: "Offices");
        }
    }
}
