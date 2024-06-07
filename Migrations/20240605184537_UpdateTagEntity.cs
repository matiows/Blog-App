using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog_App.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTagEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "UserAccounts",
                newName: "LastUpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "LastEditedDateTime",
                table: "Blogs",
                newName: "LastUpdatedDateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdatedDateTime",
                table: "UserAccounts",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDateTime",
                table: "Blogs",
                newName: "LastEditedDateTime");
        }
    }
}
