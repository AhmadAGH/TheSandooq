using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSandooq.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "dbIncomes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "dbIncomes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
