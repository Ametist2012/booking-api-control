using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingApiControl.Migrations
{
    /// <inheritdoc />
    public partial class AddClassToRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Class",
                table: "Rooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class",
                table: "Rooms");
        }
    }
}
