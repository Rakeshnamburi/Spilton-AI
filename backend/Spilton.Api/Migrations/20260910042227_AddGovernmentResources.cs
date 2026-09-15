using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spilton.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddGovernmentResources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GovernmentResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpaceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExamId = table.Column<Guid>(type: "uuid", nullable: true),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Kind = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Organization = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    NotificationNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SourceUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    SourceType = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Verification = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    SourceHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    VerifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    PublishedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: true),
                    Stage = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Subject = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Language = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Shift = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Summary = table.Column<string>(type: "character varying(700)", maxLength: 700, nullable: false),
                    IsSaved = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovernmentResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GovernmentResources_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GovernmentResources_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GovernmentResources_Spaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "Spaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GovernmentResources_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GovernmentResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Quote = table.Column<string>(type: "character varying(1800)", maxLength: 1800, nullable: false),
                    ChunkId = table.Column<Guid>(type: "uuid", nullable: false),
                    Page = table.Column<int>(type: "integer", nullable: true),
                    Section = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Confidence = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationFields_GovernmentResources_GovernmentResourceId",
                        column: x => x.GovernmentResourceId,
                        principalTable: "GovernmentResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GovernmentResources_DocumentId",
                table: "GovernmentResources",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_GovernmentResources_ExamId",
                table: "GovernmentResources",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_GovernmentResources_SpaceId",
                table: "GovernmentResources",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_GovernmentResources_UserId_SpaceId_Kind",
                table: "GovernmentResources",
                columns: new[] { "UserId", "SpaceId", "Kind" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationFields_GovernmentResourceId",
                table: "NotificationFields",
                column: "GovernmentResourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationFields");

            migrationBuilder.DropTable(
                name: "GovernmentResources");
        }
    }
}
