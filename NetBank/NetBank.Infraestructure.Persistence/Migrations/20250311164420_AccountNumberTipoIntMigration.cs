using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetBank.Infraestructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AccountNumberTipoIntMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AccountNumber",
                table: "Beneficiarios",
                type: "Int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "Beneficiarios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Int");
        }
    }
}
