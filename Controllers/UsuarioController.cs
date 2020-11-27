using Microsoft.AspNetCore.Mvc;
using TreinoApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using TreinoApi.Models;
using TreinoApi.Classes;
using Microsoft.EntityFrameworkCore;
using TreinoApi.HATEOAS;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace TreinoApi.Controllers 
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {


        private readonly ApplicationDbContext database;
       
        public UsuarioController(ApplicationDbContext database){
            this.database = database;       
        }

         [HttpPost("registro")]
        public IActionResult Registro([FromBody] Usuario usuario){
            if(usuario.Email.Length <= 6){
                
            Response.StatusCode = 400;
            return new ObjectResult (new{msg = "Email inválido!" });

            }
            if(usuario.Senha.Length <= 6){
                
            Response.StatusCode = 400;
            return new ObjectResult (new{msg = "Senha muito curta!" });

            }
            database.Usuarios.Add(usuario);
            database.SaveChanges();
            
            Response.StatusCode = 201;
            return new ObjectResult (new{msg = "Usuário criado com sucesso!" });
        }

    [HttpPost("Login")] 
      public IActionResult Login ([FromBody]Usuario credenciais){
        
        try{
        Usuario usuario = database.Usuarios.First(User=> User.Email.Equals(credenciais.Email));



        if(usuario != null){
          if(usuario.Senha.Equals(credenciais.Senha)){
            string chaveSeguranca = "treinoapi_chave_seguranca_estudos_gft";
            var chaveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSeguranca));
            var credenciaisDeAcesso = new SigningCredentials (chaveSimetrica, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>();
            claims.Add(new Claim("id", usuario.Id.ToString()));
            claims.Add(new Claim("email", usuario.Email));
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            var JWT = new JwtSecurityToken(

              issuer: "TreinoApi",
              expires: DateTime.Now.AddHours(1),
              audience: "usuario_comum",
              signingCredentials: credenciaisDeAcesso,
              claims: claims
            );

            return Ok (new JwtSecurityTokenHandler().WriteToken(JWT));

          } else{
            Response.StatusCode=401;
          return new ObjectResult("Usuário ou senha inválida");
          }

         } else{
          Response.StatusCode=401;
          return new ObjectResult("Usuário ou senha inválida");
         }
         }catch(Exception e){
            Response.StatusCode=401;
          return new ObjectResult("Usuário ou senha inválida");
         }

      }
    }


        
}
