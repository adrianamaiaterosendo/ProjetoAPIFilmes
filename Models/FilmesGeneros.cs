using System.Collections.Generic;
using Newtonsoft.Json;

namespace TreinoApi.Models
{
    public class FilmesGeneros
    {
        
        public int GeneroId {get; set;}

         [JsonIgnore]
        public Genero Genero {get; set;}
        public int FilmesId {get; set;}

         [JsonIgnore]
        public Filmes Filmes {get; set;}
    }
}