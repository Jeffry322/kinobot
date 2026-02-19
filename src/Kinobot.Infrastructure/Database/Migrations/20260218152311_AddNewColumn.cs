using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kinobot.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWatched",
                table: "WatchlistMedias",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWatched",
                table: "WatchlistMedias");
        }
    }
}
