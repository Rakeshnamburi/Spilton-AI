using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spilton.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddGoogleOAuthAndDatabaseStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoogleLoginChallenge",
                columns: table => new
                {
                    StateHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CodeVerifier = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Nonce = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UsedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleLoginChallenge", x => x.StateHash);
                });

            migrationBuilder.CreateTable(
                name: "GoogleLoginTicket",
                columns: table => new
                {
                    TicketHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UsedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleLoginTicket", x => x.TicketHash);
                    table.ForeignKey(
                        name: "FK_GoogleLoginTicket_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoredFiles",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Content = table.Column<byte[]>(type: "bytea", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredFiles", x => x.Name);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoogleLoginChallenge_ExpiresAt",
                table: "GoogleLoginChallenge",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_GoogleLoginTicket_ExpiresAt",
                table: "GoogleLoginTicket",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_GoogleLoginTicket_UserId",
                table: "GoogleLoginTicket",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoogleLoginChallenge");

            migrationBuilder.DropTable(
                name: "GoogleLoginTicket");

            migrationBuilder.DropTable(
                name: "StoredFiles");
        }
    }
}
