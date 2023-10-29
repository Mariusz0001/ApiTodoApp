using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTodoApp.Migrations
{
    public partial class PriorityAndDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userId",
                table: "PersonalTasks",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PersonalTasks",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "PersonalTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "PersonalTasks");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "PersonalTasks");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PersonalTasks",
                newName: "userId");
        }
    }
}
