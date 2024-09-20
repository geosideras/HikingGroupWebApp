using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HikingRunningWebApp.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HikkingTrips_Addresses_AddressId",
                table: "HikkingTrips");

            migrationBuilder.DropForeignKey(
                name: "FK_HikkingTrips_AspNetUsers_AppUserId",
                table: "HikkingTrips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HikkingTrips",
                table: "HikkingTrips");

            migrationBuilder.RenameTable(
                name: "HikkingTrips",
                newName: "HikingTrips");

            migrationBuilder.RenameIndex(
                name: "IX_HikkingTrips_AppUserId",
                table: "HikingTrips",
                newName: "IX_HikingTrips_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_HikkingTrips_AddressId",
                table: "HikingTrips",
                newName: "IX_HikingTrips_AddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HikingTrips",
                table: "HikingTrips",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HikingTrips_Addresses_AddressId",
                table: "HikingTrips",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HikingTrips_AspNetUsers_AppUserId",
                table: "HikingTrips",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HikingTrips_Addresses_AddressId",
                table: "HikingTrips");

            migrationBuilder.DropForeignKey(
                name: "FK_HikingTrips_AspNetUsers_AppUserId",
                table: "HikingTrips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HikingTrips",
                table: "HikingTrips");

            migrationBuilder.RenameTable(
                name: "HikingTrips",
                newName: "HikkingTrips");

            migrationBuilder.RenameIndex(
                name: "IX_HikingTrips_AppUserId",
                table: "HikkingTrips",
                newName: "IX_HikkingTrips_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_HikingTrips_AddressId",
                table: "HikkingTrips",
                newName: "IX_HikkingTrips_AddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HikkingTrips",
                table: "HikkingTrips",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HikkingTrips_Addresses_AddressId",
                table: "HikkingTrips",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HikkingTrips_AspNetUsers_AppUserId",
                table: "HikkingTrips",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
