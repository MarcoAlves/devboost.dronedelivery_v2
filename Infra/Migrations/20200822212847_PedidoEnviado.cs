using Microsoft.EntityFrameworkCore.Migrations;

namespace devboost.dronedelivery.felipe.Migrations
{
    public partial class PedidoEnviado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PedidoEnviado",
                table: "PedidoDrones",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PedidoEnviado",
                table: "PedidoDrones");
        }
    }
}
