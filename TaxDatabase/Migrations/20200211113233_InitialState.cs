using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaxDatabase.Migrations
{
    public partial class InitialState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Municipalities",
                columns: table => new
                {
                    MunicipalityID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipalities", x => x.MunicipalityID);
                });

            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TaxValue = table.Column<double>(nullable: false),
                    MunicipalityDataID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Taxes_Municipalities_MunicipalityDataID",
                        column: x => x.MunicipalityDataID,
                        principalTable: "Municipalities",
                        principalColumn: "MunicipalityID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_MunicipalityDataID",
                table: "Taxes",
                column: "MunicipalityDataID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Taxes");

            migrationBuilder.DropTable(
                name: "Municipalities");
        }
    }
}
