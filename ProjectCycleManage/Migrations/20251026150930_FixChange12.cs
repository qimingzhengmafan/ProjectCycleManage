using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixChange12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InforTypesDataId",
                table: "InformationTable",
                type: "int",
                nullable: true,
                comment: "信息类型-例-信息-时间");

            migrationBuilder.AddColumn<int>(
                name: "FileTypesDataId",
                table: "DocumentType",
                type: "int",
                nullable: true,
                comment: "文档类型-例-文档-OA");

            migrationBuilder.CreateTable(
                name: "FileTypesTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FileTypesName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileTypesTable", x => x.Id);
                },
                comment: "文件类型")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_InformationTable_InforTypesDataId",
                table: "InformationTable",
                column: "InforTypesDataId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentType_FileTypesDataId",
                table: "DocumentType",
                column: "FileTypesDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentType_FileTypesTable_FileTypesDataId",
                table: "DocumentType",
                column: "FileTypesDataId",
                principalTable: "FileTypesTable",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InformationTable_FileTypesTable_InforTypesDataId",
                table: "InformationTable",
                column: "InforTypesDataId",
                principalTable: "FileTypesTable",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentType_FileTypesTable_FileTypesDataId",
                table: "DocumentType");

            migrationBuilder.DropForeignKey(
                name: "FK_InformationTable_FileTypesTable_InforTypesDataId",
                table: "InformationTable");

            migrationBuilder.DropTable(
                name: "FileTypesTable");

            migrationBuilder.DropIndex(
                name: "IX_InformationTable_InforTypesDataId",
                table: "InformationTable");

            migrationBuilder.DropIndex(
                name: "IX_DocumentType_FileTypesDataId",
                table: "DocumentType");

            migrationBuilder.DropColumn(
                name: "InforTypesDataId",
                table: "InformationTable");

            migrationBuilder.DropColumn(
                name: "FileTypesDataId",
                table: "DocumentType");
        }
    }
}
