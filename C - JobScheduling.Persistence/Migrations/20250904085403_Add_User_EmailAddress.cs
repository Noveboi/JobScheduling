using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobScheduling.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_User_EmailAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Key",
                table: "UserJob",
                newName: "JobKey");

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "JobKey",
                table: "UserJob",
                newName: "Key");
        }
    }
}
