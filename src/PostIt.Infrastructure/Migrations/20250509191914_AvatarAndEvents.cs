using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostIt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AvatarAndEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsConfirmed",
                table: "Users",
                newName: "IsEmailConfirmed");

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IsEmailConfirmed",
                table: "Users",
                newName: "IsConfirmed");
        }
    }
}
