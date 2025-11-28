using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixTableStructure26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "SeptemberCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: true,
                comment: "9月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "9月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "OctoberCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: true,
                comment: "10月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "10月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "NovemberCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: true,
                comment: "11月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "11月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "MayCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: true,
                comment: "5月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "5月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "MarchCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: true,
                comment: "3月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "3月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "JuneCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: true,
                comment: "6月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "6月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "JulyCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: true,
                comment: "7月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "7月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "JanuaryCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: true,
                comment: "1月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "1月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "FebruaryCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: true,
                comment: "2月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "2月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "DecemberCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: true,
                comment: "12月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "12月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "AugustCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: true,
                comment: "8月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "8月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "AprilCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: true,
                comment: "4月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "4月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "SeptemberCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: true,
                comment: "9月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "9月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "OctoberCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: true,
                comment: "10月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "10月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "NovemberCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: true,
                comment: "11月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "11月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "MayCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: true,
                comment: "5月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "5月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "MarchCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: true,
                comment: "3月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "3月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "JuneCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: true,
                comment: "6月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "6月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "JulyCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: true,
                comment: "7月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "7月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "JanuaryCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: true,
                comment: "1月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "1月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "FebruaryCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: true,
                comment: "2月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "2月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "DecemberCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: true,
                comment: "12月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "12月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "AugustCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: true,
                comment: "8月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "8月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "AprilCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: true,
                comment: "4月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldComment: "4月修正金额");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "SeptemberCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "9月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "9月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "OctoberCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "10月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "10月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "NovemberCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "11月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "11月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "MayCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "5月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "5月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "MarchCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "3月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "3月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "JuneCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "6月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "6月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "JulyCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "7月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "7月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "JanuaryCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "1月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "1月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "FebruaryCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "2月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "2月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "DecemberCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "12月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "12月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "AugustCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "8月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "8月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "AprilCorrection",
                table: "RevOfAssetQuantTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "4月修正数量",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "4月修正数量");

            migrationBuilder.AlterColumn<double>(
                name: "SeptemberCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "9月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "9月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "OctoberCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "10月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "10月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "NovemberCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "11月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "11月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "MayCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "5月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "5月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "MarchCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "3月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "3月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "JuneCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "6月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "6月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "JulyCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "7月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "7月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "JanuaryCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "1月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "1月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "FebruaryCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "2月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "2月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "DecemberCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "12月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "12月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "AugustCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "8月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "8月修正金额");

            migrationBuilder.AlterColumn<double>(
                name: "AprilCorrection",
                table: "AsAmountCorrectTab",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                comment: "4月修正金额",
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldComment: "4月修正金额");
        }
    }
}
