using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kinobot.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexOnUserIdMediaIdMediaType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MediaType",
                table: "WatchlistMedias",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistMedias_TelegramUserId_MediaType_MediaId",
                table: "WatchlistMedias",
                columns: new[] { "TelegramUserId", "MediaType", "MediaId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WatchlistMedias_TelegramUserId_MediaType_MediaId",
                table: "WatchlistMedias");

            migrationBuilder.AlterColumn<string>(
                name: "MediaType",
                table: "WatchlistMedias",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
