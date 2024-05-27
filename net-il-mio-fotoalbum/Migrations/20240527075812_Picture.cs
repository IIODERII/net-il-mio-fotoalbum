using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace net_il_mio_fotoalbum.Migrations
{
    /// <inheritdoc />
    public partial class Picture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryPicture_pizza_PicturesId",
                table: "CategoryPicture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pizza",
                table: "pizza");

            migrationBuilder.RenameTable(
                name: "pizza",
                newName: "Pictures");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryPicture_Pictures_PicturesId",
                table: "CategoryPicture",
                column: "PicturesId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryPicture_Pictures_PicturesId",
                table: "CategoryPicture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures");

            migrationBuilder.RenameTable(
                name: "Pictures",
                newName: "pizza");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pizza",
                table: "pizza",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryPicture_pizza_PicturesId",
                table: "CategoryPicture",
                column: "PicturesId",
                principalTable: "pizza",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
