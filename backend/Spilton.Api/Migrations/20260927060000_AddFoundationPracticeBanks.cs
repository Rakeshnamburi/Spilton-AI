using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Spilton.Api.Data;

#nullable disable

namespace Spilton.Api.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260927060000_AddFoundationPracticeBanks")]
public sealed class AddFoundationPracticeBanks : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            CREATE TEMP TABLE "_SpiltonFoundationTemplates" (
                "Position" integer NOT NULL,
                "SubjectName" text NOT NULL,
                "TopicName" text NOT NULL,
                "QuestionText" text NOT NULL,
                "Explanation" text NOT NULL,
                "CorrectOptionIndex" integer NOT NULL,
                "Option0" text NOT NULL,
                "Option1" text NOT NULL,
                "Option2" text NOT NULL,
                "Option3" text NOT NULL
            ) ON COMMIT DROP;

            INSERT INTO "_SpiltonFoundationTemplates" VALUES
              (0, 'Quantitative Aptitude', 'Percentage', 'What is 25% of 480?', '25% is one quarter. 480 divided by 4 is 120.', 1, '100', '120', '140', '160'),
              (1, 'Quantitative Aptitude', 'Profit and Loss', 'An item costs ₹200 and sells for ₹240. What is the profit percentage on cost?', 'The profit is ₹40. 40 divided by 200, multiplied by 100, is 20%.', 1, '10%', '20%', '25%', '40%'),
              (2, 'Reasoning', 'Series', 'What comes next in the series: 2, 4, 8, 16, ...?', 'Each term is twice the previous term, so 16 multiplied by 2 is 32.', 2, '20', '24', '32', '64'),
              (3, 'Reasoning', 'Coding-Decoding', 'If A=1, B=2, through Z=26, what is the sum of the letter values in CAT?', 'C=3, A=1 and T=20. Their sum is 24.', 2, '21', '23', '24', '26'),
              (4, 'English', 'Error Detection', 'Choose the correct word: She ___ to school every day.', 'A singular third-person subject takes “goes” in the simple present tense.', 1, 'go', 'goes', 'going', 'gone'),
              (5, 'English', 'Vocabulary', 'Which word is closest in meaning to “rapid”?', 'Rapid means fast or quick.', 2, 'slow', 'late', 'fast', 'weak'),
              (6, 'General Awareness', 'Static GK', 'What is the chemical formula of water?', 'A water molecule contains two hydrogen atoms and one oxygen atom: H₂O.', 2, 'CO₂', 'O₂', 'H₂O', 'NaCl'),
              (7, 'General Awareness', 'Geography', 'Which is the largest continent by area?', 'Asia is the largest continent by land area.', 0, 'Asia', 'Africa', 'Europe', 'Australia');

            WITH stages AS (
                SELECT s."Id" AS stage_id, e."Name" AS exam_name, s."Name" AS stage_name
                FROM "ExamStages" s
                JOIN "Exams" e ON e."Id" = s."ExamId"
                WHERE NOT (e."Name" = 'SSC CGL' AND s."Name" = 'Tier 1')
            )
            INSERT INTO "MockTests" ("Id", "UserId", "SpaceId", "ExamStageId", "Title", "Description", "DurationSeconds", "FloorAtZero", "NavigationRule", "CreatedAt")
            SELECT md5('spilton-foundation-test:' || stage_id::text)::uuid,
                   NULL, NULL, stage_id,
                   exam_name || ' · ' || stage_name || ' foundation practice',
                   'Eight original foundation questions for practice. This is not an official paper or verified previous-year paper.',
                   600, false, 'FREE', TIMESTAMPTZ '2026-09-27 00:00:00+05:30'
            FROM stages
            ON CONFLICT ("Id") DO NOTHING;

            WITH stages AS (
                SELECT s."Id" AS stage_id
                FROM "ExamStages" s
                JOIN "Exams" e ON e."Id" = s."ExamId"
                WHERE NOT (e."Name" = 'SSC CGL' AND s."Name" = 'Tier 1')
            ), section_subjects AS (
                SELECT stages.stage_id, subjects."Id" AS subject_id, subjects."Name" AS subject_name,
                       CASE subjects."Name"
                           WHEN 'Quantitative Aptitude' THEN 0
                           WHEN 'Reasoning' THEN 1
                           WHEN 'English' THEN 2
                           ELSE 3
                       END AS section_position
                FROM stages
                JOIN "Subjects" subjects ON subjects."ExamStageId" = stages.stage_id
                WHERE subjects."Name" IN ('Quantitative Aptitude', 'Reasoning', 'English', 'General Awareness')
            )
            INSERT INTO "MockSections" ("Id", "MockTestId", "SubjectId", "Name", "Position", "MarksCorrect", "NegativeMarks")
            SELECT md5('spilton-foundation-section:' || stage_id::text || ':' || subject_id::text)::uuid,
                   md5('spilton-foundation-test:' || stage_id::text)::uuid,
                   subject_id, subject_name, section_position, 2.0, 0.5
            FROM section_subjects
            ON CONFLICT ("Id") DO NOTHING;

            WITH stages AS (
                SELECT s."Id" AS stage_id
                FROM "ExamStages" s
                JOIN "Exams" e ON e."Id" = s."ExamId"
                WHERE NOT (e."Name" = 'SSC CGL' AND s."Name" = 'Tier 1')
            ), question_rows AS (
                SELECT stages.stage_id, subjects."Id" AS subject_id, topics."Id" AS topic_id, templates.*
                FROM stages
                CROSS JOIN "_SpiltonFoundationTemplates" templates
                JOIN "Subjects" subjects ON subjects."ExamStageId" = stages.stage_id AND subjects."Name" = templates."SubjectName"
                JOIN "Topics" topics ON topics."SubjectId" = subjects."Id" AND topics."Name" = templates."TopicName"
            )
            INSERT INTO "Questions" ("Id", "UserId", "SpaceId", "TopicId", "Text", "Explanation", "Difficulty", "SourceType", "SourceResourceId", "SourceMetadataJson", "CorrectOptionIndex")
            SELECT md5('spilton-foundation-question:' || stage_id::text || ':' || "Position"::text)::uuid,
                   NULL, NULL, topic_id, "QuestionText", "Explanation", 'EASY', 'MANUAL_FOUNDATION', NULL,
                   '{"label":"Original foundation question; not PYQ"}', "CorrectOptionIndex"
            FROM question_rows
            ON CONFLICT ("Id") DO NOTHING;

            WITH stages AS (
                SELECT s."Id" AS stage_id
                FROM "ExamStages" s
                JOIN "Exams" e ON e."Id" = s."ExamId"
                WHERE NOT (e."Name" = 'SSC CGL' AND s."Name" = 'Tier 1')
            ), question_rows AS (
                SELECT stages.stage_id, templates.*
                FROM stages CROSS JOIN "_SpiltonFoundationTemplates" templates
            ), option_rows AS (
                SELECT question_rows.*,
                       option_index,
                       CASE option_index
                           WHEN 0 THEN "Option0"
                           WHEN 1 THEN "Option1"
                           WHEN 2 THEN "Option2"
                           ELSE "Option3"
                       END AS option_text
                FROM question_rows CROSS JOIN generate_series(0, 3) AS option_index
            )
            INSERT INTO "QuestionOptions" ("Id", "QuestionId", "Index", "Text")
            SELECT md5('spilton-foundation-option:' || stage_id::text || ':' || "Position"::text || ':' || option_index::text)::uuid,
                   md5('spilton-foundation-question:' || stage_id::text || ':' || "Position"::text)::uuid,
                   option_index, option_text
            FROM option_rows
            ON CONFLICT ("Id") DO NOTHING;

            WITH stages AS (
                SELECT s."Id" AS stage_id
                FROM "ExamStages" s
                JOIN "Exams" e ON e."Id" = s."ExamId"
                WHERE NOT (e."Name" = 'SSC CGL' AND s."Name" = 'Tier 1')
            ), question_rows AS (
                SELECT stages.stage_id, subjects."Id" AS subject_id, templates."Position"
                FROM stages
                CROSS JOIN "_SpiltonFoundationTemplates" templates
                JOIN "Subjects" subjects ON subjects."ExamStageId" = stages.stage_id AND subjects."Name" = templates."SubjectName"
            )
            INSERT INTO "MockTestQuestions" ("MockTestId", "QuestionId", "MockSectionId", "Position")
            SELECT md5('spilton-foundation-test:' || stage_id::text)::uuid,
                   md5('spilton-foundation-question:' || stage_id::text || ':' || "Position"::text)::uuid,
                   md5('spilton-foundation-section:' || stage_id::text || ':' || subject_id::text)::uuid,
                   "Position"
            FROM question_rows
            ON CONFLICT ("MockTestId", "QuestionId") DO NOTHING;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DELETE FROM "MockTests"
            WHERE "Id" IN (
                SELECT md5('spilton-foundation-test:' || s."Id"::text)::uuid
                FROM "ExamStages" s
                JOIN "Exams" e ON e."Id" = s."ExamId"
                WHERE NOT (e."Name" = 'SSC CGL' AND s."Name" = 'Tier 1')
            );

            DELETE FROM "Questions"
            WHERE "SourceType" = 'MANUAL_FOUNDATION'
              AND "UserId" IS NULL
              AND "SpaceId" IS NULL;
            """);
    }
}
