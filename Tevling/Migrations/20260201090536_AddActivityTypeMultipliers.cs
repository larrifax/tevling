using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tevling.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityTypeMultipliers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChallengeActivityTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActivityType = table.Column<int>(type: "INTEGER", nullable: false),
                    Multiplier = table.Column<double>(type: "REAL", nullable: false),
                    ChallengeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeActivityTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeActivityTypes_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeTemplateActivityTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActivityType = table.Column<int>(type: "INTEGER", nullable: false),
                    Multiplier = table.Column<double>(type: "REAL", nullable: false),
                    ChallengeTemplateId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeTemplateActivityTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeTemplateActivityTypes_ChallengeTemplates_ChallengeTemplateId",
                        column: x => x.ChallengeTemplateId,
                        principalTable: "ChallengeTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeActivityTypes_ChallengeId",
                table: "ChallengeActivityTypes",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeTemplateActivityTypes_ChallengeTemplateId",
                table: "ChallengeTemplateActivityTypes",
                column: "ChallengeTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeActivityTypes");

            migrationBuilder.DropTable(
                name: "ChallengeTemplateActivityTypes");
        }
    }
}
