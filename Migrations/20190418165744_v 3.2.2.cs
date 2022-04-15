using Microsoft.EntityFrameworkCore.Migrations;

namespace MedEd.Migrations
{
    public partial class v322 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DostupnostMedProizvod",
                table: "MedProizvodLjekarna",
                nullable: true,
                oldClrType: typeof(short));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "DostupnostMedProizvod",
                table: "MedProizvodLjekarna",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
