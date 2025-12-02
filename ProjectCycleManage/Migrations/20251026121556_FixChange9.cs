using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixChange9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckData",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Projects");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectCycle",
                table: "Projects",
                type: "int",
                nullable: true,
                comment: "项目周期",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Applicant",
                table: "Projects",
                type: "longtext",
                nullable: true,
                comment: "申请人")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicationTime",
                table: "Projects",
                type: "datetime(6)",
                nullable: true,
                comment: "申请时间");

            migrationBuilder.AddColumn<string>(
                name: "UsingDepartment",
                table: "Projects",
                type: "longtext",
                nullable: true,
                comment: "使用部门")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Permission",
                table: "PeopleTable",
                type: "longtext",
                nullable: true,
                comment: "权限信息")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Permission",
                table: "DocumentType",
                type: "longtext",
                nullable: true,
                comment: "权限分级")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipTypeStageDocTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    equipmenttypeId = table.Column<int>(type: "int", nullable: true, comment: "设备类型"),
                    ProjectStageId = table.Column<int>(type: "int", nullable: false, comment: "阶段-例-项目评审"),
                    documenttypeId = table.Column<int>(type: "int", nullable: false, comment: "文档-例-设备名称")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipTypeStageDocTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipTypeStageDocTable_DocumentType_documenttypeId",
                        column: x => x.documenttypeId,
                        principalTable: "DocumentType",
                        principalColumn: "DocumentTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipTypeStageDocTable_EquipmentType_equipmenttypeId",
                        column: x => x.equipmenttypeId,
                        principalTable: "EquipmentType",
                        principalColumn: "EquipmentTypeId");
                    table.ForeignKey(
                        name: "FK_EquipTypeStageDocTable_ProjectStage_ProjectStageId",
                        column: x => x.ProjectStageId,
                        principalTable: "ProjectStage",
                        principalColumn: "ProjectStageId",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "设备类型-阶段-文档对应表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InformationTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Infor = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Permission = table.Column<string>(type: "longtext", nullable: true, comment: "权限分级")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformationTable", x => x.Id);
                },
                comment: "权限信息表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PermInfoTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PermName = table.Column<string>(type: "longtext", nullable: false, comment: "权限名称-例-跟进人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Perm = table.Column<int>(type: "int", nullable: false, comment: "权限等级")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermInfoTable", x => x.Id);
                },
                comment: "权限信息表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProjFlowTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjFlowInfor = table.Column<string>(type: "longtext", nullable: false, comment: "流程信息-例-项目评审")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjFlowTable", x => x.Id);
                },
                comment: "项目流程表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipTypeStageInfoTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    equipmenttypeId = table.Column<int>(type: "int", nullable: true, comment: "设备类型"),
                    ProjectStageId = table.Column<int>(type: "int", nullable: false, comment: "阶段-例-项目评审"),
                    InformationId = table.Column<int>(type: "int", nullable: false, comment: "信息-例-设备名称")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipTypeStageInfoTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipTypeStageInfoTable_EquipmentType_equipmenttypeId",
                        column: x => x.equipmenttypeId,
                        principalTable: "EquipmentType",
                        principalColumn: "EquipmentTypeId");
                    table.ForeignKey(
                        name: "FK_EquipTypeStageInfoTable_InformationTable_InformationId",
                        column: x => x.InformationId,
                        principalTable: "InformationTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipTypeStageInfoTable_ProjectStage_ProjectStageId",
                        column: x => x.ProjectStageId,
                        principalTable: "ProjectStage",
                        principalColumn: "ProjectStageId",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "设备类型-阶段-信息对应表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TypeApprFlowPersSeqTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    equipmenttypeId = table.Column<int>(type: "int", nullable: true, comment: "设备类型"),
                    projectflowId = table.Column<int>(type: "int", nullable: true, comment: "流程信息"),
                    ReviewerPeopleId = table.Column<int>(type: "int", nullable: true, comment: "审核人员"),
                    Sequence = table.Column<int>(type: "int", nullable: true, comment: "顺序"),
                    Mark = table.Column<string>(type: "longtext", nullable: true, comment: "标记-Dele表示删除")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeApprFlowPersSeqTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeApprFlowPersSeqTable_EquipmentType_equipmenttypeId",
                        column: x => x.equipmenttypeId,
                        principalTable: "EquipmentType",
                        principalColumn: "EquipmentTypeId");
                    table.ForeignKey(
                        name: "FK_TypeApprFlowPersSeqTable_PeopleTable_ReviewerPeopleId",
                        column: x => x.ReviewerPeopleId,
                        principalTable: "PeopleTable",
                        principalColumn: "PeopleId");
                    table.ForeignKey(
                        name: "FK_TypeApprFlowPersSeqTable_ProjFlowTable_projectflowId",
                        column: x => x.projectflowId,
                        principalTable: "ProjFlowTable",
                        principalColumn: "Id");
                },
                comment: "类型审批流程人员顺序表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EquipTypeStageDocTable_documenttypeId",
                table: "EquipTypeStageDocTable",
                column: "documenttypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipTypeStageDocTable_equipmenttypeId",
                table: "EquipTypeStageDocTable",
                column: "equipmenttypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipTypeStageDocTable_ProjectStageId",
                table: "EquipTypeStageDocTable",
                column: "ProjectStageId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipTypeStageInfoTable_equipmenttypeId",
                table: "EquipTypeStageInfoTable",
                column: "equipmenttypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipTypeStageInfoTable_InformationId",
                table: "EquipTypeStageInfoTable",
                column: "InformationId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipTypeStageInfoTable_ProjectStageId",
                table: "EquipTypeStageInfoTable",
                column: "ProjectStageId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeApprFlowPersSeqTable_equipmenttypeId",
                table: "TypeApprFlowPersSeqTable",
                column: "equipmenttypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeApprFlowPersSeqTable_projectflowId",
                table: "TypeApprFlowPersSeqTable",
                column: "projectflowId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeApprFlowPersSeqTable_ReviewerPeopleId",
                table: "TypeApprFlowPersSeqTable",
                column: "ReviewerPeopleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipTypeStageDocTable");

            migrationBuilder.DropTable(
                name: "EquipTypeStageInfoTable");

            migrationBuilder.DropTable(
                name: "PermInfoTable");

            migrationBuilder.DropTable(
                name: "TypeApprFlowPersSeqTable");

            migrationBuilder.DropTable(
                name: "InformationTable");

            migrationBuilder.DropTable(
                name: "ProjFlowTable");

            migrationBuilder.DropColumn(
                name: "Applicant",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ApplicationTime",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UsingDepartment",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Permission",
                table: "PeopleTable");

            migrationBuilder.DropColumn(
                name: "Permission",
                table: "DocumentType");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectCycle",
                table: "Projects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "项目周期");

            migrationBuilder.AddColumn<string>(
                name: "CheckData",
                table: "Projects",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Projects",
                type: "datetime(6)",
                nullable: true);
        }
    }
}
