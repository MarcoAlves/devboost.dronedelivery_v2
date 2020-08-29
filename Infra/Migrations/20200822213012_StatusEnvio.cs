using Microsoft.EntityFrameworkCore.Migrations;

namespace devboost.dronedelivery.felipe.Migrations
{
    public partial class StatusEnvio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PedidoEnviado",
                table: "PedidoDrones");

            migrationBuilder.AddColumn<int>(
                name: "StatusEnvio",
                table: "PedidoDrones",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusEnvio",
                table: "PedidoDrones");

            migrationBuilder.AddColumn<bool>(
                name: "PedidoEnviado",
                table: "PedidoDrones",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
