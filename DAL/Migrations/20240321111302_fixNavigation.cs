using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class fixNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CheckLogs_RequestId",
                table: "CheckLogs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckList_RequestId",
                table: "CheckList",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckList_CheckRequests_RequestId",
                table: "CheckList",
                column: "RequestId",
                principalTable: "CheckRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckLogs_CheckRequests_RequestId",
                table: "CheckLogs",
                column: "RequestId",
                principalTable: "CheckRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckList_CheckRequests_RequestId",
                table: "CheckList");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckLogs_CheckRequests_RequestId",
                table: "CheckLogs");

            migrationBuilder.DropIndex(
                name: "IX_CheckLogs_RequestId",
                table: "CheckLogs");

            migrationBuilder.DropIndex(
                name: "IX_CheckList_RequestId",
                table: "CheckList");
        }
    }
}
