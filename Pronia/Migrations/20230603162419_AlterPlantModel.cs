using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pronia.Migrations
{
    public partial class AlterPlantModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PlantTags_PlantId",
                table: "PlantTags",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantTags_TagId",
                table: "PlantTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantTags_Plants_PlantId",
                table: "PlantTags",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantTags_Tags_TagId",
                table: "PlantTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantTags_Plants_PlantId",
                table: "PlantTags");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantTags_Tags_TagId",
                table: "PlantTags");

            migrationBuilder.DropIndex(
                name: "IX_PlantTags_PlantId",
                table: "PlantTags");

            migrationBuilder.DropIndex(
                name: "IX_PlantTags_TagId",
                table: "PlantTags");
        }
    }
}
