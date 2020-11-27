using Microsoft.AspNetCore.Mvc;
using TreinoApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using TreinoApi.Models;
using TreinoApi.Classes;
using Microsoft.EntityFrameworkCore;
using TreinoApi.HATEOAS;

namespace TreinoApi.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class GeneroController : ControllerBase
    {
        private readonly ApplicationDbContext database;
        private HATEOAS.HATEOAS HATEOAS;

        public GeneroController(ApplicationDbContext database){
            this.database = database;
            HATEOAS = new HATEOAS.HATEOAS ("localhost:5001/api/v1/Atores");
            HATEOAS.AddAction("GET_INFO", "GET");
            HATEOAS.AddAction("DELETE_PRODUCT", "DELETE");
            HATEOAS.AddAction("EDIT_PRODUCT", "PATCH");
        }

        [HttpGet]   
        public IActionResult ListaGeneros (){
            var genero = database.Generos.Include(f=> f.FilmesGeneros).ToList();
             List<GeneroContainer> generosHATEOAS = new List<GeneroContainer>();
            foreach(var gen in genero){
                GeneroContainer generoHATEOAS = new GeneroContainer();
                generoHATEOAS.genero = gen;
                generoHATEOAS.links = HATEOAS.GetActions(gen.Id.ToString());
                generosHATEOAS.Add(generoHATEOAS);

            }
           return Ok(new{generosHATEOAS});
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id){

            try{
                var genero = database.Generos.Include(f=> f.FilmesGeneros).First(f=> f.Id == id);
                GeneroContainer generoHATEOAS = new GeneroContainer();
                generoHATEOAS.genero = genero;
                generoHATEOAS.links = HATEOAS.GetActions(genero.Id.ToString());
            return Ok(generoHATEOAS);
            
          
            }catch(Exception ){  

            Response.StatusCode = 404;          
            return new ObjectResult (new{msg= "Id inválido"}); }

        }

        [HttpPost]
        public IActionResult Post([FromBody] GeneroTemp gTemp){
            Genero genero = new Genero();
              if(gTemp.Nome.Length <=1){
                Response.StatusCode = 400;
                return new ObjectResult (new{msg="O gênero tem que ter um nome válido!"});
            }
            genero.Nome = gTemp.Nome;
            

            database.Generos.Add(genero);
            database.SaveChanges();
            
            Response.StatusCode = 201;
            return new ObjectResult (new{msg = "Produto criado com sucesso!" });
        }

        [HttpPatch]
        public IActionResult Editar ([FromBody] Genero genero){
            if(genero.Id > 0){
                try{
                    var g = database.Generos.Include(g=> g.FilmesGeneros).First(gtemp=> gtemp.Id == genero.Id);
                    

                    if(g != null){

                        g.Nome = genero.Nome != null ? genero.Nome : g.Nome;

                        if(genero.Nome.Length <= 1){

                        Response.StatusCode = 400;
                        return new ObjectResult (new{msg="Nome inválido ou vazio, tente outro nome!"}); 
                            
                        
                        

                        }else{
                            database.SaveChanges();
                        
                        }
                    if(g.FilmesGeneros != null){
                        Response.StatusCode = 400;
                        return new ObjectResult (new{msg="Não é possível alterar os filmes cadastrados, acesse a rota de filmes!"}); 


                    }
                        
                        return Ok();

                    }else{
                         Response.StatusCode = 404;
                         return new ObjectResult (new{msg="Id inválido"}); 

                    }


                }catch{
                   Response.StatusCode = 404;
                return new ObjectResult (new{msg="Id inválido"}); 
                }

            }else{
                 Response.StatusCode = 404;
                return new ObjectResult (new{msg="Id inválido"});
            }

        }

         [HttpDelete("{id}")]
        public IActionResult Delete(int id){

               try{
                var genero = database.Generos.First(f=> f.Id == id);
                database.Generos.Remove(genero);
                database.SaveChanges();
               
            return Ok("Gênero excluído com sucesso"); 
            }catch(Exception ){  

            Response.StatusCode = 404;          
            return new ObjectResult (new{msg= "Id inválido"}); }


        }

           public class GeneroContainer{
            public Genero genero {get; set;}

            public Link[] links {get; set;}
        }
        
    }
}