using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FakeNewsCovid.Domain.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaggedUrls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Url = table.Column<string>(nullable: true),
                    TaggedFakeCount = table.Column<int>(nullable: false),
                    Fakebility = table.Column<int>(nullable: false),
                    InnerWeb = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaggedUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FakeReasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReasonNotFakeUrl = table.Column<string>(nullable: true),
                    TaggedUrlId = table.Column<int>(nullable: false),
                    ReasonNotFakeCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FakeReasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FakeReasons_TaggedUrls_TaggedUrlId",
                        column: x => x.TaggedUrlId,
                        principalTable: "TaggedUrls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FakeReasons_TaggedUrlId",
                table: "FakeReasons",
                column: "TaggedUrlId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FakeReasons");

            migrationBuilder.DropTable(
                name: "TaggedUrls");
        }
    }
}
