namespace DragonSouvenirs.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ExtendOfficesEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Neighborhood",
                table: "Offices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Offices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetNumber",
                table: "Offices",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Neighborhood",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "StreetNumber",
                table: "Offices");
        }
    }
}
