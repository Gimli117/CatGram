using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatGram.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdentitySetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "CatProfiles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatProfiles_ApplicationUserId",
                table: "CatProfiles",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CatProfiles_AspNetUsers_ApplicationUserId",
                table: "CatProfiles",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatProfiles_AspNetUsers_ApplicationUserId",
                table: "CatProfiles");

            migrationBuilder.DropIndex(
                name: "IX_CatProfiles_ApplicationUserId",
                table: "CatProfiles");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "CatProfiles");

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
    }
}
