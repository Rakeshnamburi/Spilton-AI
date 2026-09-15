using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Spilton.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMockPracticeEngine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MockTests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    SpaceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExamStageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: false),
                    DurationSeconds = table.Column<int>(type: "integer", nullable: false),
                    FloorAtZero = table.Column<bool>(type: "boolean", nullable: false),
                    NavigationRule = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MockTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MockTests_ExamStages_ExamStageId",
                        column: x => x.ExamStageId,
                        principalTable: "ExamStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MockTests_Spaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "Spaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MockTests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    SpaceId = table.Column<Guid>(type: "uuid", nullable: true),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    Explanation = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    Difficulty = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    SourceType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    SourceResourceId = table.Column<Guid>(type: "uuid", nullable: true),
                    SourceMetadataJson = table.Column<string>(type: "jsonb", nullable: false),
                    CorrectOptionIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.CheckConstraint("CK_Question_Answer", "\"CorrectOptionIndex\" BETWEEN 0 AND 3");
                    table.ForeignKey(
                        name: "FK_Questions_GovernmentResources_SourceResourceId",
                        column: x => x.SourceResourceId,
                        principalTable: "GovernmentResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Questions_Spaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "Spaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MockAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpaceId = table.Column<Guid>(type: "uuid", nullable: true),
                    MockTestId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartKey = table.Column<Guid>(type: "uuid", nullable: false),
                    UserExamProfileId = table.Column<Guid>(type: "uuid", nullable: true),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    CompletionReason = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    Correct = table.Column<int>(type: "integer", nullable: false),
                    Incorrect = table.Column<int>(type: "integer", nullable: false),
                    Unattempted = table.Column<int>(type: "integer", nullable: false),
                    TimeTakenSeconds = table.Column<int>(type: "integer", nullable: false),
                    RawScore = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    NegativeMarks = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    FinalScore = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    AccuracyPercent = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MockAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MockAttempts_MockTests_MockTestId",
                        column: x => x.MockTestId,
                        principalTable: "MockTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MockAttempts_Spaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "Spaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MockAttempts_UserExamProfiles_UserExamProfileId",
                        column: x => x.UserExamProfileId,
                        principalTable: "UserExamProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MockAttempts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MockSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MockTestId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    MarksCorrect = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    NegativeMarks = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MockSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MockSections_MockTests_MockTestId",
                        column: x => x.MockTestId,
                        principalTable: "MockTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MockSections_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionOptions", x => x.Id);
                    table.CheckConstraint("CK_Option_Index", "\"Index\" BETWEEN 0 AND 3");
                    table.ForeignKey(
                        name: "FK_QuestionOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MockTestQuestions",
                columns: table => new
                {
                    MockTestId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    MockSectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MockTestQuestions", x => new { x.MockTestId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_MockTestQuestions_MockSections_MockSectionId",
                        column: x => x.MockSectionId,
                        principalTable: "MockSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MockTestQuestions_MockTests_MockTestId",
                        column: x => x.MockTestId,
                        principalTable: "MockTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MockTestQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttemptAnswers",
                columns: table => new
                {
                    MockAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    OptionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Visited = table.Column<bool>(type: "boolean", nullable: false),
                    MarkedForReview = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    MarksCorrect = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    NegativeMarks = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: true),
                    Score = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttemptAnswers", x => new { x.MockAttemptId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_AttemptAnswers_MockAttempts_MockAttemptId",
                        column: x => x.MockAttemptId,
                        principalTable: "MockAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttemptAnswers_QuestionOptions_OptionId",
                        column: x => x.OptionId,
                        principalTable: "QuestionOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttemptAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "MockTests",
                columns: new[] { "Id", "CreatedAt", "Description", "DurationSeconds", "ExamStageId", "FloorAtZero", "NavigationRule", "SpaceId", "Title", "UserId" },
                values: new object[] { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new DateTimeOffset(new DateTime(2026, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Manually authored development practice, not an official paper or a complete current exam pattern. Free section navigation. Stored sample scoring: +2 correct, −0.5 incorrect, 0 unattempted.", 600, new Guid("4b4b85c5-7efc-17c3-ea31-575f3fde09c0"), false, "FREE", null, "SSC CGL starter · 16 original practice questions", null });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CorrectOptionIndex", "Difficulty", "Explanation", "SourceMetadataJson", "SourceResourceId", "SourceType", "SpaceId", "Text", "TopicId", "UserId" },
                values: new object[,]
                {
                    { new Guid("1675f9a1-dfd2-386a-fbc0-ead39b3861d7"), 2, "EASY", "15/100 × 200 = 30.", "{\"label\":\"Original development question; not PYQ\"}", null, "MANUAL_DEVELOPMENT", null, "What is 15% of 200?", new Guid("9754bfa5-5e7a-b81c-8161-3fc2da4e288f"), null },
                    { new Guid("4d362297-ec87-d6bb-6066-88252b367938"), 0, "EASY", "12.5% equals one eighth. 80 ÷ 8 = 10.", "{\"label\":\"Original development question; not PYQ\"}", null, "MANUAL_DEVELOPMENT", null, "What is 12.5% of 80?", new Guid("9754bfa5-5e7a-b81c-8161-3fc2da4e288f"), null },
                    { new Guid("52cc46f1-196b-44c9-f557-3bd2d05f28a7"), 0, "EASY", "20/100 × 250 = 50.", "{\"label\":\"Original development question; not PYQ\"}", null, "MANUAL_DEVELOPMENT", null, "What is 20% of 250?", new Guid("9754bfa5-5e7a-b81c-8161-3fc2da4e288f"), null },
                    { new Guid("5f3f1aa6-fa50-ec61-457a-ca72cfbc4f75"), 2, "EASY", "A water molecule contains two hydrogen atoms and one oxygen atom: H2O.", "{\"label\":\"Original development question; not PYQ\"}", null, "MANUAL_DEVELOPMENT", null, "What is the chemical formula of water?", new Guid("2ee79a5c-1817-7974-b10f-9556ddf79938"), null },
                    { new Guid("63f0c58a-9d3a-49fc-9a30-16896c4e75f5"), 1, "EASY", "Add 3 to each term; 9 + 3 = 12.", "{\"label\":\"Original development question; not PYQ\"}", null, "MANUAL_DEVELOPMENT", null, "What comes next: 3, 6, 9, ...?", new Guid("8d7b8cc9-3012-3b52-d363-7a56040366ba"), null },
                    { new Guid("65f10c42-fbb7-6302-8108-71ee5131f8b2"), 0, "EASY", "D→E, O→P, G→H, so DOG becomes EPH.", "{\"label\":\"Original development question; not PYQ\"}", null, "MANUAL_DEVELOPMENT", null, "Each letter is replaced by the next alphabet letter. How is DOG encoded?", new Guid("bf1955d0-11b7-df71-9ed2-e2cfed10d136"), null },
                    { new Guid("6dd2aae4-be22-804d-a7e5-61dbbd77d45d"), 1, "EASY", "Profit is ₹20. Profit percentage is 20/100 × 100 = 20%.", "{\"label\":\"Original development question; not PYQ\"}", null, "MANUAL_DEVELOPMENT", null, "An item costs ₹100 and sells for ₹120. What is the profit percentage on cost?", new Guid("f1e66e7e-a06d-67b9-8d94-d01d673eb98e"), null },
                    { new Guid("733edbd6-49be-89e7-2482-fce05d48a1fe"), 1, "EASY", "New Delhi is the capital of India.", "{\"label\":\"Original development question; not PYQ\"}", null, "MANUAL_DEVELOPMENT", null, "What is the capital of India?", new Guid("2ee79a5c-1817-7974-b10f-9556ddf79938"), null },
                    { new Guid("895d0362-18aa-9c82-dbdc-e3a9eedd9554"), 2, "EASY", "C=3, A=1, T=20; the sum is 24.", "{\"label\":\"Original development question; not PYQ\"}", null, "MANUAL_DEVELOPMENT", null, "If A=1, B=2, ..., Z=26, what is the sum of the letter values in CAT?", new Guid("bf1955d0-11b7-df71-9ed2-e2cfed10d136"), null },
                    { new Guid("9db94346-c377-8cbb-3353-e5eb29d3ad32"), 2, "EASY", "Each term is twice the preceding term. 16 × 2 = 32.", "{\"label\":\"Original development question; not PYQ\"}", null, "MANUAL_DEVELOPMENT", null, "What comes next: 2, 4, 8, 16, ...?", new Guid("8d7b8cc9-3012-3b52-d363-7a56040366ba"), null },
                    { new Guid("b691d063-bd12-93f0-e80a-41416e44e9b6"), 2, "EASY", "Rapid means fast or quick.", "{\"label\":\"Original development question; not PYQ\"}", null, "MANUAL_DEVELOPMENT", null, "Which word is closest in meaning to 'rapid'?", new Guid("ce48134c-a931-4849-7d0e-29e74f20eb32"), null },
                    { new Guid("b9d84f58-09fb-c3de-df43-659895c28397"), 1, "EASY", "25% is one quarter. 480 ÷ 4 = 120.", "{\"label\":\"Original development question; not PYQ\"}", null, "MANUAL_DEVELOPMENT", null, "What is 25% of 480?", new Guid("9754bfa5-5e7a-b81c-8161-3fc2da4e288f"), null },
                    { new Guid("e04e6f5a-b165-d99b-049e-06ecddd25e37"), 3, "EASY", "40/100 × 150 = 60.", "{\"label\":\"Original development question; not PYQ\"}", null, "MANUAL_DEVELOPMENT", null, "What is 40% of 150?", new Guid("9754bfa5-5e7a-b81c-8161-3fc2da4e288f"), null },
                    { new Guid("e08abfa9-2f46-7627-1205-580dad24660f"), 1, "EASY", "A singular third-person subject uses 'goes' in the simple present.", "{\"label\":\"Original development question; not PYQ\"}", null, "MANUAL_DEVELOPMENT", null, "Choose the correct word: She ___ to school every day.", new Guid("5a91af83-a3c5-f4c8-83d8-dd35fa977f77"), null },
                    { new Guid("ec8e6690-00ff-4fc0-6e53-9f398bbea8ec"), 1, "EASY", "Ten percent is one tenth: 750 ÷ 10 = 75.", "{\"label\":\"Original development question; not PYQ\"}", null, "MANUAL_DEVELOPMENT", null, "What is 10% of 750?", new Guid("9754bfa5-5e7a-b81c-8161-3fc2da4e288f"), null },
                    { new Guid("f89ab96f-4711-b43c-a5d3-422ed0bd8c5e"), 2, "EASY", "Loss is ₹20. Loss percentage is 20/200 × 100 = 10%.", "{\"label\":\"Original development question; not PYQ\"}", null, "MANUAL_DEVELOPMENT", null, "An item costs ₹200 and sells for ₹180. What is the loss percentage on cost?", new Guid("f1e66e7e-a06d-67b9-8d94-d01d673eb98e"), null }
                });

            migrationBuilder.InsertData(
                table: "MockSections",
                columns: new[] { "Id", "MarksCorrect", "MockTestId", "Name", "NegativeMarks", "Position", "SubjectId" },
                values: new object[,]
                {
                    { new Guid("7ed63f78-4765-ca44-a213-fe391be677db"), 2m, new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), "English", 0.5m, 2, new Guid("898eb630-9c17-9ee7-c375-739dc4c14ead") },
                    { new Guid("9ab11f0a-c072-8b8e-9b54-f9fd0d4611f3"), 2m, new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), "Quantitative Aptitude", 0.5m, 0, new Guid("a98cd19f-ab5f-c3a3-bfaa-b5bf97ad8026") },
                    { new Guid("ad031f95-1bdd-5c2f-a6b3-b94d77b6a961"), 2m, new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), "Reasoning", 0.5m, 1, new Guid("4a5581f2-39c2-c9c7-50ad-954a957f1b07") },
                    { new Guid("d186532d-d4b1-f3c7-3553-7836ab874829"), 2m, new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), "General Awareness", 0.5m, 3, new Guid("899ad66e-cb98-e49a-6994-05c201d848fa") }
                });

            migrationBuilder.InsertData(
                table: "QuestionOptions",
                columns: new[] { "Id", "Index", "QuestionId", "Text" },
                values: new object[,]
                {
                    { new Guid("012ed3cd-b4ee-a640-30e3-8594516818f2"), 2, new Guid("e04e6f5a-b165-d99b-049e-06ecddd25e37"), "45" },
                    { new Guid("0141886b-8d62-04cf-dda5-74bcf1da9eae"), 2, new Guid("1675f9a1-dfd2-386a-fbc0-ead39b3861d7"), "30" },
                    { new Guid("03f7d23b-4d0e-3d9b-074a-e15ab8e452e4"), 0, new Guid("f89ab96f-4711-b43c-a5d3-422ed0bd8c5e"), "5%" },
                    { new Guid("05fc5938-8f3f-cbbf-e489-cb157f3ba6df"), 1, new Guid("ec8e6690-00ff-4fc0-6e53-9f398bbea8ec"), "75" },
                    { new Guid("08f604ef-00c4-6309-aaa2-834462fb5e88"), 0, new Guid("63f0c58a-9d3a-49fc-9a30-16896c4e75f5"), "10" },
                    { new Guid("1b61f9e4-49e8-5c6f-e2ca-1b706dc8d6a0"), 3, new Guid("4d362297-ec87-d6bb-6066-88252b367938"), "16" },
                    { new Guid("20b11562-3b65-4dd9-365e-45fb1bb86b20"), 0, new Guid("733edbd6-49be-89e7-2482-fce05d48a1fe"), "Mumbai" },
                    { new Guid("23031570-d020-6efa-ee20-4de843d97da5"), 3, new Guid("f89ab96f-4711-b43c-a5d3-422ed0bd8c5e"), "90%" },
                    { new Guid("237a6be3-b13c-d369-cd1e-dae2eef94f56"), 3, new Guid("9db94346-c377-8cbb-3353-e5eb29d3ad32"), "64" },
                    { new Guid("258d1f0c-d6b7-1d63-70d6-7c423e380ed0"), 3, new Guid("52cc46f1-196b-44c9-f557-3bd2d05f28a7"), "100" },
                    { new Guid("302b465e-fec5-57c9-dcd9-1ca0f64498da"), 1, new Guid("e04e6f5a-b165-d99b-049e-06ecddd25e37"), "30" },
                    { new Guid("3a9e703d-0ed1-3633-83a8-764886579a0e"), 2, new Guid("6dd2aae4-be22-804d-a7e5-61dbbd77d45d"), "25%" },
                    { new Guid("3c50b265-7411-15af-fecd-196db5241809"), 3, new Guid("e08abfa9-2f46-7627-1205-580dad24660f"), "gone" },
                    { new Guid("3f968b58-0ea2-7762-d207-83285b54a6f0"), 3, new Guid("895d0362-18aa-9c82-dbdc-e3a9eedd9554"), "26" },
                    { new Guid("45986894-2785-e9f9-c170-cb7223fb9611"), 2, new Guid("b691d063-bd12-93f0-e80a-41416e44e9b6"), "fast" },
                    { new Guid("4993e6ea-a9c8-e7ae-5455-b6b42b8fcc1a"), 1, new Guid("f89ab96f-4711-b43c-a5d3-422ed0bd8c5e"), "20%" },
                    { new Guid("4bdcb2a6-5aa9-45a1-d709-1df1544929f9"), 2, new Guid("b9d84f58-09fb-c3de-df43-659895c28397"), "140" },
                    { new Guid("4be52c33-b4b3-2201-c4b2-fc9d1ba20789"), 0, new Guid("ec8e6690-00ff-4fc0-6e53-9f398bbea8ec"), "7.5" },
                    { new Guid("56cb8290-863b-a040-d741-de3ae7194af0"), 0, new Guid("65f10c42-fbb7-6302-8108-71ee5131f8b2"), "EPH" },
                    { new Guid("59576443-3747-d443-160f-9a01d05a563e"), 1, new Guid("52cc46f1-196b-44c9-f557-3bd2d05f28a7"), "25" },
                    { new Guid("5991b9fe-08ac-1a64-d861-c20851bc6aa7"), 0, new Guid("895d0362-18aa-9c82-dbdc-e3a9eedd9554"), "21" },
                    { new Guid("659e5823-89e6-17d9-271d-1296a8f794df"), 2, new Guid("ec8e6690-00ff-4fc0-6e53-9f398bbea8ec"), "150" },
                    { new Guid("65e3cf43-f547-cc65-7a01-a3ef35065b91"), 0, new Guid("1675f9a1-dfd2-386a-fbc0-ead39b3861d7"), "15" },
                    { new Guid("66e16ba7-f25d-eef8-8a7e-9022cd8b65b6"), 3, new Guid("6dd2aae4-be22-804d-a7e5-61dbbd77d45d"), "120%" },
                    { new Guid("6bb62754-bcfc-8cf6-43e9-bf30299666ec"), 1, new Guid("63f0c58a-9d3a-49fc-9a30-16896c4e75f5"), "12" },
                    { new Guid("6d129dac-383c-3e75-4fee-86e18197c6c4"), 0, new Guid("e04e6f5a-b165-d99b-049e-06ecddd25e37"), "15" },
                    { new Guid("6e330f71-b8da-fe93-8e98-bc6bc01e4244"), 0, new Guid("4d362297-ec87-d6bb-6066-88252b367938"), "10" },
                    { new Guid("7174c23f-d5f9-56bd-8160-ae31b729a95c"), 1, new Guid("9db94346-c377-8cbb-3353-e5eb29d3ad32"), "24" },
                    { new Guid("717773aa-f124-7014-a98c-ef0624355ada"), 2, new Guid("e08abfa9-2f46-7627-1205-580dad24660f"), "going" },
                    { new Guid("786ab5da-b4db-072f-bb5f-b898518de7b9"), 0, new Guid("5f3f1aa6-fa50-ec61-457a-ca72cfbc4f75"), "CO2" },
                    { new Guid("7cc3f4ad-5d03-47bc-3e2e-83fd52a2b754"), 2, new Guid("5f3f1aa6-fa50-ec61-457a-ca72cfbc4f75"), "H2O" },
                    { new Guid("83286d73-977d-637d-9b40-d54eaa1433ee"), 3, new Guid("b9d84f58-09fb-c3de-df43-659895c28397"), "160" },
                    { new Guid("85b3c1e9-927f-b2f1-28ca-bc8c9e400377"), 2, new Guid("895d0362-18aa-9c82-dbdc-e3a9eedd9554"), "24" },
                    { new Guid("89995e5c-7822-368e-0d4d-5710f53a5293"), 3, new Guid("65f10c42-fbb7-6302-8108-71ee5131f8b2"), "FPH" },
                    { new Guid("92f007a1-6d47-56de-a2cf-4dfa0f433c9a"), 3, new Guid("b691d063-bd12-93f0-e80a-41416e44e9b6"), "weak" },
                    { new Guid("971bb6e5-48d6-cc2a-bd41-472cd88bde70"), 2, new Guid("4d362297-ec87-d6bb-6066-88252b367938"), "8" },
                    { new Guid("9cf2c49f-abb7-7a99-8521-948b598f8936"), 2, new Guid("65f10c42-fbb7-6302-8108-71ee5131f8b2"), "ENH" },
                    { new Guid("9eae27e7-18ad-997c-eb84-c986b94d9399"), 3, new Guid("1675f9a1-dfd2-386a-fbc0-ead39b3861d7"), "40" },
                    { new Guid("9fab4030-4404-e61e-22dc-4c28d86e5a93"), 1, new Guid("e08abfa9-2f46-7627-1205-580dad24660f"), "goes" },
                    { new Guid("a47e4e95-f3ae-c6ae-6a84-8e091444a6df"), 0, new Guid("e08abfa9-2f46-7627-1205-580dad24660f"), "go" },
                    { new Guid("a664f005-d2c1-187b-5b8e-ce2d4ae5b442"), 1, new Guid("733edbd6-49be-89e7-2482-fce05d48a1fe"), "New Delhi" },
                    { new Guid("b034fb86-1828-4698-19f1-44d48d257423"), 3, new Guid("5f3f1aa6-fa50-ec61-457a-ca72cfbc4f75"), "NaCl" },
                    { new Guid("b134b7d3-eb1a-9b29-d825-321bd4bcea15"), 0, new Guid("b691d063-bd12-93f0-e80a-41416e44e9b6"), "slow" },
                    { new Guid("ba093a8a-b0a2-7e2f-e32d-7f3d93e58558"), 3, new Guid("e04e6f5a-b165-d99b-049e-06ecddd25e37"), "60" },
                    { new Guid("ba67836f-37fb-b2d7-53cb-bf836a1ad8ce"), 3, new Guid("63f0c58a-9d3a-49fc-9a30-16896c4e75f5"), "18" },
                    { new Guid("baca4e12-854f-d5d2-6a96-eb76cbc875f7"), 0, new Guid("6dd2aae4-be22-804d-a7e5-61dbbd77d45d"), "10%" },
                    { new Guid("bc95cee2-f271-4b29-2015-6245c1ec57a8"), 3, new Guid("733edbd6-49be-89e7-2482-fce05d48a1fe"), "Kolkata" },
                    { new Guid("bf34f7d1-697b-dc47-b596-0857b07278c6"), 0, new Guid("9db94346-c377-8cbb-3353-e5eb29d3ad32"), "20" },
                    { new Guid("c35d5ce4-dd58-22df-f2dd-dd74f5a79973"), 2, new Guid("f89ab96f-4711-b43c-a5d3-422ed0bd8c5e"), "10%" },
                    { new Guid("c63dae98-32dd-3c4b-674a-8e1cb39f94ce"), 1, new Guid("4d362297-ec87-d6bb-6066-88252b367938"), "12" },
                    { new Guid("c979b679-2a8a-3154-45f5-8fba567c50e1"), 0, new Guid("52cc46f1-196b-44c9-f557-3bd2d05f28a7"), "50" },
                    { new Guid("c9b5b899-52c4-2c33-2cb1-6811307139dc"), 3, new Guid("ec8e6690-00ff-4fc0-6e53-9f398bbea8ec"), "750" },
                    { new Guid("cbb2fe47-11db-6bd2-2442-5e864981d9cd"), 1, new Guid("895d0362-18aa-9c82-dbdc-e3a9eedd9554"), "23" },
                    { new Guid("cbeaee6c-da43-8f58-ac82-a403cdf35944"), 2, new Guid("733edbd6-49be-89e7-2482-fce05d48a1fe"), "Chennai" },
                    { new Guid("ccd2bf2f-e1c4-92b1-d2b9-8b26ef000fce"), 1, new Guid("b691d063-bd12-93f0-e80a-41416e44e9b6"), "late" },
                    { new Guid("d52a133e-3d5d-50a1-9884-0bf7e3f90338"), 1, new Guid("1675f9a1-dfd2-386a-fbc0-ead39b3861d7"), "20" },
                    { new Guid("d8951402-408e-c587-52a9-28239e65c4e5"), 1, new Guid("6dd2aae4-be22-804d-a7e5-61dbbd77d45d"), "20%" },
                    { new Guid("e32ddc55-9f33-e55a-2c2d-c3ed01f9928e"), 2, new Guid("63f0c58a-9d3a-49fc-9a30-16896c4e75f5"), "15" },
                    { new Guid("e9dec28c-c266-bdaa-d2e5-2e76070d7865"), 1, new Guid("65f10c42-fbb7-6302-8108-71ee5131f8b2"), "EPG" },
                    { new Guid("eb034a9d-cc0c-72b3-58d0-5a655945c91f"), 1, new Guid("b9d84f58-09fb-c3de-df43-659895c28397"), "120" },
                    { new Guid("f2c6c7f5-5c9f-a5ea-fd00-3e2117ff1cbc"), 2, new Guid("9db94346-c377-8cbb-3353-e5eb29d3ad32"), "32" },
                    { new Guid("f8119e68-da0e-86b4-5449-0dff31a9f558"), 2, new Guid("52cc46f1-196b-44c9-f557-3bd2d05f28a7"), "75" },
                    { new Guid("fa5d1bb7-5645-ef8f-95d1-eb979078ae10"), 1, new Guid("5f3f1aa6-fa50-ec61-457a-ca72cfbc4f75"), "O2" },
                    { new Guid("faecd258-6d9e-34d9-2bea-b91d1990e2dd"), 0, new Guid("b9d84f58-09fb-c3de-df43-659895c28397"), "100" }
                });

            migrationBuilder.InsertData(
                table: "MockTestQuestions",
                columns: new[] { "MockTestId", "QuestionId", "MockSectionId", "Position" },
                values: new object[,]
                {
                    { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new Guid("1675f9a1-dfd2-386a-fbc0-ead39b3861d7"), new Guid("9ab11f0a-c072-8b8e-9b54-f9fd0d4611f3"), 2 },
                    { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new Guid("4d362297-ec87-d6bb-6066-88252b367938"), new Guid("9ab11f0a-c072-8b8e-9b54-f9fd0d4611f3"), 5 },
                    { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new Guid("52cc46f1-196b-44c9-f557-3bd2d05f28a7"), new Guid("9ab11f0a-c072-8b8e-9b54-f9fd0d4611f3"), 1 },
                    { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new Guid("5f3f1aa6-fa50-ec61-457a-ca72cfbc4f75"), new Guid("d186532d-d4b1-f3c7-3553-7836ab874829"), 14 },
                    { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new Guid("63f0c58a-9d3a-49fc-9a30-16896c4e75f5"), new Guid("ad031f95-1bdd-5c2f-a6b3-b94d77b6a961"), 9 },
                    { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new Guid("65f10c42-fbb7-6302-8108-71ee5131f8b2"), new Guid("ad031f95-1bdd-5c2f-a6b3-b94d77b6a961"), 11 },
                    { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new Guid("6dd2aae4-be22-804d-a7e5-61dbbd77d45d"), new Guid("9ab11f0a-c072-8b8e-9b54-f9fd0d4611f3"), 6 },
                    { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new Guid("733edbd6-49be-89e7-2482-fce05d48a1fe"), new Guid("d186532d-d4b1-f3c7-3553-7836ab874829"), 15 },
                    { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new Guid("895d0362-18aa-9c82-dbdc-e3a9eedd9554"), new Guid("ad031f95-1bdd-5c2f-a6b3-b94d77b6a961"), 10 },
                    { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new Guid("9db94346-c377-8cbb-3353-e5eb29d3ad32"), new Guid("ad031f95-1bdd-5c2f-a6b3-b94d77b6a961"), 8 },
                    { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new Guid("b691d063-bd12-93f0-e80a-41416e44e9b6"), new Guid("7ed63f78-4765-ca44-a213-fe391be677db"), 13 },
                    { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new Guid("b9d84f58-09fb-c3de-df43-659895c28397"), new Guid("9ab11f0a-c072-8b8e-9b54-f9fd0d4611f3"), 0 },
                    { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new Guid("e04e6f5a-b165-d99b-049e-06ecddd25e37"), new Guid("9ab11f0a-c072-8b8e-9b54-f9fd0d4611f3"), 4 },
                    { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new Guid("e08abfa9-2f46-7627-1205-580dad24660f"), new Guid("7ed63f78-4765-ca44-a213-fe391be677db"), 12 },
                    { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new Guid("ec8e6690-00ff-4fc0-6e53-9f398bbea8ec"), new Guid("9ab11f0a-c072-8b8e-9b54-f9fd0d4611f3"), 3 },
                    { new Guid("0dbd8310-35ed-8c25-855a-76c2d7aef9ee"), new Guid("f89ab96f-4711-b43c-a5d3-422ed0bd8c5e"), new Guid("9ab11f0a-c072-8b8e-9b54-f9fd0d4611f3"), 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttemptAnswers_OptionId",
                table: "AttemptAnswers",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttemptAnswers_QuestionId",
                table: "AttemptAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_MockAttempts_MockTestId",
                table: "MockAttempts",
                column: "MockTestId");

            migrationBuilder.CreateIndex(
                name: "IX_MockAttempts_SpaceId",
                table: "MockAttempts",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_MockAttempts_Status_ExpiresAt",
                table: "MockAttempts",
                columns: new[] { "Status", "ExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MockAttempts_UserExamProfileId",
                table: "MockAttempts",
                column: "UserExamProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MockAttempts_UserId_SpaceId_StartedAt",
                table: "MockAttempts",
                columns: new[] { "UserId", "SpaceId", "StartedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MockAttempts_UserId_StartKey",
                table: "MockAttempts",
                columns: new[] { "UserId", "StartKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MockSections_MockTestId_Position",
                table: "MockSections",
                columns: new[] { "MockTestId", "Position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MockSections_SubjectId",
                table: "MockSections",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MockTestQuestions_MockSectionId",
                table: "MockTestQuestions",
                column: "MockSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_MockTestQuestions_MockTestId_Position",
                table: "MockTestQuestions",
                columns: new[] { "MockTestId", "Position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MockTestQuestions_QuestionId",
                table: "MockTestQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_MockTests_ExamStageId",
                table: "MockTests",
                column: "ExamStageId");

            migrationBuilder.CreateIndex(
                name: "IX_MockTests_SpaceId",
                table: "MockTests",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_MockTests_UserId",
                table: "MockTests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_QuestionId_Index",
                table: "QuestionOptions",
                columns: new[] { "QuestionId", "Index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SourceResourceId",
                table: "Questions",
                column: "SourceResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SpaceId",
                table: "Questions",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TopicId",
                table: "Questions",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_UserId_SpaceId_TopicId",
                table: "Questions",
                columns: new[] { "UserId", "SpaceId", "TopicId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttemptAnswers");

            migrationBuilder.DropTable(
                name: "MockTestQuestions");

            migrationBuilder.DropTable(
                name: "MockAttempts");

            migrationBuilder.DropTable(
                name: "QuestionOptions");

            migrationBuilder.DropTable(
                name: "MockSections");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "MockTests");
        }
    }
}
