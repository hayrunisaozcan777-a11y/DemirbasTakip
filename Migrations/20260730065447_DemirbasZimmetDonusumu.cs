using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemirbasTakip.Migrations
{
    /// <inheritdoc />
    public partial class DemirbasZimmetDonusumu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rezervasyonlar");

            migrationBuilder.DropTable(
                name: "Kaynaklar");

            migrationBuilder.CreateTable(
                name: "Demirbaslar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemirbasKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kategori = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeriNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demirbaslar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zimmetler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonelId = table.Column<int>(type: "int", nullable: false),
                    DemirbasId = table.Column<int>(type: "int", nullable: false),
                    ZimmetTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IadeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Not = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Durum = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zimmetler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zimmetler_Demirbaslar_DemirbasId",
                        column: x => x.DemirbasId,
                        principalTable: "Demirbaslar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Zimmetler_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zimmetler_DemirbasId",
                table: "Zimmetler",
                column: "DemirbasId");

            migrationBuilder.CreateIndex(
                name: "IX_Zimmetler_PersonelId",
                table: "Zimmetler",
                column: "PersonelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Zimmetler");

            migrationBuilder.DropTable(
                name: "Demirbaslar");

            migrationBuilder.CreateTable(
                name: "Kaynaklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    Kapasite = table.Column<int>(type: "int", nullable: false),
                    Konum = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tur = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kaynaklar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rezervasyonlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KaynakId = table.Column<int>(type: "int", nullable: false),
                    PersonelId = table.Column<int>(type: "int", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    BaslangicZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BitisZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Durum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervasyonlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rezervasyonlar_Kaynaklar_KaynakId",
                        column: x => x.KaynakId,
                        principalTable: "Kaynaklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rezervasyonlar_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rezervasyonlar_KaynakId",
                table: "Rezervasyonlar",
                column: "KaynakId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervasyonlar_PersonelId",
                table: "Rezervasyonlar",
                column: "PersonelId");
        }
    }
}
