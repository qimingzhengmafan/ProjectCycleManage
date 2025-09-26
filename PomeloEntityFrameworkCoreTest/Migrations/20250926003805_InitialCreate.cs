using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PomeloEntityFrameworkCoreTest.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentType",
                columns: table => new
                {
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DocumentTypeName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentType", x => x.DocumentTypeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipmentType",
                columns: table => new
                {
                    EquipmentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EquipmentName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentType", x => x.EquipmentTypeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PeopleTable",
                columns: table => new
                {
                    PeopleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PeopleName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeopleTable", x => x.PeopleId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProjectPhaseStatus",
                columns: table => new
                {
                    ProjectPhaseStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectPhaseStatusName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPhaseStatus", x => x.ProjectPhaseStatusId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProjectStage",
                columns: table => new
                {
                    ProjectStageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectStageName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStage", x => x.ProjectStageId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TypeTable",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TypeName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeTable", x => x.TypeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Year = table.Column<int>(type: "int", nullable: false),
                    ProcurementMonth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProjectName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectIdentifyingNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    equipmenttypeId = table.Column<int>(type: "int", nullable: false),
                    typeId = table.Column<int>(type: "int", nullable: false),
                    ProjectStageId = table.Column<int>(type: "int", nullable: false),
                    FinishTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Budget = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActualExpenditure = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectProgress = table.Column<int>(type: "int", nullable: false),
                    ProjectPhaseStatusId = table.Column<int>(type: "int", nullable: false),
                    ProjectLeaderId = table.Column<int>(type: "int", nullable: false),
                    projectfollowuppersonId = table.Column<int>(type: "int", nullable: false),
                    AssetNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    remarks = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectsId);
                    table.ForeignKey(
                        name: "FK_Projects_EquipmentType_equipmenttypeId",
                        column: x => x.equipmenttypeId,
                        principalTable: "EquipmentType",
                        principalColumn: "EquipmentTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_PeopleTable_ProjectLeaderId",
                        column: x => x.ProjectLeaderId,
                        principalTable: "PeopleTable",
                        principalColumn: "PeopleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_PeopleTable_projectfollowuppersonId",
                        column: x => x.projectfollowuppersonId,
                        principalTable: "PeopleTable",
                        principalColumn: "PeopleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_ProjectPhaseStatus_ProjectPhaseStatusId",
                        column: x => x.ProjectPhaseStatusId,
                        principalTable: "ProjectPhaseStatus",
                        principalColumn: "ProjectPhaseStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_ProjectStage_ProjectStageId",
                        column: x => x.ProjectStageId,
                        principalTable: "ProjectStage",
                        principalColumn: "ProjectStageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_TypeTable_typeId",
                        column: x => x.typeId,
                        principalTable: "TypeTable",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InspectionRecord",
                columns: table => new
                {
                    InspectionRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectsId = table.Column<int>(type: "int", nullable: false),
                    CheckPeopleId = table.Column<int>(type: "int", nullable: false),
                    CheckTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CheckResult = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CheckOpinion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionRecord", x => x.InspectionRecordId);
                    table.ForeignKey(
                        name: "FK_InspectionRecord_PeopleTable_CheckPeopleId",
                        column: x => x.CheckPeopleId,
                        principalTable: "PeopleTable",
                        principalColumn: "PeopleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionRecord_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "ProjectsId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProjectDocumentStatus",
                columns: table => new
                {
                    ProjectDocumentStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectsId = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    IsHasDocument = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TheLastUpDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatePeopleId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDocumentStatus", x => x.ProjectDocumentStatusId);
                    table.ForeignKey(
                        name: "FK_ProjectDocumentStatus_DocumentType_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentType",
                        principalColumn: "DocumentTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectDocumentStatus_PeopleTable_UpdatePeopleId",
                        column: x => x.UpdatePeopleId,
                        principalTable: "PeopleTable",
                        principalColumn: "PeopleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectDocumentStatus_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "ProjectsId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRecord_CheckPeopleId",
                table: "InspectionRecord",
                column: "CheckPeopleId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRecord_ProjectsId",
                table: "InspectionRecord",
                column: "ProjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDocumentStatus_DocumentTypeId",
                table: "ProjectDocumentStatus",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDocumentStatus_ProjectsId",
                table: "ProjectDocumentStatus",
                column: "ProjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDocumentStatus_UpdatePeopleId",
                table: "ProjectDocumentStatus",
                column: "UpdatePeopleId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_equipmenttypeId",
                table: "Projects",
                column: "equipmenttypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_projectfollowuppersonId",
                table: "Projects",
                column: "projectfollowuppersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectLeaderId",
                table: "Projects",
                column: "ProjectLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectPhaseStatusId",
                table: "Projects",
                column: "ProjectPhaseStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectStageId",
                table: "Projects",
                column: "ProjectStageId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_typeId",
                table: "Projects",
                column: "typeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InspectionRecord");

            migrationBuilder.DropTable(
                name: "ProjectDocumentStatus");

            migrationBuilder.DropTable(
                name: "DocumentType");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "EquipmentType");

            migrationBuilder.DropTable(
                name: "PeopleTable");

            migrationBuilder.DropTable(
                name: "ProjectPhaseStatus");

            migrationBuilder.DropTable(
                name: "ProjectStage");

            migrationBuilder.DropTable(
                name: "TypeTable");
        }
    }
}
