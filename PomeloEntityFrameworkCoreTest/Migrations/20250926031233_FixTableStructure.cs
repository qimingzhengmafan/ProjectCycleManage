using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PomeloEntityFrameworkCoreTest.Migrations
{
    /// <inheritdoc />
    public partial class FixTableStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProjectProgress",
                table: "Projects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ProjectTypeDocumentAssociationTables",
                columns: table => new
                {
                    ProjectTypeDocumentAssociationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectsId = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    IsNecessary = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTypeDocumentAssociationTables", x => x.ProjectTypeDocumentAssociationId);
                    table.ForeignKey(
                        name: "FK_ProjectTypeDocumentAssociationTables_DocumentType_DocumentTy~",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentType",
                        principalColumn: "DocumentTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTypeDocumentAssociationTables_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "ProjectsId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTypeDocumentAssociationTables_DocumentTypeId",
                table: "ProjectTypeDocumentAssociationTables",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTypeDocumentAssociationTables_ProjectsId",
                table: "ProjectTypeDocumentAssociationTables",
                column: "ProjectsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTypeDocumentAssociationTables");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectProgress",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
