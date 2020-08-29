using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace devboost.dronedelivery.felipe.Migrations
{
    public partial class PedidoDrone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PedidoDrones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DroneId = table.Column<int>(nullable: false),
                    PedidoId = table.Column<int>(nullable: false),
                    DataHoraFinalizacao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoDrones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoDrones_Drone_DroneId",
                        column: x => x.DroneId,
                        principalTable: "Drone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoDrones_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoDrones_DroneId",
                table: "PedidoDrones",
                column: "DroneId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoDrones_PedidoId",
                table: "PedidoDrones",
                column: "PedidoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoDrones");
        }
    }
}
