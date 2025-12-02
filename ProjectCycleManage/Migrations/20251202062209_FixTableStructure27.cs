using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixTableStructure27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "EquipTypeStageInfoTable",
                type: "longtext",
                nullable: false,
                comment: "文档状态，Nece-必要，Abolish-废除")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "EquipTypeStageDocTable",
                type: "longtext",
                nullable: false,
                comment: "文档状态，Nece-必要，Abolish-废除")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "EquipTypeStageInfoTable");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EquipTypeStageDocTable");
        }
    }
}
