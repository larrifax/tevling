using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tevling.Migrations
{
    /// <inheritdoc />
    public partial class MigrateActivityTypesToRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Migrate existing Challenge ActivityTypes to ChallengeActivityTypes with default multiplier of 1.0
            migrationBuilder.Sql(@"
                INSERT INTO ChallengeActivityTypes (ActivityType, Multiplier, ChallengeId)
                SELECT 
                    CAST(value AS INTEGER) as ActivityType,
                    1.0 as Multiplier,
                    c.Id as ChallengeId
                FROM Challenges c, json_each(c.ActivityTypes) 
                WHERE c.ActivityTypes IS NOT NULL AND c.ActivityTypes != '[]'
            ");

            // Migrate existing ChallengeTemplate ActivityTypes to ChallengeTemplateActivityTypes with default multiplier of 1.0
            migrationBuilder.Sql(@"
                INSERT INTO ChallengeTemplateActivityTypes (ActivityType, Multiplier, ChallengeTemplateId)
                SELECT 
                    CAST(value AS INTEGER) as ActivityType,
                    1.0 as Multiplier,
                    ct.Id as ChallengeTemplateId
                FROM ChallengeTemplates ct, json_each(ct.ActivityTypes)
                WHERE ct.ActivityTypes IS NOT NULL AND ct.ActivityTypes != '[]'
            ");

            migrationBuilder.DropColumn(
                name: "ActivityTypes",
                table: "ChallengeTemplates");

            migrationBuilder.DropColumn(
                name: "ActivityTypes",
                table: "Challenges");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActivityTypes",
                table: "ChallengeTemplates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ActivityTypes",
                table: "Challenges",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
