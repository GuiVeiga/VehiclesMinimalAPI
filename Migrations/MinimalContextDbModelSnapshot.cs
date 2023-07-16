﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VehiclesMinimalAPI.Data;

#nullable disable

namespace VehiclesMinimalAPI.Migrations
{
    [DbContext(typeof(MinimalContextDb))]
    partial class MinimalContextDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VehiclesMinimalAPI.Models.Automovel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cor")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("Disponivel")
                        .HasColumnType("bit");

                    b.Property<string>("Marca")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasColumnType("varchar(7)");

                    b.HasKey("Id");

                    b.ToTable("Automoveis", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Cor = "Preto",
                            Disponivel = true,
                            Marca = "Chevrolet",
                            Modelo = "Joy",
                            Placa = "ABC1234"
                        },
                        new
                        {
                            Id = 2,
                            Cor = "Prata",
                            Disponivel = true,
                            Marca = "Volkswagen",
                            Modelo = "Gol",
                            Placa = "DEF5678"
                        },
                        new
                        {
                            Id = 3,
                            Cor = "Branco",
                            Disponivel = true,
                            Marca = "Ford",
                            Modelo = "Ka",
                            Placa = "GHI9101"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}