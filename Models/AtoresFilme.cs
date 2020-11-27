
using System.Collections.Generic;
using Newtonsoft.Json;
namespace TreinoApi.Models
{
    public class AtoresFilmes
    {

        
        public int AtoresId {get; set;}
        
        [JsonIgnore]
        public Atores Atores {get; set;}
        public int FilmesId {get; set;}
        
        [JsonIgnore]
        public Filmes Filmes {get; set;}

    }
}