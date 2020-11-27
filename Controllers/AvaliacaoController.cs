using Microsoft.AspNetCore.Mvc;
using TreinoApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using TreinoApi.Models;
using TreinoApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TreinoApi.HATEOAS;
using Newtonsoft.Json;

namespace TreinoApi.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class AvaliacaoController : ControllerBase
    {
         private readonly ApplicationDbContext database;

         private HATEOAS.HATEOAS HATEOAS;

           public AvaliacaoController(ApplicationDbContext database){
            this.database = database;
            HATEOAS = new HATEOAS.HATEOAS ("localhost:5001/api/v1/Avaliacao");
            HATEOAS.AddAction("GET_INFO", "GET");
            HATEOAS.AddAction("DELETE_PRODUCT", "DELETE");
            
          }
        [HttpGet]   
        public IActionResult ListaAvaliacoes(){
            var avaliacao = database.AvaliacaoFilmes.Include(f=> f.Filmes).ToList();

             List<AvaliacaoContainer2> avaliacaoHATEOAS = new List<AvaliacaoContainer2>();
            foreach(var av in avaliacao){
                AvaliacaoContainer2 avaliacaofilmeHATEOAS = new AvaliacaoContainer2();
                avaliacaofilmeHATEOAS.avaliacao = av;
                avaliacaofilmeHATEOAS.links = HATEOAS.GetActions(av.Id.ToString());
                avaliacaoHATEOAS.Add(avaliacaofilmeHATEOAS);

            }
           return Ok(new{avaliacaoHATEOAS}); 
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id){

            try{
                var filmesAv = database.AvaliacaoFilmes.Include(f => f.Filmes).First(f=> f.FilmesId == id);

                
                 var filmeAvm = database.AvaliacaoFilmes.Where(a=> a.FilmesId == id).ToList();
                 double avaliacao = 0;
            foreach(var media in filmeAvm){
                
                avaliacao = media.NotaFilme + avaliacao;

            }
            var mediaAvaliacao = avaliacao / filmeAvm.Count();
                
               
                AvaliacaoContainer avaliacaoHATEOAS = new AvaliacaoContainer();
                avaliacaoHATEOAS.mediaAvaliacao = mediaAvaliacao;
                avaliacaoHATEOAS.filmes = filmesAv.Filmes;
                avaliacaoHATEOAS.avaliacao = filmesAv;                
                avaliacaoHATEOAS.links = HATEOAS.GetActions(filmesAv.Id.ToString());
            return Ok(avaliacaoHATEOAS);
               
            }catch(Exception ){  

            Response.StatusCode = 404;          
            return new ObjectResult (new{msg= "Id de filme inválido, ou filme sem avaliação cadastrada"}); }

        }
            
     

        [HttpPost]
        public IActionResult Post([FromBody] AvaliacaoTemp aTemp){
            AvaliacaoFilme avaliacaoFilme = new AvaliacaoFilme();
              if(aTemp.FilmesId <= 0)            
            {
                Response.StatusCode = 400;
                return new ObjectResult (new{msg="O filme tem que ter um Id válido!"});
            }
            avaliacaoFilme.FilmesId = aTemp.FilmesId;

            if(aTemp.NotaFilme <= 0 || aTemp.NotaFilme >5){

                 Response.StatusCode = 400;
                return new ObjectResult (new{msg="A Nota do filme tem ser entre 0 e 5!"});

            }
            avaliacaoFilme.NotaFilme = aTemp.NotaFilme;

            

            database.AvaliacaoFilmes.Add(avaliacaoFilme);
            database.SaveChanges();
            
            Response.StatusCode = 201;
            return new ObjectResult (new{msg = "Avaliação cadastrada com sucesso!" });
        }

        
         [HttpDelete("{id}")]
        public IActionResult Delete(int id){

               try{
                var avaliacao = database.AvaliacaoFilmes.First(f=> f.Id == id);
                database.AvaliacaoFilmes.Remove(avaliacao);
                database.SaveChanges();
               
            return Ok("Avaliação excluída com sucesso"); 
            }catch(Exception ){  

            Response.StatusCode = 404;          
            return new ObjectResult (new{msg= "Id  da avaliação inválido"}); }


        }

        public class AvaliacaoContainer{
            
            [JsonIgnore]
            public AvaliacaoFilme avaliacao {get; set;}

            public double mediaAvaliacao {get; set;}

            public Filmes filmes {get; set;}

            public Link[] links {get; set;}

            
        }
        
        public class AvaliacaoContainer2{
            
           
            public AvaliacaoFilme avaliacao {get; set;}

            public Filmes filmes {get; set;}

            public Link[] links {get; set;}

            
        }


    }
}