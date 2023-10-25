using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTodoApp.Migrations
{
    public partial class PersonalTaskUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "PersonalTasks",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userId",
                table: "PersonalTasks");
        }
    }
}
