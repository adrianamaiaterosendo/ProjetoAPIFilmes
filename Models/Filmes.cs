using System;
using System.Collections.Generic;

namespace TreinoApi.Models
{
    public class Filmes
    {
        public int Id {get; set;}
        public string Nome {get; set;}
        public double Duracao {get; set;}
        public string Idioma {get; set;}
        
        public string DataLancamento{get; set;}
        
        public ICollection <AtoresFilmes> AtoresFilmes{get; set;}
        public ICollection <FilmesGeneros> FilmesGeneros{get; set;}

        public bool Disponivel {get; set;}
    }
}