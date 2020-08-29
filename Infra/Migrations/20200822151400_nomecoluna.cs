using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace devboost.dronedelivery.felipe.Migrations
{
    public partial class nomecoluna : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataHoraAtualizacao",
                table: "Pedido");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataHoraFinalizacao",
                table: "Pedido",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataHoraFinalizacao",
                table: "Pedido");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataHoraAtualizacao",
                table: "Pedido",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
