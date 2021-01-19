using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Services;
using CsgoEssentials.Infra.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CsgoEssentials.API.Controllers
{
    [Route("v1/users")]
    public class UserController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get([FromServices] IUserService userService)
        {
            try
            {
                var users = await userService.GetAllAsNoTracking();
                return Ok(users);
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_BUSCAR_OS_USUARIOS });
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<IEnumerable<User>>> Get(int id, [FromServices] IUserService userService)
        {
            try
            {
                var user = await userService.GetByIdAsNoTracking(id);
                if (user == null)
                    return BadRequest(new { message = Messages.USUARIO_NAO_ENCONTRADO });

                return Ok(user);
            }
            catch
            {
                return BadRequest(new { message = Messages.OCORREU_UM_ERRO_INESPERADO });
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(
            [FromServices] IUserService userService,
            [FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Criptografando a senha.
                user.Password = MD5Hash.CalculaHash(user.Password);

                await userService.Add(user);

                return Ok(user);
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_CRIAR_O_USUARIO });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public ActionResult<User> Put(
            int id,
            [FromServices] IUserService userService,
            [FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != user.Id)
                return NotFound(new { message = Messages.USUARIO_NAO_ENCONTRADO });

            try
            {
                // Criptografando a senha.
                user.Password = MD5Hash.CalculaHash(user.Password);

                userService.Update(user);
                return Ok(user);
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_ATUALIZAR_O_USUARIO });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<User>> Delete(
            int id,
            [FromServices] IUserService userService)
        {
            try
            {
                var user = await userService.GetById(id);

                if (user == null)
                    return NotFound(new { message = Messages.USUARIO_NAO_ENCONTRADO });

                userService.Delete(user);

                return Ok(new { message = Messages.USUARIO_REMOVIDO_COM_SUCESSO });
            }
            catch
            {
                return BadRequest(new { message = Messages.OCORREU_UM_ERRO_INESPERADO });
            }
        }
    }
}
