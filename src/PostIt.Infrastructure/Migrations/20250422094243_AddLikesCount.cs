using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostIt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLikesCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Views",
                table: "Posts",
                newName: "ViewCount");

            migrationBuilder.AddColumn<int>(
                name: "PostsCount",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "Posts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostsCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "ViewCount",
                table: "Posts",
                newName: "Views");
        }
    }
}
