using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemirbasTakip.Migrations
{
    /// <inheritdoc />
    public partial class IslemLogYeniKolonlar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Detay",
                table: "IslemLoglari");

            migrationBuilder.DropColumn(
                name: "Islem",
                table: "IslemLoglari");

            migrationBuilder.DropColumn(
                name: "KullaniciAdi",
                table: "IslemLoglari");

            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "IslemLoglari",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IslemTuru",
                table: "IslemLoglari",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Kullanici",
                table: "IslemLoglari",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "IslemLoglari");

            migrationBuilder.DropColumn(
                name: "IslemTuru",
                table: "IslemLoglari");

            migrationBuilder.DropColumn(
                name: "Kullanici",
                table: "IslemLoglari");

            migrationBuilder.AddColumn<string>(
                name: "Detay",
                table: "IslemLoglari",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Islem",
                table: "IslemLoglari",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KullaniciAdi",
                table: "IslemLoglari",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
