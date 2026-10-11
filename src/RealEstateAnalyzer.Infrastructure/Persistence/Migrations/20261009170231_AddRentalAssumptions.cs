using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateAnalyzer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalAssumptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RentalMonthlyRent",
                table: "Offers",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RentalOtherMonthlyCosts",
                table: "Offers",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentalVacancyMonthsPerYear",
                table: "Offers",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentalMonthlyRent",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "RentalOtherMonthlyCosts",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "RentalVacancyMonthsPerYear",
                table: "Offers");
        }
    }
}
