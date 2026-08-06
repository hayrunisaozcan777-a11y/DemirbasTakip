using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemirbasTakip.Migrations
{
    /// <inheritdoc />
    public partial class KullaniciOnayEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OnaylandiMi",
                table: "Kullanicilar",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Kullanicilar",
                keyColumn: "Id",
                keyValue: 1,
                column: "OnaylandiMi",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnaylandiMi",
                table: "Kullanicilar");
        }
    }
}
