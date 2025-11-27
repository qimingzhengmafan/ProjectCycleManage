using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixChange23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "SalesVolumeTables",
                comment: "年度销售预测",
                oldComment: "年度销售额")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<double>(
                name: "SalesVolume",
                table: "SalesVolumeTables",
                type: "double",
                nullable: false,
                comment: "销售预测",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "销售额");

            migrationBuilder.AddColumn<double>(
                name: "AprilSalesVolume",
                table: "SalesVolumeTables",
                type: "double",
                nullable: true,
                comment: "4月销售预测");

            migrationBuilder.AddColumn<double>(
                name: "AugustSalesVolume",
                table: "SalesVolumeTables",
                type: "double",
                nullable: true,
                comment: "8月销售预测");

            migrationBuilder.AddColumn<double>(
                name: "DecemberSalesVolume",
                table: "SalesVolumeTables",
                type: "double",
                nullable: true,
                comment: "12月销售预测");

            migrationBuilder.AddColumn<double>(
                name: "FebruarySalesVolume",
                table: "SalesVolumeTables",
                type: "double",
                nullable: true,
                comment: "2月销售预测");

            migrationBuilder.AddColumn<double>(
                name: "JanuarySalesVolume",
                table: "SalesVolumeTables",
                type: "double",
                nullable: true,
                comment: "1月销售预测");

            migrationBuilder.AddColumn<double>(
                name: "JulySalesVolume",
                table: "SalesVolumeTables",
                type: "double",
                nullable: true,
                comment: "7月销售预测");

            migrationBuilder.AddColumn<double>(
                name: "JuneSalesVolume",
                table: "SalesVolumeTables",
                type: "double",
                nullable: true,
                comment: "6月销售预测");

            migrationBuilder.AddColumn<double>(
                name: "MarchSalesVolume",
                table: "SalesVolumeTables",
                type: "double",
                nullable: true,
                comment: "3月销售预测");

            migrationBuilder.AddColumn<double>(
                name: "MaySalesVolume",
                table: "SalesVolumeTables",
                type: "double",
                nullable: true,
                comment: "5月销售预测");

            migrationBuilder.AddColumn<double>(
                name: "NovemberSalesVolume",
                table: "SalesVolumeTables",
                type: "double",
                nullable: true,
                comment: "11月销售预测");

            migrationBuilder.AddColumn<double>(
                name: "OctoberSalesVolume",
                table: "SalesVolumeTables",
                type: "double",
                nullable: true,
                comment: "10月销售预测");

            migrationBuilder.AddColumn<double>(
                name: "SeptemberSalesVolume",
                table: "SalesVolumeTables",
                type: "double",
                nullable: true,
                comment: "9月销售预测");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AprilSalesVolume",
                table: "SalesVolumeTables");

            migrationBuilder.DropColumn(
                name: "AugustSalesVolume",
                table: "SalesVolumeTables");

            migrationBuilder.DropColumn(
                name: "DecemberSalesVolume",
                table: "SalesVolumeTables");

            migrationBuilder.DropColumn(
                name: "FebruarySalesVolume",
                table: "SalesVolumeTables");

            migrationBuilder.DropColumn(
                name: "JanuarySalesVolume",
                table: "SalesVolumeTables");

            migrationBuilder.DropColumn(
                name: "JulySalesVolume",
                table: "SalesVolumeTables");

            migrationBuilder.DropColumn(
                name: "JuneSalesVolume",
                table: "SalesVolumeTables");

            migrationBuilder.DropColumn(
                name: "MarchSalesVolume",
                table: "SalesVolumeTables");

            migrationBuilder.DropColumn(
                name: "MaySalesVolume",
                table: "SalesVolumeTables");

            migrationBuilder.DropColumn(
                name: "NovemberSalesVolume",
                table: "SalesVolumeTables");

            migrationBuilder.DropColumn(
                name: "OctoberSalesVolume",
                table: "SalesVolumeTables");

            migrationBuilder.DropColumn(
                name: "SeptemberSalesVolume",
                table: "SalesVolumeTables");

            migrationBuilder.AlterTable(
                name: "SalesVolumeTables",
                comment: "年度销售额",
                oldComment: "年度销售预测")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<double>(
                name: "SalesVolume",
                table: "SalesVolumeTables",
                type: "double",
                nullable: false,
                comment: "销售额",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "销售预测");
        }
    }
}
