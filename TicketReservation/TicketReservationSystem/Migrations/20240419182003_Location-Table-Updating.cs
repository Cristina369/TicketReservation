using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketReservationSystem.Migrations
{
    /// <inheritdoc />
    public partial class LocationTableUpdating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationPath",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationPath",
                table: "Locations");
        }
    }
}
