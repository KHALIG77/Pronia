using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pronia.Migrations
{
    public partial class AlterCommentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "PlantComments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlantComments_AppUserId",
                table: "PlantComments",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantComments_PlantId",
                table: "PlantComments",
                column: "PlantId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantComments_AspNetUsers_AppUserId",
                table: "PlantComments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantComments_Plants_PlantId",
                table: "PlantComments",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantComments_AspNetUsers_AppUserId",
                table: "PlantComments");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantComments_Plants_PlantId",
                table: "PlantComments");

            migrationBuilder.DropIndex(
                name: "IX_PlantComments_AppUserId",
                table: "PlantComments");

            migrationBuilder.DropIndex(
                name: "IX_PlantComments_PlantId",
                table: "PlantComments");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "PlantComments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
