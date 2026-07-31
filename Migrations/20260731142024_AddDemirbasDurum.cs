using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemirbasTakip.Migrations
{
    /// <inheritdoc />
    public partial class AddDemirbasDurum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SifreDegistirilsinMi",
                table: "Kullanicilar",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Durum",
                table: "Demirbaslar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Kullanicilar",
                keyColumn: "Id",
                keyValue: 1,
                column: "SifreDegistirilsinMi",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SifreDegistirilsinMi",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "Durum",
                table: "Demirbaslar");
        }
    }
}
