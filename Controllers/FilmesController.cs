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
    public class FilmesController : ControllerBase
    {
        private readonly ApplicationDbContext database;
        private HATEOAS.HATEOAS HATEOAS;

        public FilmesController(ApplicationDbContext database){
            this.database = database;
            HATEOAS = new HATEOAS.HATEOAS ("localhost:5001/api/v1/Filmes");
            HATEOAS.AddAction("GET_INFO", "GET");
            HATEOAS.AddAction("DELETE_PRODUCT", "DELETE");
            HATEOAS.AddAction("EDIT_PRODUCT", "PATCH");
        }


        [HttpGet]   
        public IActionResult ListaFilmes (){
            var filmes = database.Filmes.Include(f => f.AtoresFilmes).Include(g=> g.FilmesGeneros).ToList();

             List<FilmesContainer> filmesHATEOAS = new List<FilmesContainer>();
            foreach(var filme in filmes){
                FilmesContainer filmeHATEOAS = new FilmesContainer();
                filmeHATEOAS.filmes = filme;
                filmeHATEOAS.links = HATEOAS.GetActions(filme.Id.ToString());
                filmesHATEOAS.Add(filmeHATEOAS);

            }
           return Ok(new{filmesHATEOAS});            
        }            
        

        [HttpGet("{id}")]
        public IActionResult Get(int id){

            try{
                var filmes = database.Filmes.Include(f => f.AtoresFilmes).Include(g=> g.FilmesGeneros).First(f=> f.Id == id);
                FilmesContainer filmeHATEOAS = new FilmesContainer();
                filmeHATEOAS.filmes = filmes;
                filmeHATEOAS.links = HATEOAS.GetActions(filmes.Id.ToString());
            return Ok(filmeHATEOAS); 
 
            }catch(Exception ){  

            Response.StatusCode = 404;          
            return new ObjectResult (new{msg= "Id inválido"}); }

        }

        [HttpPost]
        public IActionResult Post([FromBody] FilmeTemp fTemp){
            Filmes filmes = new Filmes();
            if(fTemp.Nome.Length <=1){
                Response.StatusCode = 400;
                return new ObjectResult (new{msg="O filme tem que ter um nome válido!"});
            }
            if(fTemp.Idioma.Length <=1){
               Response.StatusCode = 400;
                return new ObjectResult (new{msg="O filme tem que ter um idioma!"}); 
            }
               if(fTemp.Duracao <=1){
               Response.StatusCode = 400;
                return new ObjectResult (new{msg="O filme tem que ter uma duração em minutos!"}); 
            }
               if(fTemp.DataLancamento.Length <=1){
               Response.StatusCode = 400;
                return new ObjectResult (new{msg="O filme tem que ter uma data de Lançamento (ano)!"}); 
            }
                if(fTemp.AtoresFilmesId == null){
               Response.StatusCode = 400;
                return new ObjectResult (new{msg="O filme tem que ter pelo menos um ator principal cadastrado!"}); 
            }
               if(fTemp.FilmesGenerosId == null){
               Response.StatusCode = 400;
                return new ObjectResult (new{msg="O filme tem que ter pelo menos um gênero cadastrado!"}); 
            }
            filmes.Nome = fTemp.Nome;
            filmes.Duracao = fTemp.Duracao;
            filmes.Idioma = fTemp.Idioma;
            filmes.DataLancamento = fTemp.DataLancamento;
            filmes.Disponivel = true;
            
           
            
            database.Filmes.Add(filmes);
            database.SaveChanges();
            var filmeId = database.Filmes.Where(f=> f.Nome == filmes.Nome).First(a=> a.Id == filmes.Id);

             foreach (var atorFilmeId in fTemp.AtoresFilmesId){
                       
                        AtoresFilmes atoresFilmes1 = new AtoresFilmes();
                        atoresFilmes1.AtoresId = atorFilmeId;
                        atoresFilmes1.FilmesId = filmeId.Id;

                        database.AtoresFilmes.Add(atoresFilmes1);                        
                        database.SaveChanges();
                    };

            
             foreach (var filmeGeneroId in fTemp.FilmesGenerosId){
                       
                        FilmesGeneros generoFilmes1 = new FilmesGeneros();
                        generoFilmes1.GeneroId = filmeGeneroId;
                        generoFilmes1.FilmesId = filmeId.Id;

                        database.FilmesGeneros.Add(generoFilmes1);                        
                        database.SaveChanges();
                    };



            Response.StatusCode = 201;
            return new ObjectResult (new{msg = "Filme incluído com sucesso!" });
        }

         [HttpPatch]
        public IActionResult Editar ([FromBody] FilmeTemp filme){
            if(filme.Id > 0){
                try{
                    var f = database.Filmes.Include(g=> g.FilmesGeneros).Include(a=> a.AtoresFilmes).First(ftemp=> ftemp.Id == filme.Id);
                    

                    if(f != null){

                        f.Nome = filme.Nome != null ? filme.Nome : f.Nome;
                        f.Duracao = filme.Duracao != 0 ? filme.Duracao : f.Duracao;
                        f.Idioma = filme.Idioma != null ? filme.Idioma : f.Idioma;
                        f.DataLancamento = filme.DataLancamento != null ? filme.DataLancamento : f.DataLancamento;


                        if(filme.Nome.Length <= 1){

                        Response.StatusCode = 400;
                        return new ObjectResult (new{msg="Nome inválido ou vazio, tente outro nome!"}); 
                            
                        
                        

                        }else{
                            database.SaveChanges();
                        
                        }
                        if(filme.Duracao <= 0){

                        Response.StatusCode = 400;
                        return new ObjectResult (new{msg="Duração inválida ou vazio!"}); 
                            
                        
                        

                        }else{
                            database.SaveChanges();
                        
                        }
                         if(filme.Idioma.Length <= 1){

                        Response.StatusCode = 400;
                        return new ObjectResult (new{msg="Idioma inválido ou vazio!"}); 
                            
                        
                        

                        }else{
                            database.SaveChanges();
                        
                        }
                         if(filme.DataLancamento.Length < 4){

                        Response.StatusCode = 400;
                        return new ObjectResult (new{msg="Data de lançamento inválido ou vazio, coloque um ano válido! Exemplo: 1980"}); 
                            
                        
                        

                        }else{
                            database.SaveChanges();
                        
                        }
                    if(f.AtoresFilmes != null){
                        var atores = database.AtoresFilmes.Where(f=> f.FilmesId == filme.Id);
                        database.AtoresFilmes.RemoveRange(atores);
                        database.SaveChanges();
                        
                            var AtoresFilmesTemp = database.AtoresFilmes.ToList();
                            foreach (var atorFilmeId in filme.AtoresFilmesId){
                       
                            AtoresFilmes atoresFilmes1 = new AtoresFilmes();
                            atoresFilmes1.FilmesId = f.Id;
                            atoresFilmes1.AtoresId = atorFilmeId;

                            database.AtoresFilmes.Add(atoresFilmes1);                        
                            database.SaveChanges();
                            }; }  
                    if(f.FilmesGeneros != null){
                        var generos = database.FilmesGeneros.Where(f=> f.FilmesId == filme.Id);
                        database.FilmesGeneros.RemoveRange(generos);
                        database.SaveChanges();
                        
                            var GeneroFilmesTemp = database.FilmesGeneros.ToList();
                            foreach (var generoFilmeId in filme.FilmesGenerosId){
                       
                            FilmesGeneros generoFilmes1 = new FilmesGeneros();
                            generoFilmes1.FilmesId = f.Id;
                            generoFilmes1.GeneroId = generoFilmeId;

                            database.FilmesGeneros.Add(generoFilmes1);                        
                            database.SaveChanges();
                            }; }                      
                        
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
                var filmes = database.Filmes.First(f=> f.Id == id);
                database.Filmes.Remove(filmes);
                database.SaveChanges();
               
            return Ok("Filme excluído com sucesso"); 
            }catch(Exception ){  

            Response.StatusCode = 404;          
            return new ObjectResult (new{msg= "Id inválido"}); }


        }

           public class FilmesContainer{
            public Filmes filmes {get; set;}

            public Link[] links {get; set;}
        }


        
    }
}