using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixTableStructure13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "InspectionRecord",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "顺序");

            migrationBuilder.AddColumn<int>(
                name: "projId",
                table: "InspectionRecord",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "流程ID记录");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRecord_projId",
                table: "InspectionRecord",
                column: "projId");

            migrationBuilder.AddForeignKey(
                name: "FK_InspectionRecord_ProjFlowTable_projId",
                table: "InspectionRecord",
                column: "projId",
                principalTable: "ProjFlowTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InspectionRecord_ProjFlowTable_projId",
                table: "InspectionRecord");

            migrationBuilder.DropIndex(
                name: "IX_InspectionRecord_projId",
                table: "InspectionRecord");

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "InspectionRecord");

            migrationBuilder.DropColumn(
                name: "projId",
                table: "InspectionRecord");
        }
    }
}
