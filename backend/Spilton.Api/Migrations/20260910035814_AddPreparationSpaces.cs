using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Spilton.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPreparationSpaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SpaceId",
                table: "Documents",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SpaceId",
                table: "Conversations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Family = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spaces_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamStages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamStages_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpaceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Description = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: false),
                    TargetDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ProgressPercent = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goals_Spaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "Spaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Goals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Memories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpaceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Content = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Source = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memories_Spaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "Spaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Memories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudyPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpaceId = table.Column<Guid>(type: "uuid", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Days = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyPlans_Spaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "Spaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudyPlans_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamStageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_ExamStages_ExamStageId",
                        column: x => x.ExamStageId,
                        principalTable: "ExamStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserExamProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpaceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExamStageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpectedExamDate = table.Column<DateOnly>(type: "date", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DailyMinutes = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PreferredLanguage = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    TargetScore = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExamProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExamProfiles_ExamStages_ExamStageId",
                        column: x => x.ExamStageId,
                        principalTable: "ExamStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserExamProfiles_Spaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "Spaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserExamProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudyPlanItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudyPlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Minutes = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Reason = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlanItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyPlanItems_StudyPlans_StudyPlanId",
                        column: x => x.StudyPlanId,
                        principalTable: "StudyPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudyPlanItems_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTopicProgress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserExamProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    QuestionsAttempted = table.Column<int>(type: "integer", nullable: false),
                    QuestionsCorrect = table.Column<int>(type: "integer", nullable: false),
                    ConfidenceScore = table.Column<int>(type: "integer", nullable: true),
                    LastPracticedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTopicProgress", x => x.Id);
                    table.CheckConstraint("CK_Progress_Counts", "\"QuestionsAttempted\" >= 0 AND \"QuestionsCorrect\" >= 0 AND \"QuestionsCorrect\" <= \"QuestionsAttempted\"");
                    table.ForeignKey(
                        name: "FK_UserTopicProgress_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTopicProgress_UserExamProfiles_UserExamProfileId",
                        column: x => x.UserExamProfileId,
                        principalTable: "UserExamProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Exams",
                columns: new[] { "Id", "Family", "Name" },
                values: new object[,]
                {
                    { new Guid("0838d026-1d31-25ad-921b-4f88b7fd16fc"), "APPSC", "APPSC Preparation" },
                    { new Guid("25b6217a-7b02-4fc6-13a6-3a9b3bdc187a"), "UPSC", "UPSC Preparation" },
                    { new Guid("5203a36e-3f14-28c5-3cf5-dbfb9e8dbdc7"), "SSC", "SSC CGL" },
                    { new Guid("67a09dd0-b1f9-3056-07f7-38bfce69de44"), "RRB", "RRB NTPC" },
                    { new Guid("bf3e0171-deb0-4a7a-6220-a554625c1e20"), "Banking", "Banking Preparation" },
                    { new Guid("c7eb30df-82ba-b681-16bc-d42281260567"), "SSC", "SSC CHSL" }
                });

            migrationBuilder.InsertData(
                table: "ExamStages",
                columns: new[] { "Id", "ExamId", "Name" },
                values: new object[,]
                {
                    { new Guid("05887fde-7a0e-b43f-d508-f1cfa7bf75d1"), new Guid("67a09dd0-b1f9-3056-07f7-38bfce69de44"), "General preparation" },
                    { new Guid("19ca8ca9-d21f-575f-7af9-3310e764aa70"), new Guid("0838d026-1d31-25ad-921b-4f88b7fd16fc"), "General preparation" },
                    { new Guid("363fe794-8cd0-f203-f40c-95247fef1ecc"), new Guid("c7eb30df-82ba-b681-16bc-d42281260567"), "Tier 2" },
                    { new Guid("4b4b85c5-7efc-17c3-ea31-575f3fde09c0"), new Guid("5203a36e-3f14-28c5-3cf5-dbfb9e8dbdc7"), "Tier 1" },
                    { new Guid("5d62f58a-58ac-9e20-37c8-5ee91b6928c4"), new Guid("25b6217a-7b02-4fc6-13a6-3a9b3bdc187a"), "General preparation" },
                    { new Guid("6e360767-ff22-4509-7328-03511808a4ab"), new Guid("bf3e0171-deb0-4a7a-6220-a554625c1e20"), "General preparation" },
                    { new Guid("6eaea10a-e489-75bd-be06-38b2bdd1c22e"), new Guid("5203a36e-3f14-28c5-3cf5-dbfb9e8dbdc7"), "Tier 2" },
                    { new Guid("799d3509-b2d3-1051-8a21-e8ec1ac8dbf4"), new Guid("c7eb30df-82ba-b681-16bc-d42281260567"), "Tier 1" }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "ExamStageId", "Name" },
                values: new object[,]
                {
                    { new Guid("02db2b9a-c84b-5ce1-0f3c-68969f7e554c"), new Guid("05887fde-7a0e-b43f-d508-f1cfa7bf75d1"), "Quantitative Aptitude" },
                    { new Guid("08a8470e-bb21-548e-0a08-60a5f3bff9df"), new Guid("363fe794-8cd0-f203-f40c-95247fef1ecc"), "General Awareness" },
                    { new Guid("12e911ad-fe68-6d34-81aa-2a3e423479fe"), new Guid("05887fde-7a0e-b43f-d508-f1cfa7bf75d1"), "General Awareness" },
                    { new Guid("1cf1914a-70a9-0732-71a9-2658384fa516"), new Guid("363fe794-8cd0-f203-f40c-95247fef1ecc"), "English" },
                    { new Guid("1f274bfd-5c4d-b4fe-522d-2fc070566c42"), new Guid("5d62f58a-58ac-9e20-37c8-5ee91b6928c4"), "English" },
                    { new Guid("21adc355-8586-e349-28e8-3a4492e3c8a6"), new Guid("6eaea10a-e489-75bd-be06-38b2bdd1c22e"), "General Awareness" },
                    { new Guid("2aded84d-55ba-7fe6-5912-6eb2e4108adb"), new Guid("6e360767-ff22-4509-7328-03511808a4ab"), "Reasoning" },
                    { new Guid("2b9c687b-8c73-8bec-723f-33d1b2ed4c49"), new Guid("6eaea10a-e489-75bd-be06-38b2bdd1c22e"), "Reasoning" },
                    { new Guid("2dcd5d84-67cb-7ae2-4f7e-7fb7699ae2df"), new Guid("799d3509-b2d3-1051-8a21-e8ec1ac8dbf4"), "General Awareness" },
                    { new Guid("2f7d3cf3-0488-9ba8-c202-25354dcddf64"), new Guid("05887fde-7a0e-b43f-d508-f1cfa7bf75d1"), "English" },
                    { new Guid("327fe777-c46f-0930-3f8f-744612e374b0"), new Guid("5d62f58a-58ac-9e20-37c8-5ee91b6928c4"), "Quantitative Aptitude" },
                    { new Guid("3ae9430f-30bb-52a1-643c-f18ee6522b95"), new Guid("5d62f58a-58ac-9e20-37c8-5ee91b6928c4"), "Reasoning" },
                    { new Guid("3d7d71ba-9120-b239-d696-715b35ee5535"), new Guid("19ca8ca9-d21f-575f-7af9-3310e764aa70"), "General Awareness" },
                    { new Guid("4a5581f2-39c2-c9c7-50ad-954a957f1b07"), new Guid("4b4b85c5-7efc-17c3-ea31-575f3fde09c0"), "Reasoning" },
                    { new Guid("52732bab-625e-b767-0dea-6f39a2f403cb"), new Guid("6eaea10a-e489-75bd-be06-38b2bdd1c22e"), "English" },
                    { new Guid("593ab663-779a-cd4b-e7c3-9b0e29a0aea9"), new Guid("6e360767-ff22-4509-7328-03511808a4ab"), "English" },
                    { new Guid("6c42f073-e167-13ea-1e64-4667f30548d2"), new Guid("363fe794-8cd0-f203-f40c-95247fef1ecc"), "Quantitative Aptitude" },
                    { new Guid("6e95c6b1-5db5-855a-cc9b-5be0a1bcf838"), new Guid("799d3509-b2d3-1051-8a21-e8ec1ac8dbf4"), "Reasoning" },
                    { new Guid("75bb5701-e8eb-3fba-15ea-f4d403ccde0e"), new Guid("19ca8ca9-d21f-575f-7af9-3310e764aa70"), "Reasoning" },
                    { new Guid("87b72a5b-9923-e0a8-542c-95024f4d41a0"), new Guid("19ca8ca9-d21f-575f-7af9-3310e764aa70"), "English" },
                    { new Guid("898eb630-9c17-9ee7-c375-739dc4c14ead"), new Guid("4b4b85c5-7efc-17c3-ea31-575f3fde09c0"), "English" },
                    { new Guid("899ad66e-cb98-e49a-6994-05c201d848fa"), new Guid("4b4b85c5-7efc-17c3-ea31-575f3fde09c0"), "General Awareness" },
                    { new Guid("90347abb-8fcd-7d22-748c-00c8bcf30b49"), new Guid("799d3509-b2d3-1051-8a21-e8ec1ac8dbf4"), "English" },
                    { new Guid("95c19115-0ff0-2d83-7ea8-8fa8ec49d3bd"), new Guid("5d62f58a-58ac-9e20-37c8-5ee91b6928c4"), "General Awareness" },
                    { new Guid("a0fe7782-735b-c64b-5ee0-ef642bd4b412"), new Guid("6eaea10a-e489-75bd-be06-38b2bdd1c22e"), "Quantitative Aptitude" },
                    { new Guid("a98cd19f-ab5f-c3a3-bfaa-b5bf97ad8026"), new Guid("4b4b85c5-7efc-17c3-ea31-575f3fde09c0"), "Quantitative Aptitude" },
                    { new Guid("ab42a4f6-2ca3-1387-f605-7541d3bdc964"), new Guid("6e360767-ff22-4509-7328-03511808a4ab"), "General Awareness" },
                    { new Guid("af68f49f-904d-04cc-ab49-e20ae7b87985"), new Guid("6e360767-ff22-4509-7328-03511808a4ab"), "Quantitative Aptitude" },
                    { new Guid("b1979e05-7e07-b2f8-d3f8-821dbb1ce6dd"), new Guid("363fe794-8cd0-f203-f40c-95247fef1ecc"), "Reasoning" },
                    { new Guid("c48473f0-e6ac-dd2b-2aca-5c9ed9ca886b"), new Guid("799d3509-b2d3-1051-8a21-e8ec1ac8dbf4"), "Quantitative Aptitude" },
                    { new Guid("e0368c2d-0969-a6d1-09bd-8e27f2e79993"), new Guid("05887fde-7a0e-b43f-d508-f1cfa7bf75d1"), "Reasoning" },
                    { new Guid("ef6d15a8-abfc-4a2b-ac8d-5033319bc397"), new Guid("19ca8ca9-d21f-575f-7af9-3310e764aa70"), "Quantitative Aptitude" }
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "Name", "SubjectId" },
                values: new object[,]
                {
                    { new Guid("006afb85-c37d-6d54-1243-585e54319538"), "Error Detection", new Guid("1cf1914a-70a9-0732-71a9-2658384fa516") },
                    { new Guid("01f4eab0-a154-61cd-f5fc-56eef6fdbe13"), "Logical Reasoning", new Guid("2b9c687b-8c73-8bec-723f-33d1b2ed4c49") },
                    { new Guid("0520243f-cc6e-cb5c-edc4-bce6d0c03e19"), "Percentage", new Guid("327fe777-c46f-0930-3f8f-744612e374b0") },
                    { new Guid("06b1fe07-1bf1-b969-ccbc-d50f8dc66fec"), "Vocabulary", new Guid("52732bab-625e-b767-0dea-6f39a2f403cb") },
                    { new Guid("094c94f6-10fb-987d-5def-9fd55898cccd"), "Geometry", new Guid("ef6d15a8-abfc-4a2b-ac8d-5033319bc397") },
                    { new Guid("0c88667f-08a5-ccaa-9eb7-04782a94abee"), "History", new Guid("899ad66e-cb98-e49a-6994-05c201d848fa") },
                    { new Guid("0e00b532-de5f-e958-fc3c-bff990da3ad7"), "History", new Guid("3d7d71ba-9120-b239-d696-715b35ee5535") },
                    { new Guid("14107032-8fb9-162f-55a5-50fe0e2d9589"), "Geography", new Guid("08a8470e-bb21-548e-0a08-60a5f3bff9df") },
                    { new Guid("17845109-183d-1709-ce68-934b7b9e3685"), "Coding-Decoding", new Guid("e0368c2d-0969-a6d1-09bd-8e27f2e79993") },
                    { new Guid("1e6274aa-ed80-cc26-e2d4-6d318310d57d"), "Geometry", new Guid("a0fe7782-735b-c64b-5ee0-ef642bd4b412") },
                    { new Guid("20012ca9-ebad-57d4-e71e-599966845b08"), "Error Detection", new Guid("1f274bfd-5c4d-b4fe-522d-2fc070566c42") },
                    { new Guid("20d8f7f2-f489-3229-dc4e-742370a8acf0"), "History", new Guid("2dcd5d84-67cb-7ae2-4f7e-7fb7699ae2df") },
                    { new Guid("22a8fbdb-66b6-8f15-d6f3-757c30fb9b0e"), "Geography", new Guid("2dcd5d84-67cb-7ae2-4f7e-7fb7699ae2df") },
                    { new Guid("27b89e12-3f6b-64a4-0250-e8d8042ddd95"), "Static GK", new Guid("ab42a4f6-2ca3-1387-f605-7541d3bdc964") },
                    { new Guid("286f2db0-b99d-2617-0702-69ee0d3540dc"), "Error Detection", new Guid("2f7d3cf3-0488-9ba8-c202-25354dcddf64") },
                    { new Guid("2a8a2294-1840-5693-82c1-25fc4adefba5"), "Static GK", new Guid("12e911ad-fe68-6d34-81aa-2a3e423479fe") },
                    { new Guid("2c551512-d791-2479-27dc-763d619598d3"), "History", new Guid("08a8470e-bb21-548e-0a08-60a5f3bff9df") },
                    { new Guid("2e357b40-0f2f-a20c-731a-9306154b8501"), "History", new Guid("95c19115-0ff0-2d83-7ea8-8fa8ec49d3bd") },
                    { new Guid("2ebfdf1f-2971-2d0f-f951-00063170d10e"), "Logical Reasoning", new Guid("4a5581f2-39c2-c9c7-50ad-954a957f1b07") },
                    { new Guid("2ec656d6-0d74-15d7-af98-429f1e323386"), "Reading Comprehension", new Guid("2f7d3cf3-0488-9ba8-c202-25354dcddf64") },
                    { new Guid("2ee79a5c-1817-7974-b10f-9556ddf79938"), "Static GK", new Guid("899ad66e-cb98-e49a-6994-05c201d848fa") },
                    { new Guid("334d3aaa-6503-7f88-3b06-a9a9294bd734"), "Profit and Loss", new Guid("c48473f0-e6ac-dd2b-2aca-5c9ed9ca886b") },
                    { new Guid("36b5ba94-5236-85de-f492-9e986fff2d5a"), "Reading Comprehension", new Guid("87b72a5b-9923-e0a8-542c-95024f4d41a0") },
                    { new Guid("39e33197-d66c-4dee-aac1-c712efff430b"), "Geometry", new Guid("c48473f0-e6ac-dd2b-2aca-5c9ed9ca886b") },
                    { new Guid("3d47e2f4-9c72-e951-443e-a7221f98238d"), "Profit and Loss", new Guid("6c42f073-e167-13ea-1e64-4667f30548d2") },
                    { new Guid("3dfa2226-b829-39a3-7e60-74f7813318f5"), "Geography", new Guid("95c19115-0ff0-2d83-7ea8-8fa8ec49d3bd") },
                    { new Guid("439b6a39-8457-bc3d-c730-5098be909350"), "Reading Comprehension", new Guid("1cf1914a-70a9-0732-71a9-2658384fa516") },
                    { new Guid("464915d3-d5e7-b39e-b10d-74e8d66fb544"), "Logical Reasoning", new Guid("75bb5701-e8eb-3fba-15ea-f4d403ccde0e") },
                    { new Guid("4673749d-37e7-bac2-96cd-a1d61dc9ab3f"), "Reading Comprehension", new Guid("52732bab-625e-b767-0dea-6f39a2f403cb") },
                    { new Guid("4dee5ee3-c624-7f52-61f9-591ee98d7b2a"), "History", new Guid("ab42a4f6-2ca3-1387-f605-7541d3bdc964") },
                    { new Guid("52662e2e-85b0-8144-a50b-4da97b33af29"), "Coding-Decoding", new Guid("2aded84d-55ba-7fe6-5912-6eb2e4108adb") },
                    { new Guid("5430fd48-a166-89f3-5111-39a8e6c491c3"), "Geometry", new Guid("af68f49f-904d-04cc-ab49-e20ae7b87985") },
                    { new Guid("5832a310-92ff-b5c6-bb5d-7309be073ca7"), "Coding-Decoding", new Guid("75bb5701-e8eb-3fba-15ea-f4d403ccde0e") },
                    { new Guid("5965fef8-8a16-72a9-e6c5-39c5f374de40"), "Series", new Guid("2b9c687b-8c73-8bec-723f-33d1b2ed4c49") },
                    { new Guid("59edbd10-66a3-bc0e-e02a-ac85e2b9d278"), "Static GK", new Guid("08a8470e-bb21-548e-0a08-60a5f3bff9df") },
                    { new Guid("5a91af83-a3c5-f4c8-83d8-dd35fa977f77"), "Error Detection", new Guid("898eb630-9c17-9ee7-c375-739dc4c14ead") },
                    { new Guid("5b475785-58a5-2cd6-24a4-83f0e4e450cc"), "Error Detection", new Guid("593ab663-779a-cd4b-e7c3-9b0e29a0aea9") },
                    { new Guid("5d3e3c61-4455-b80c-9ec9-2eab28bb721b"), "Geography", new Guid("899ad66e-cb98-e49a-6994-05c201d848fa") },
                    { new Guid("6483005c-f65a-318d-5a89-ca0bc7e4729d"), "Vocabulary", new Guid("593ab663-779a-cd4b-e7c3-9b0e29a0aea9") },
                    { new Guid("66128a1f-3059-b75e-f522-f128417cf043"), "Coding-Decoding", new Guid("b1979e05-7e07-b2f8-d3f8-821dbb1ce6dd") },
                    { new Guid("698bfa28-befc-6378-2794-69124e2dde80"), "Error Detection", new Guid("87b72a5b-9923-e0a8-542c-95024f4d41a0") },
                    { new Guid("69afcae3-e269-781d-d72c-6e4330e17b35"), "History", new Guid("12e911ad-fe68-6d34-81aa-2a3e423479fe") },
                    { new Guid("6c04f604-b96a-837e-8464-c1baa43feb1e"), "Static GK", new Guid("2dcd5d84-67cb-7ae2-4f7e-7fb7699ae2df") },
                    { new Guid("6c2e7c8d-0f31-ec8d-aaba-5b50303c8ef7"), "Series", new Guid("3ae9430f-30bb-52a1-643c-f18ee6522b95") },
                    { new Guid("6e1d1084-dce5-7b94-12ab-3438861d8056"), "Vocabulary", new Guid("2f7d3cf3-0488-9ba8-c202-25354dcddf64") },
                    { new Guid("72ce00f0-db1e-2474-089f-c736873706a9"), "Geography", new Guid("ab42a4f6-2ca3-1387-f605-7541d3bdc964") },
                    { new Guid("73b138b6-019d-6d5b-d67e-3d1aef783759"), "Logical Reasoning", new Guid("2aded84d-55ba-7fe6-5912-6eb2e4108adb") },
                    { new Guid("78363a1d-bab5-44c7-3b11-1cce097dc5b5"), "Vocabulary", new Guid("87b72a5b-9923-e0a8-542c-95024f4d41a0") },
                    { new Guid("7858feb1-4d59-0324-7735-642cbfa925d8"), "Geography", new Guid("12e911ad-fe68-6d34-81aa-2a3e423479fe") },
                    { new Guid("7d54ed12-b21b-93d0-8326-9a125f828ed5"), "Reading Comprehension", new Guid("1f274bfd-5c4d-b4fe-522d-2fc070566c42") },
                    { new Guid("839252bf-9f0c-ba1f-abee-2f8a70ca6861"), "Logical Reasoning", new Guid("b1979e05-7e07-b2f8-d3f8-821dbb1ce6dd") },
                    { new Guid("839d98a7-deef-5095-a0f9-25e41cc8ac1f"), "Vocabulary", new Guid("1cf1914a-70a9-0732-71a9-2658384fa516") },
                    { new Guid("842f6749-85f2-0e2b-ce48-a0b6e5ac1909"), "Geometry", new Guid("327fe777-c46f-0930-3f8f-744612e374b0") },
                    { new Guid("84f9036a-6937-6926-5cd9-fa173c8f78cf"), "Percentage", new Guid("ef6d15a8-abfc-4a2b-ac8d-5033319bc397") },
                    { new Guid("89162032-759c-8685-6da6-533cef1121c2"), "Static GK", new Guid("21adc355-8586-e349-28e8-3a4492e3c8a6") },
                    { new Guid("8d7b8cc9-3012-3b52-d363-7a56040366ba"), "Series", new Guid("4a5581f2-39c2-c9c7-50ad-954a957f1b07") },
                    { new Guid("8ebca2dc-dd8e-a08a-b354-50152075dbbf"), "Profit and Loss", new Guid("a0fe7782-735b-c64b-5ee0-ef642bd4b412") },
                    { new Guid("92d9544d-5356-a90f-b122-277fa9df7502"), "Coding-Decoding", new Guid("6e95c6b1-5db5-855a-cc9b-5be0a1bcf838") },
                    { new Guid("93706aea-8c53-0363-c36f-8d9ae42c3f16"), "Geography", new Guid("21adc355-8586-e349-28e8-3a4492e3c8a6") },
                    { new Guid("940f66d7-879d-c1f3-6160-58bddcd006d8"), "Static GK", new Guid("95c19115-0ff0-2d83-7ea8-8fa8ec49d3bd") },
                    { new Guid("94626a7c-8a7c-7052-e24e-6168a14b619a"), "Error Detection", new Guid("90347abb-8fcd-7d22-748c-00c8bcf30b49") },
                    { new Guid("9754bfa5-5e7a-b81c-8161-3fc2da4e288f"), "Percentage", new Guid("a98cd19f-ab5f-c3a3-bfaa-b5bf97ad8026") },
                    { new Guid("9b817e8d-9aea-54ec-61c7-445a7e15669e"), "Error Detection", new Guid("52732bab-625e-b767-0dea-6f39a2f403cb") },
                    { new Guid("a013f134-8288-247f-8e55-cd8e6137b559"), "Percentage", new Guid("6c42f073-e167-13ea-1e64-4667f30548d2") },
                    { new Guid("a508f4cf-0d9c-7629-2aea-da74deb105aa"), "Series", new Guid("6e95c6b1-5db5-855a-cc9b-5be0a1bcf838") },
                    { new Guid("a8c35efe-1c10-552d-3549-27fd2de16f2d"), "Geography", new Guid("3d7d71ba-9120-b239-d696-715b35ee5535") },
                    { new Guid("a9192b7f-cfeb-7435-d324-0da87d599e10"), "Coding-Decoding", new Guid("2b9c687b-8c73-8bec-723f-33d1b2ed4c49") },
                    { new Guid("a97fed4c-a034-c80c-fae0-a9939f343df3"), "Reading Comprehension", new Guid("593ab663-779a-cd4b-e7c3-9b0e29a0aea9") },
                    { new Guid("ace98aff-d3d3-2d59-d406-090d6720333d"), "Geometry", new Guid("02db2b9a-c84b-5ce1-0f3c-68969f7e554c") },
                    { new Guid("ad98d2ef-ad94-2e92-5543-c4eb6bd96646"), "Logical Reasoning", new Guid("e0368c2d-0969-a6d1-09bd-8e27f2e79993") },
                    { new Guid("af538c04-c2fc-7ec6-5f6a-4f33b09f8334"), "History", new Guid("21adc355-8586-e349-28e8-3a4492e3c8a6") },
                    { new Guid("b9185fac-59a7-7a42-fb9c-e744afae7bcc"), "Profit and Loss", new Guid("ef6d15a8-abfc-4a2b-ac8d-5033319bc397") },
                    { new Guid("b9ee7ef5-1fa2-4b96-1e7b-b40906edc356"), "Series", new Guid("b1979e05-7e07-b2f8-d3f8-821dbb1ce6dd") },
                    { new Guid("bf1955d0-11b7-df71-9ed2-e2cfed10d136"), "Coding-Decoding", new Guid("4a5581f2-39c2-c9c7-50ad-954a957f1b07") },
                    { new Guid("c1739f76-fe2f-9a76-e216-97b9684a8096"), "Reading Comprehension", new Guid("90347abb-8fcd-7d22-748c-00c8bcf30b49") },
                    { new Guid("c3f6082b-a2bf-85d7-5d1e-868dcc2da9c1"), "Reading Comprehension", new Guid("898eb630-9c17-9ee7-c375-739dc4c14ead") },
                    { new Guid("c515467a-9cd4-244b-ef48-e92f9c4d1f2a"), "Profit and Loss", new Guid("327fe777-c46f-0930-3f8f-744612e374b0") },
                    { new Guid("ca48c4e3-fb3d-7413-ad65-a20dd325a4f4"), "Series", new Guid("e0368c2d-0969-a6d1-09bd-8e27f2e79993") },
                    { new Guid("cb3a3ad6-82e1-e43e-6bdd-6cb1c5214437"), "Series", new Guid("2aded84d-55ba-7fe6-5912-6eb2e4108adb") },
                    { new Guid("ce48134c-a931-4849-7d0e-29e74f20eb32"), "Vocabulary", new Guid("898eb630-9c17-9ee7-c375-739dc4c14ead") },
                    { new Guid("dbde005b-64cb-d165-6712-d79c60505484"), "Logical Reasoning", new Guid("3ae9430f-30bb-52a1-643c-f18ee6522b95") },
                    { new Guid("de1722c8-8d74-8ebc-37bc-fcf407c468b4"), "Static GK", new Guid("3d7d71ba-9120-b239-d696-715b35ee5535") },
                    { new Guid("df0d7024-e61d-ff8d-5d42-f1f9afa07246"), "Percentage", new Guid("02db2b9a-c84b-5ce1-0f3c-68969f7e554c") },
                    { new Guid("e2d7b0fa-03f3-b73f-b386-e105b84d64c2"), "Profit and Loss", new Guid("02db2b9a-c84b-5ce1-0f3c-68969f7e554c") },
                    { new Guid("e2f82265-1818-2113-593a-80a45fe4c8f9"), "Percentage", new Guid("af68f49f-904d-04cc-ab49-e20ae7b87985") },
                    { new Guid("e77f73c8-94ef-bb10-0a63-efc740d2cac5"), "Series", new Guid("75bb5701-e8eb-3fba-15ea-f4d403ccde0e") },
                    { new Guid("e8be4456-b4bd-9d6f-fad1-266957f6418d"), "Coding-Decoding", new Guid("3ae9430f-30bb-52a1-643c-f18ee6522b95") },
                    { new Guid("e97a927c-4d5a-64a6-dbeb-6ed70658b969"), "Geometry", new Guid("6c42f073-e167-13ea-1e64-4667f30548d2") },
                    { new Guid("eb4a7173-cf71-5602-fad5-b7622551d3a0"), "Geometry", new Guid("a98cd19f-ab5f-c3a3-bfaa-b5bf97ad8026") },
                    { new Guid("eff265cf-0259-ff43-e16f-aeb8b62a5c85"), "Percentage", new Guid("c48473f0-e6ac-dd2b-2aca-5c9ed9ca886b") },
                    { new Guid("f1e66e7e-a06d-67b9-8d94-d01d673eb98e"), "Profit and Loss", new Guid("a98cd19f-ab5f-c3a3-bfaa-b5bf97ad8026") },
                    { new Guid("f32e366b-7222-c6d4-03cc-993f440f683b"), "Vocabulary", new Guid("1f274bfd-5c4d-b4fe-522d-2fc070566c42") },
                    { new Guid("f76da6cd-e43f-d734-9aa7-ab93db200cd5"), "Vocabulary", new Guid("90347abb-8fcd-7d22-748c-00c8bcf30b49") },
                    { new Guid("f7717f83-aa0a-4e8c-eb0a-8d601ed47c1a"), "Logical Reasoning", new Guid("6e95c6b1-5db5-855a-cc9b-5be0a1bcf838") },
                    { new Guid("fa2e958e-86a0-2eb7-9943-b3eeaacf03a6"), "Profit and Loss", new Guid("af68f49f-904d-04cc-ab49-e20ae7b87985") },
                    { new Guid("fcc8ae34-0921-9c15-99c6-193b772710a2"), "Percentage", new Guid("a0fe7782-735b-c64b-5ee0-ef642bd4b412") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_SpaceId",
                table: "Documents",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_SpaceId",
                table: "Conversations",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_Name",
                table: "Exams",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamStages_ExamId",
                table: "ExamStages",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_SpaceId",
                table: "Goals",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_UserId_SpaceId",
                table: "Goals",
                columns: new[] { "UserId", "SpaceId" });

            migrationBuilder.CreateIndex(
                name: "IX_Memories_SpaceId",
                table: "Memories",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Memories_UserId_SpaceId",
                table: "Memories",
                columns: new[] { "UserId", "SpaceId" });

            migrationBuilder.CreateIndex(
                name: "IX_Spaces_UserId_IsArchived",
                table: "Spaces",
                columns: new[] { "UserId", "IsArchived" });

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanItems_StudyPlanId_Date",
                table: "StudyPlanItems",
                columns: new[] { "StudyPlanId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanItems_TopicId",
                table: "StudyPlanItems",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlans_SpaceId",
                table: "StudyPlans",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlans_UserId_SpaceId",
                table: "StudyPlans",
                columns: new[] { "UserId", "SpaceId" });

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_ExamStageId",
                table: "Subjects",
                column: "ExamStageId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_SubjectId",
                table: "Topics",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExamProfiles_ExamStageId",
                table: "UserExamProfiles",
                column: "ExamStageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExamProfiles_SpaceId",
                table: "UserExamProfiles",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExamProfiles_UserId",
                table: "UserExamProfiles",
                column: "UserId",
                unique: true,
                filter: "\"SpaceId\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserExamProfiles_UserId_SpaceId",
                table: "UserExamProfiles",
                columns: new[] { "UserId", "SpaceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTopicProgress_TopicId",
                table: "UserTopicProgress",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTopicProgress_UserExamProfileId_TopicId",
                table: "UserTopicProgress",
                columns: new[] { "UserExamProfileId", "TopicId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Spaces_SpaceId",
                table: "Conversations",
                column: "SpaceId",
                principalTable: "Spaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Spaces_SpaceId",
                table: "Documents",
                column: "SpaceId",
                principalTable: "Spaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Spaces_SpaceId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Spaces_SpaceId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropTable(
                name: "Memories");

            migrationBuilder.DropTable(
                name: "StudyPlanItems");

            migrationBuilder.DropTable(
                name: "UserTopicProgress");

            migrationBuilder.DropTable(
                name: "StudyPlans");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "UserExamProfiles");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Spaces");

            migrationBuilder.DropTable(
                name: "ExamStages");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Documents_SpaceId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_SpaceId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "SpaceId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "SpaceId",
                table: "Conversations");
        }
    }
}
