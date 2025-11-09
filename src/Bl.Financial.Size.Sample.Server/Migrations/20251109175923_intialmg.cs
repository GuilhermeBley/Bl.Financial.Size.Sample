using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bl.Financial.Size.Sample.Server.Migrations
{
    /// <inheritdoc />
    public partial class intialmg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sizetest");

            migrationBuilder.CreateTable(
                name: "Anticipations",
                schema: "sizetest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "BIGINT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NfId = table.Column<long>(type: "BIGINT", nullable: false),
                    Desagio = table.Column<decimal>(type: "DECIMAL(19,4)", nullable: false),
                    LiquidValue = table.Column<decimal>(type: "DECIMAL(19,4)", nullable: false),
                    TotalValue = table.Column<decimal>(type: "DECIMAL(19,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anticipations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                schema: "sizetest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "BIGINT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cnpj = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(500)", nullable: false),
                    MonthlyBilling = table.Column<decimal>(type: "DECIMAL(19,4)", nullable: false),
                    ServiceKind = table.Column<string>(type: "NVARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nfs",
                schema: "sizetest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "BIGINT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "BIGINT", nullable: false),
                    Number = table.Column<long>(type: "BIGINT", nullable: false),
                    Value = table.Column<decimal>(type: "DECIMAL(19,4)", nullable: false),
                    UniqueId = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    DueDate = table.Column<DateOnly>(type: "DATE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nfs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Cnpj",
                schema: "sizetest",
                table: "Companies",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nfs_UniqueId",
                schema: "sizetest",
                table: "Nfs",
                column: "UniqueId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anticipations",
                schema: "sizetest");

            migrationBuilder.DropTable(
                name: "Companies",
                schema: "sizetest");

            migrationBuilder.DropTable(
                name: "Nfs",
                schema: "sizetest");
        }
    }
}
