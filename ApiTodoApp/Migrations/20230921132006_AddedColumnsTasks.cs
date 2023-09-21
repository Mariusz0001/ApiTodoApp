using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTodoApp.Migrations
{
    public partial class AddedColumnsTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "PersonalTasks",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "PersonalTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "PersonalTasks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PersonalTasks");
        }
    }
}
