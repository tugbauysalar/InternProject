using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teams_TeamId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LeadName",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "TeamLeadId",
                table: "Teams",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamLeadId",
                table: "Teams",
                column: "TeamLeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teams_TeamId",
                table: "AspNetUsers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_TeamLeadId",
                table: "Teams",
                column: "TeamLeadId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teams_TeamId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_TeamLeadId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_TeamLeadId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamLeadId",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "LeadName",
                table: "Teams",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teams_TeamId",
                table: "AspNetUsers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
