using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatGram.Data.Migrations
{
    /// <inheritdoc />
    public partial class LinkCatToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "color",
                table: "CatProfiles",
                newName: "Color");

            migrationBuilder.AddColumn<int>(
                name: "CatProfileId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CatProfileId",
                table: "AspNetUsers",
                column: "CatProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CatProfiles_CatProfileId",
                table: "AspNetUsers",
                column: "CatProfileId",
                principalTable: "CatProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CatProfiles_CatProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CatProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CatProfileId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Color",
                table: "CatProfiles",
                newName: "color");
        }
    }
}
