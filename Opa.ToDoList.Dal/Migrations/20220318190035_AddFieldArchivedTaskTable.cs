using Microsoft.EntityFrameworkCore.Migrations;

namespace Opa.ToDoList.Dal.Migrations
{
    public partial class AddFieldArchivedTaskTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Tasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Tasks");
        }
    }
}
