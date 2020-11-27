
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TreinoApi.Models
{
    public class Avaliacao
    {
        public int FilmesId {get; set;}
        
        [JsonIgnore]
        public Filmes Filmes {get; set;}

        public double NotaFilme {get; set;}
  
    }
}