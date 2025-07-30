using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactTemplate.Authentication.Data.Migrations
{
    /// <inheritdoc />
    public partial class CustomIdentityUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "first_name",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "first_name",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "last_name",
                schema: "identity",
                table: "AspNetUsers");
        }
    }
}
