using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixTableStructure14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjInforId",
                table: "Projects",
                type: "int",
                nullable: true,
                comment: "流程ID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjInforId",
                table: "Projects",
                column: "ProjInforId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjFlowTable_ProjInforId",
                table: "Projects",
                column: "ProjInforId",
                principalTable: "ProjFlowTable",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjFlowTable_ProjInforId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjInforId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjInforId",
                table: "Projects");
        }
    }
}
