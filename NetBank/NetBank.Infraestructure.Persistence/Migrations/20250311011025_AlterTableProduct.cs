using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetBank.Infraestructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Productos",
                type: "decimal(14,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,4)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Productos",
                type: "decimal(14,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,4)",
                oldNullable: true);
        }
    }
}
