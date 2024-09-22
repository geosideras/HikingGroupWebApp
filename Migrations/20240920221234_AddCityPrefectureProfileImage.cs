using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HikingRunningWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCityPrefectureProfileImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HikingTrips_Addresses_AddressId",
                table: "HikingTrips");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "HikingTrips",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_HikingTrips_Addresses_AddressId",
                table: "HikingTrips",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HikingTrips_Addresses_AddressId",
                table: "HikingTrips");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "HikingTrips",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HikingTrips_Addresses_AddressId",
                table: "HikingTrips",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
