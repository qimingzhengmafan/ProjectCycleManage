using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixTableStructure25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AprilCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "4月修正数量");

            migrationBuilder.AddColumn<double>(
                name: "AugustCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "8月修正数量");

            migrationBuilder.AddColumn<double>(
                name: "DecemberCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "12月修正数量");

            migrationBuilder.AddColumn<double>(
                name: "FebruaryCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "2月修正数量");

            migrationBuilder.AddColumn<double>(
                name: "JanuaryCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "1月修正数量");

            migrationBuilder.AddColumn<double>(
                name: "JulyCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "7月修正数量");

            migrationBuilder.AddColumn<double>(
                name: "JuneCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "6月修正数量");

            migrationBuilder.AddColumn<double>(
                name: "MarchCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "3月修正数量");

            migrationBuilder.AddColumn<double>(
                name: "MayCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "5月修正数量");

            migrationBuilder.AddColumn<double>(
                name: "NovemberCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "11月修正数量");

            migrationBuilder.AddColumn<double>(
                name: "OctoberCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "10月修正数量");

            migrationBuilder.AddColumn<double>(
                name: "SeptemberCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "9月修正数量");

            migrationBuilder.AddColumn<double>(
                name: "AprilCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "4月修正金额");

            migrationBuilder.AddColumn<double>(
                name: "AugustCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "8月修正金额");

            migrationBuilder.AddColumn<double>(
                name: "DecemberCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "12月修正金额");

            migrationBuilder.AddColumn<double>(
                name: "FebruaryCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "2月修正金额");

            migrationBuilder.AddColumn<double>(
                name: "JanuaryCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "1月修正金额");

            migrationBuilder.AddColumn<double>(
                name: "JulyCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "7月修正金额");

            migrationBuilder.AddColumn<double>(
                name: "JuneCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "6月修正金额");

            migrationBuilder.AddColumn<double>(
                name: "MarchCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "3月修正金额");

            migrationBuilder.AddColumn<double>(
                name: "MayCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "5月修正金额");

            migrationBuilder.AddColumn<double>(
                name: "NovemberCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "11月修正金额");

            migrationBuilder.AddColumn<double>(
                name: "OctoberCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "10月修正金额");

            migrationBuilder.AddColumn<double>(
                name: "SeptemberCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "9月修正金额");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AprilCorrection",
                table: "RevOfAssetQuantTab");

            migrationBuilder.DropColumn(
                name: "AugustCorrection",
                table: "RevOfAssetQuantTab");

            migrationBuilder.DropColumn(
                name: "DecemberCorrection",
                table: "RevOfAssetQuantTab");

            migrationBuilder.DropColumn(
                name: "FebruaryCorrection",
                table: "RevOfAssetQuantTab");

            migrationBuilder.DropColumn(
                name: "JanuaryCorrection",
                table: "RevOfAssetQuantTab");

            migrationBuilder.DropColumn(
                name: "JulyCorrection",
                table: "RevOfAssetQuantTab");

            migrationBuilder.DropColumn(
                name: "JuneCorrection",
                table: "RevOfAssetQuantTab");

            migrationBuilder.DropColumn(
                name: "MarchCorrection",
                table: "RevOfAssetQuantTab");

            migrationBuilder.DropColumn(
                name: "MayCorrection",
                table: "RevOfAssetQuantTab");

            migrationBuilder.DropColumn(
                name: "NovemberCorrection",
                table: "RevOfAssetQuantTab");

            migrationBuilder.DropColumn(
                name: "OctoberCorrection",
                table: "RevOfAssetQuantTab");

            migrationBuilder.DropColumn(
                name: "SeptemberCorrection",
                table: "RevOfAssetQuantTab");

            migrationBuilder.DropColumn(
                name: "AprilCorrection",
                table: "AsAmountCorrectTab");

            migrationBuilder.DropColumn(
                name: "AugustCorrection",
                table: "AsAmountCorrectTab");

            migrationBuilder.DropColumn(
                name: "DecemberCorrection",
                table: "AsAmountCorrectTab");

            migrationBuilder.DropColumn(
                name: "FebruaryCorrection",
                table: "AsAmountCorrectTab");

            migrationBuilder.DropColumn(
                name: "JanuaryCorrection",
                table: "AsAmountCorrectTab");

            migrationBuilder.DropColumn(
                name: "JulyCorrection",
                table: "AsAmountCorrectTab");

            migrationBuilder.DropColumn(
                name: "JuneCorrection",
                table: "AsAmountCorrectTab");

            migrationBuilder.DropColumn(
                name: "MarchCorrection",
                table: "AsAmountCorrectTab");

            migrationBuilder.DropColumn(
                name: "MayCorrection",
                table: "AsAmountCorrectTab");

            migrationBuilder.DropColumn(
                name: "NovemberCorrection",
                table: "AsAmountCorrectTab");

            migrationBuilder.DropColumn(
                name: "OctoberCorrection",
                table: "AsAmountCorrectTab");

            migrationBuilder.DropColumn(
                name: "SeptemberCorrection",
                table: "AsAmountCorrectTab");
        }
    }
}
