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


namespace TreinoApi.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class AtoresController : ControllerBase
    {
      private readonly ApplicationDbContext database;
        private HATEOAS.HATEOAS HATEOAS;

        public AtoresController(ApplicationDbContext database){
            this.database = database;
            HATEOAS = new HATEOAS.HATEOAS ("localhost:5001/api/v1/Atores");
            HATEOAS.AddAction("GET_INFO", "GET");
            HATEOAS.AddAction("DELETE_PRODUCT", "DELETE");
            HATEOAS.AddAction("EDIT_PRODUCT", "PATCH");
        }
        [HttpGet]   
        public IActionResult ListaAtores (){
            var atores = database.Atores.Include(f => f.AtoresFilmes).ToList();
             List<AtoresContainer> atoresHATEOAS = new List<AtoresContainer>();
            foreach(var ator in atores){
                AtoresContainer atorHATEOAS = new AtoresContainer();
                atorHATEOAS.atores = ator;
                atorHATEOAS.links = HATEOAS.GetActions(ator.Id.ToString());
                atoresHATEOAS.Add(atorHATEOAS);

            }
           return Ok(new{atoresHATEOAS}); 
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id){

            try{
                var atores = database.Atores.Include(f => f.AtoresFilmes).First(f=> f.Id == id);
               
                AtoresContainer atorHATEOAS = new AtoresContainer();
                atorHATEOAS.atores = atores;
                atorHATEOAS.links = HATEOAS.GetActions(atores.Id.ToString());
            return Ok(atorHATEOAS);
               
            }catch(Exception ){  

            Response.StatusCode = 404;          
            return new ObjectResult (new{msg= "Id inválido"}); }

        }

        [HttpPost]
        public IActionResult Post([FromBody] AtoresTemp aTemp){
            Atores atores = new Atores();

              if(aTemp.Nome.Length <=1){
                Response.StatusCode = 400;
                return new ObjectResult (new{msg="O ator tem que ter um nome válido!"});
            }
            atores.Nome = aTemp.Nome;
            
            database.Atores.Add(atores);
            database.SaveChanges();


            var atorId = database.Atores.Where(a=> a.Nome == atores.Nome).First(a=> a.Id == atores.Id);

             foreach (var atorFilmeId in aTemp.AtoresFilmesId){
                       
                        AtoresFilmes atoresFilmes1 = new AtoresFilmes();
                        atoresFilmes1.FilmesId = atorFilmeId;
                        atoresFilmes1.AtoresId = atorId.Id;

                        database.AtoresFilmes.Add(atoresFilmes1);                        
                        database.SaveChanges();
                    };

                     

            
            Response.StatusCode = 201;
            return new ObjectResult (new{msg = "Ator criado com sucesso!" });
        }

        [HttpPatch]
        public IActionResult Editar ([FromBody] AtoresTemp ator){
            var atorTemp = database.Atores.First(at => at.Id == ator.Id);
            if(ator.Id > 0){
                try{
                    var a = database.Atores.First(atemp=> atemp.Id == ator.Id);                    

                    if(a != null){

                        a.Nome = ator.Nome != null ? ator.Nome : a.Nome;
                        database.SaveChanges();

                        if(ator.Nome.Length <= 1){

                        Response.StatusCode = 400;
                        return new ObjectResult (new{msg="Nome inválido ou vazio, tente outro nome!"}); 
                            
                        
                        

                        }else{                         
                                                   
                        
                        if(ator.AtoresFilmesId != null){
                            var atores = database.AtoresFilmes.Where(f=> f.AtoresId == ator.Id);
                            database.AtoresFilmes.RemoveRange(atores);
                            database.SaveChanges();
                        
                            var AtoresFilmesTemp = database.AtoresFilmes.ToList();
                            foreach (var atorFilmeId in ator.AtoresFilmesId){
                       
                            AtoresFilmes atoresFilmes1 = new AtoresFilmes();
                            atoresFilmes1.FilmesId = atorFilmeId;
                            atoresFilmes1.AtoresId = ator.Id;

                            database.AtoresFilmes.Add(atoresFilmes1);                        
                            database.SaveChanges();
                            }; }                     
                        
                        }
                    }
              
              
                 return Ok();  }               
                  
                catch{
                    Response.StatusCode = 404;
                    return new ObjectResult (new{msg="Id Ator / Filme inválido, ou já inserido anteriormente"}); 
                }

            }   else{
                    Response.StatusCode = 404;
                    return new ObjectResult (new{msg="Id inválido"});
                }

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id){

               try{
                var atores = database.Atores.First(f=> f.Id == id);
                database.Atores.Remove(atores);
                database.SaveChanges();
               
            return Ok("Ator excluído com sucesso"); 
            }catch(Exception ){  

            Response.StatusCode = 404;          
            return new ObjectResult (new{msg= "Id inválido"}); }


        }

     public class AtoresContainer{
            public Atores atores {get; set;}

            public Link[] links {get; set;}
        }
 
    }
}