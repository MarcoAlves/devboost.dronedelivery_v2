using Microsoft.EntityFrameworkCore.Migrations;

namespace devboost.dronedelivery.felipe.Migrations
{
    public partial class perfomancedrone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Perfomance",
                table: "Drone",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Perfomance",
                table: "Drone");
        }
    }
}
