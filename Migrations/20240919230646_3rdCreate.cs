using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HikingRunningWebApp.Migrations
{
    /// <inheritdoc />
    public partial class _3rdCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HikkingTripCategory",
                table: "HikingTrips",
                newName: "HikingTripCategory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HikingTripCategory",
                table: "HikingTrips",
                newName: "HikkingTripCategory");
        }
    }
}
