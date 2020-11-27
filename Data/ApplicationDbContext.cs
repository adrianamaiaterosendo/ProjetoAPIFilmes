using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TreinoApi.Models;

namespace TreinoApi.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Atores> Atores {get; set;}         
        public DbSet<Filmes> Filmes {get; set;}
        public DbSet<Genero> Generos {get; set;}        
        public DbSet<Usuario> Usuarios {get; set;}

        public DbSet<AtoresFilmes> AtoresFilmes {get; set;}
        public DbSet<FilmesGeneros> FilmesGeneros {get; set;}

        public DbSet<AvaliacaoFilme> AvaliacaoFilmes {get; set;}


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
        { }

         protected override void OnModelCreating(ModelBuilder builder){
                builder.Entity<AtoresFilmes>().HasKey(sc => new{sc.AtoresId, sc.FilmesId});
                builder.Entity<FilmesGeneros>().HasKey(sc=> new{sc.FilmesId, sc.GeneroId});


         }
       


    }
}