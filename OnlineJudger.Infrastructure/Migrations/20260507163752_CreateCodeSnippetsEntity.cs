using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineJudger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateCodeSnippetsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "code_snippets",
                columns: table => new
                {
                    problem_id = table.Column<int>(type: "int", nullable: false),
                    language_id = table.Column<int>(type: "int", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_code_snippets", x => new { x.problem_id, x.language_id });
                    table.ForeignKey(
                        name: "FK_code_snippets_Languages_language_id",
                        column: x => x.language_id,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_code_snippets_Problems_problem_id",
                        column: x => x.problem_id,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_code_snippets_language_id",
                table: "code_snippets",
                column: "language_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "code_snippets");
        }
    }
}
