using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoGrpc.Migrations
{
    /// <inheritdoc />
    public partial class AddRouteTravelMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TravelMode",
                table: "ToDoItems",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TravelMode",
                table: "ToDoItems");
        }
    }
}
