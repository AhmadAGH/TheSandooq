using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSandooq.Migrations
{
    /// <inheritdoc />
    public partial class m4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "dbExpenses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "dbExpenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
