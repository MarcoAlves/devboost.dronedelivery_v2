using Microsoft.EntityFrameworkCore.Migrations;

namespace devboost.dronedelivery.felipe.Migrations
{
    public partial class permitindonulodrone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DroneId",
                table: "Pedido",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DroneId",
                table: "Pedido",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
