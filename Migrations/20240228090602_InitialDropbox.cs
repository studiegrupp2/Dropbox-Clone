using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_2.Migrations
{
    /// <inheritdoc />
    public partial class InitialDropbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fileName",
                table: "AppFiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fileName",
                table: "AppFiles");
        }
    }
}
