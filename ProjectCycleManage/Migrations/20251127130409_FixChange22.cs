using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixChange22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AprilBudget",
                table: "AnnualBudgetTable",
                type: "double",
                nullable: true,
                comment: "4月预算");

            migrationBuilder.AddColumn<double>(
                name: "AugustBudget",
                table: "AnnualBudgetTable",
                type: "double",
                nullable: true,
                comment: "8月预算");

            migrationBuilder.AddColumn<double>(
                name: "DecemberBudget",
                table: "AnnualBudgetTable",
                type: "double",
                nullable: true,
                comment: "12月预算");

            migrationBuilder.AddColumn<double>(
                name: "FebruaryBudget",
                table: "AnnualBudgetTable",
                type: "double",
                nullable: true,
                comment: "2月预算");

            migrationBuilder.AddColumn<double>(
                name: "JanuaryBudget",
                table: "AnnualBudgetTable",
                type: "double",
                nullable: true,
                comment: "1月预算");

            migrationBuilder.AddColumn<double>(
                name: "JulyBudget",
                table: "AnnualBudgetTable",
                type: "double",
                nullable: true,
                comment: "7月预算");

            migrationBuilder.AddColumn<double>(
                name: "JuneBudget",
                table: "AnnualBudgetTable",
                type: "double",
                nullable: true,
                comment: "6月预算");

            migrationBuilder.AddColumn<double>(
                name: "MarchBudget",
                table: "AnnualBudgetTable",
                type: "double",
                nullable: true,
                comment: "3月预算");

            migrationBuilder.AddColumn<double>(
                name: "MayBudget",
                table: "AnnualBudgetTable",
                type: "double",
                nullable: true,
                comment: "5月预算");

            migrationBuilder.AddColumn<double>(
                name: "NovemberBudget",
                table: "AnnualBudgetTable",
                type: "double",
                nullable: true,
                comment: "11月预算");

            migrationBuilder.AddColumn<double>(
                name: "OctoberBudget",
                table: "AnnualBudgetTable",
                type: "double",
                nullable: true,
                comment: "10月预算");

            migrationBuilder.AddColumn<double>(
                name: "SeptemberBudget",
                table: "AnnualBudgetTable",
                type: "double",
                nullable: true,
                comment: "9月预算");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AprilBudget",
                table: "AnnualBudgetTable");

            migrationBuilder.DropColumn(
                name: "AugustBudget",
                table: "AnnualBudgetTable");

            migrationBuilder.DropColumn(
                name: "DecemberBudget",
                table: "AnnualBudgetTable");

            migrationBuilder.DropColumn(
                name: "FebruaryBudget",
                table: "AnnualBudgetTable");

            migrationBuilder.DropColumn(
                name: "JanuaryBudget",
                table: "AnnualBudgetTable");

            migrationBuilder.DropColumn(
                name: "JulyBudget",
                table: "AnnualBudgetTable");

            migrationBuilder.DropColumn(
                name: "JuneBudget",
                table: "AnnualBudgetTable");

            migrationBuilder.DropColumn(
                name: "MarchBudget",
                table: "AnnualBudgetTable");

            migrationBuilder.DropColumn(
                name: "MayBudget",
                table: "AnnualBudgetTable");

            migrationBuilder.DropColumn(
                name: "NovemberBudget",
                table: "AnnualBudgetTable");

            migrationBuilder.DropColumn(
                name: "OctoberBudget",
                table: "AnnualBudgetTable");

            migrationBuilder.DropColumn(
                name: "SeptemberBudget",
                table: "AnnualBudgetTable");
        }
    }
}
