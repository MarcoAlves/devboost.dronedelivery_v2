﻿// <auto-generated />
using System;
using devboost.dronedelivery.felipe.EF.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace devboost.dronedelivery.felipe.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20200822131540_initialcreate")]
    partial class initialcreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("devboost.dronedelivery.felipe.Models.Drone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Autonomia")
                        .HasColumnType("int");

                    b.Property<int>("Capacidade")
                        .HasColumnType("int");

                    b.Property<int>("Carga")
                        .HasColumnType("int");

                    b.Property<int>("Velocidade")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Drone");
                });

            modelBuilder.Entity("devboost.dronedelivery.felipe.Models.Pedido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DataHoraAtualizacao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataHoraInclusao")
                        .HasColumnType("datetime2");

                    b.Property<int>("DroneId")
                        .HasColumnType("int");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<int>("Peso")
                        .HasColumnType("int");

                    b.Property<int>("Situacao")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Pedido");
                });
#pragma warning restore 612, 618
        }
    }
}
