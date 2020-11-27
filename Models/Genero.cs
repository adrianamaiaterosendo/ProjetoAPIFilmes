using System;
using System.Collections.Generic;

namespace TreinoApi.Models
{
    public class Genero
    {
        public int Id {get; set;}
        public string Nome {get; set;}

        public ICollection <FilmesGeneros> FilmesGeneros{get; set;}
        
    }
}