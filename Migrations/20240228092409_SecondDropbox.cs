using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_2.Migrations
{
    /// <inheritdoc />
    public partial class SecondDropbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "fileName",
                table: "AppFiles",
                newName: "FileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "AppFiles",
                newName: "fileName");
        }
    }
}
