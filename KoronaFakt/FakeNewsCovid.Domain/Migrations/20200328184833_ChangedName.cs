using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeNewsCovid.Domain.Migrations
{
    public partial class ChangedName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReasonNotFakeUrl",
                table: "FakeReasons");

            migrationBuilder.AddColumn<string>(
                name: "ReasonNotFake",
                table: "FakeReasons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReasonNotFake",
                table: "FakeReasons");

            migrationBuilder.AddColumn<string>(
                name: "ReasonNotFakeUrl",
                table: "FakeReasons",
                type: "text",
                nullable: true);
        }
    }
}
