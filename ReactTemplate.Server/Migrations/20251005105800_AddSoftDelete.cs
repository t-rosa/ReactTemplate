using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactTemplate.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "weather_forecasts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "weather_forecasts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "weather_forecasts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_weather_forecasts_deleted_at",
                table: "weather_forecasts",
                column: "deleted_at");

            migrationBuilder.CreateIndex(
                name: "ix_weather_forecasts_is_deleted",
                table: "weather_forecasts",
                column: "is_deleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_weather_forecasts_deleted_at",
                table: "weather_forecasts");

            migrationBuilder.DropIndex(
                name: "ix_weather_forecasts_is_deleted",
                table: "weather_forecasts");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "weather_forecasts");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "weather_forecasts");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "weather_forecasts");
        }
    }
}
