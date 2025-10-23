using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixTableStructure2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTypeDocumentAssociationTables_Projects_ProjectsId",
                table: "ProjectTypeDocumentAssociationTables");

            migrationBuilder.DropColumn(
                name: "DaysDiff",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "ProjectsId",
                table: "ProjectTypeDocumentAssociationTables",
                newName: "EquipmentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTypeDocumentAssociationTables_ProjectsId",
                table: "ProjectTypeDocumentAssociationTables",
                newName: "IX_ProjectTypeDocumentAssociationTables_EquipmentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTypeDocumentAssociationTables_EquipmentType_Equipment~",
                table: "ProjectTypeDocumentAssociationTables",
                column: "EquipmentTypeId",
                principalTable: "EquipmentType",
                principalColumn: "EquipmentTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTypeDocumentAssociationTables_EquipmentType_Equipment~",
                table: "ProjectTypeDocumentAssociationTables");

            migrationBuilder.RenameColumn(
                name: "EquipmentTypeId",
                table: "ProjectTypeDocumentAssociationTables",
                newName: "ProjectsId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTypeDocumentAssociationTables_EquipmentTypeId",
                table: "ProjectTypeDocumentAssociationTables",
                newName: "IX_ProjectTypeDocumentAssociationTables_ProjectsId");

            migrationBuilder.AddColumn<int>(
                name: "DaysDiff",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTypeDocumentAssociationTables_Projects_ProjectsId",
                table: "ProjectTypeDocumentAssociationTables",
                column: "ProjectsId",
                principalTable: "Projects",
                principalColumn: "ProjectsId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
