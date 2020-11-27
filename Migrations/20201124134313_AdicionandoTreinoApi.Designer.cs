﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TreinoApi.Data;

namespace TreinoApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20201124134313_AdicionandoTreinoApi")]
    partial class AdicionandoTreinoApi
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TreinoApi.Models.Atores", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Atores");
                });

            modelBuilder.Entity("TreinoApi.Models.AtoresFilmes", b =>
                {
                    b.Property<int>("AtoresId")
                        .HasColumnType("int");

                    b.Property<int>("FilmesId")
                        .HasColumnType("int");

                    b.HasKey("AtoresId", "FilmesId");

                    b.HasIndex("FilmesId");

                    b.ToTable("AtoresFilmes");
                });

            modelBuilder.Entity("TreinoApi.Models.Filmes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("DataLancamento")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("Disponivel")
                        .HasColumnType("tinyint(1)");

                    b.Property<double>("Duracao")
                        .HasColumnType("double");

                    b.Property<string>("Idioma")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Filmes");
                });

            modelBuilder.Entity("TreinoApi.Models.FilmesGeneros", b =>
                {
                    b.Property<int>("FilmesId")
                        .HasColumnType("int");

                    b.Property<int>("GeneroId")
                        .HasColumnType("int");

                    b.HasKey("FilmesId", "GeneroId");

                    b.HasIndex("GeneroId");

                    b.ToTable("FilmesGeneros");
                });

            modelBuilder.Entity("TreinoApi.Models.Genero", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Generos");
                });

            modelBuilder.Entity("TreinoApi.Models.AtoresFilmes", b =>
                {
                    b.HasOne("TreinoApi.Models.Atores", "Atores")
                        .WithMany("AtoresFilmes")
                        .HasForeignKey("AtoresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TreinoApi.Models.Filmes", "Filmes")
                        .WithMany("AtoresFilmes")
                        .HasForeignKey("FilmesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TreinoApi.Models.FilmesGeneros", b =>
                {
                    b.HasOne("TreinoApi.Models.Filmes", "Filmes")
                        .WithMany("FilmesGeneros")
                        .HasForeignKey("FilmesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TreinoApi.Models.Genero", "Genero")
                        .WithMany("FilmesGeneros")
                        .HasForeignKey("GeneroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
