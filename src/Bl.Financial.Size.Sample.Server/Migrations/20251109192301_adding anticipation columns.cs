using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bl.Financial.Size.Sample.Server.Migrations
{
    /// <inheritdoc />
    public partial class addinganticipationcolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                schema: "sizetest",
                table: "Anticipations",
                type: "BIGINT",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Anticipations_NfId",
                schema: "sizetest",
                table: "Anticipations",
                column: "NfId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Anticipations_NfId",
                schema: "sizetest",
                table: "Anticipations");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "sizetest",
                table: "Anticipations");
        }
    }
}
