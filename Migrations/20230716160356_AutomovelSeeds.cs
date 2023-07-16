using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VehiclesMinimalAPI.Migrations
{
    /// <inheritdoc />
    public partial class AutomovelSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Automoveis",
                columns: new[] { "Id", "Cor", "Disponivel", "Marca", "Modelo", "Placa" },
                values: new object[,]
                {
                    { 1, "Preto", true, "Chevrolet", "Joy", "ABC1234" },
                    { 2, "Prata", true, "Volkswagen", "Gol", "DEF5678" },
                    { 3, "Branco", true, "Ford", "Ka", "GHI9101" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Automoveis",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Automoveis",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Automoveis",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
