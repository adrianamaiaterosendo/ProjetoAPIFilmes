using System;
using System.Collections.Generic;
using System.Linq;
using TreinoApi.Models;
using TreinoApi.Data;


namespace TreinoApi.Classes
{
    public class FilmeTemp
    {

        private readonly ApplicationDbContext database;
        public int Id {get; set;}
        public string Nome {get; set;}
        public double Duracao {get; set;}
        public string Idioma {get; set;}
        
        public string DataLancamento{get; set;}
        
        public List <int> AtoresFilmesId{get; set;}
        public List <int> FilmesGenerosId{get; set;}

        public double Avaliacao {get; set; }

        public bool Disponivel {get; set;}


      
        }
    }
