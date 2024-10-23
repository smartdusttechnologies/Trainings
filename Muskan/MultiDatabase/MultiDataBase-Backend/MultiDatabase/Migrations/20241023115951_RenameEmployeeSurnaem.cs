using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiDatabase.Migrations
{
    public partial class RenameEmployeeSurnaem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Employees",
                newName: "EmployeeSurname");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmployeeSurname",
                table: "Employees",
                newName: "Surname");
        }
    }
}
