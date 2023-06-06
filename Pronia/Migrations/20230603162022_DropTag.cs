using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pronia.Migrations
{
    public partial class DropTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantTags_Plants_PlantId",
                table: "PlantTags");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantTags_Tag_TagId",
                table: "PlantTags");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_PlantTags_PlantId",
                table: "PlantTags");

            migrationBuilder.DropIndex(
                name: "IX_PlantTags_TagId",
                table: "PlantTags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

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
                name: "FK_PlantTags_Tag_TagId",
                table: "PlantTags",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
