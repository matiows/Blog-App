using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog_App.Migrations
{
    /// <inheritdoc />
    public partial class RenameBlogTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogsTags_Blogs_BlogId",
                table: "BlogsTags");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogsTags_Tags_TagId",
                table: "BlogsTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogsTags",
                table: "BlogsTags");

            migrationBuilder.RenameTable(
                name: "BlogsTags",
                newName: "BlogTags");

            migrationBuilder.RenameIndex(
                name: "IX_BlogsTags_TagId",
                table: "BlogTags",
                newName: "IX_BlogTags_TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogTags",
                table: "BlogTags",
                columns: new[] { "BlogId", "TagId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BlogTags_Blogs_BlogId",
                table: "BlogTags",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogTags_Tags_TagId",
                table: "BlogTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogTags_Blogs_BlogId",
                table: "BlogTags");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogTags_Tags_TagId",
                table: "BlogTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogTags",
                table: "BlogTags");

            migrationBuilder.RenameTable(
                name: "BlogTags",
                newName: "BlogsTags");

            migrationBuilder.RenameIndex(
                name: "IX_BlogTags_TagId",
                table: "BlogsTags",
                newName: "IX_BlogsTags_TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogsTags",
                table: "BlogsTags",
                columns: new[] { "BlogId", "TagId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BlogsTags_Blogs_BlogId",
                table: "BlogsTags",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogsTags_Tags_TagId",
                table: "BlogsTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
