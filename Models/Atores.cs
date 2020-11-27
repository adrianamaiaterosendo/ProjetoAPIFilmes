using System.Collections.Generic;
using Newtonsoft.Json;

namespace TreinoApi.Models
{
    public class Atores
    {
    public int Id {get; set;}
    public string Nome {get; set;}

    public ICollection <AtoresFilmes> AtoresFilmes{get; set;}
    
        
    }
}