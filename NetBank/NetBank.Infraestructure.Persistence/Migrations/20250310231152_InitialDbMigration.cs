using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetBank.Infraestructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialDbMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Beneficiarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficiarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    AccountNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "780000001, 1"),
                    Balance = table.Column<decimal>(type: "decimal(14,4)", nullable: false),
                    CreditLimit = table.Column<decimal>(type: "decimal(14,4)", nullable: true),
                    AmountOwed = table.Column<decimal>(type: "decimal(14,4)", nullable: true),
                    ProductType = table.Column<byte>(type: "tinyint", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.AccountNumber);
                });

            migrationBuilder.CreateTable(
                name: "Transacciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(14,4)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionType = table.Column<byte>(type: "tinyint", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    OriginProductId = table.Column<int>(type: "int", nullable: true),
                    DestinationProductId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacciones_Productos_DestinationProductId",
                        column: x => x.DestinationProductId,
                        principalTable: "Productos",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transacciones_Productos_OriginProductId",
                        column: x => x.OriginProductId,
                        principalTable: "Productos",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_DestinationProductId",
                table: "Transacciones",
                column: "DestinationProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_OriginProductId",
                table: "Transacciones",
                column: "OriginProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Beneficiarios");

            migrationBuilder.DropTable(
                name: "Transacciones");

            migrationBuilder.DropTable(
                name: "Productos");
        }
    }
}
