using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatGram.Data.Migrations
{
    /// <inheritdoc />
    public partial class ThirdInitUniqueFieldFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatProfiles_AspNetUsers_ApplicationUserId",
                table: "CatProfiles");

            migrationBuilder.DropIndex(
                name: "IX_CatProfiles_ApplicationUserId",
                table: "CatProfiles");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "CatProfiles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatProfiles_ApplicationUserId",
                table: "CatProfiles",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CatProfiles_ApplicationUserId1",
                table: "CatProfiles",
                column: "ApplicationUserId1",
                unique: true,
                filter: "[ApplicationUserId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CatProfiles_AspNetUsers_ApplicationUserId",
                table: "CatProfiles",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatProfiles_AspNetUsers_ApplicationUserId1",
                table: "CatProfiles",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatProfiles_AspNetUsers_ApplicationUserId",
                table: "CatProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_CatProfiles_AspNetUsers_ApplicationUserId1",
                table: "CatProfiles");

            migrationBuilder.DropIndex(
                name: "IX_CatProfiles_ApplicationUserId",
                table: "CatProfiles");

            migrationBuilder.DropIndex(
                name: "IX_CatProfiles_ApplicationUserId1",
                table: "CatProfiles");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "CatProfiles");

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
    }
}
