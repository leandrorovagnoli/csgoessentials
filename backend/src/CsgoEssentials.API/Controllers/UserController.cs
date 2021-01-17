using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Infra.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CsgoEssentials.API.Controllers
{
    [Route("v1/users")]
    public class UserController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
        {
            try
            {
                var users = await context
                    .Users
                    .AsNoTracking()
                    .ToListAsync();

                return users;
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível buscar os usuários." });
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(
            [FromServices] DataContext context,
            [FromBody] User model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Criptografando a senha.
                model.Password = MD5Hash.CalculaHash(model.Password);

                context.Users.Add(model);
                await context.SaveChangesAsync();

                // Esconde a senha ao retornar para a tela.
                model.Password = "";
                return model;
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível criar o usuário." });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<User>> Put(
            int id,
            [FromServices] DataContext context,
            [FromBody] User model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != model.Id)
                return NotFound(new { message = "Usuário não encontrado" });

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível atualizar o usuário." });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<User>> Delete(
            int id,
            [FromServices] DataContext context)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));

                if (user == null)
                    return NotFound(new { message = "Menu não encontrado." });

                context.Users.Remove(user);
                await context.SaveChangesAsync();

                return Ok(user);
            }
            catch
            {
                return BadRequest(new { message = "Ocorreu um erro." });
            }
        }
    }
}
