using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Services;
using CsgoEssentials.Domain.Services;
using CsgoEssentials.Infra.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace CsgoEssentials.API.Controllers
{
    [Route("v1/users")]
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll([FromServices] IUserService userService)
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
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<User>>> GetById(int id, [FromServices] IUserService userService)
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

        [HttpGet]
        [Route("{id:int}/articles")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<User>> GetByIdWithArticles(int id, [FromServices] IUserService userService)
        {
            try
            {
                var user = await userService.GetByIdAsNoTrackingWithArticles(id);
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
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<User>> Post(
            [FromServices] IUserService userService,
            [FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await userService.Add(user);

                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_CRIAR_O_USUARIO });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<User>> Put(
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
                await userService.Update(user);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_ATUALIZAR_O_USUARIO });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<User>> Delete(
            int id,
            [FromServices] IUserService userService)
        {
            try
            {
                var user = await userService.GetById(id);

                if (user == null)
                    return NotFound(new { message = Messages.USUARIO_NAO_ENCONTRADO });

                await userService.Delete(user);

                return Ok(new { message = Messages.USUARIO_REMOVIDO_COM_SUCESSO });
            }
            catch
            {
                return BadRequest(new { message = Messages.OCORREU_UM_ERRO_INESPERADO });
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromServices] IUserService userService,
            [FromBody] User requestUser)
        {
            try
            {
                var users = await userService.Find(x => x.UserName == requestUser.UserName && x.Password == MD5Hash.CalculaHash(requestUser.Password));

                if (users == null || users.Count() == 0)
                    return NotFound(new { message = Messages.USUARIO_OU_SENHA_INVALIDOS });

                var user = users.FirstOrDefault();

                var token = TokenService.GenerateToken(user);

                return new
                {
                    user = new
                    {
                        id = user.Id,
                        username = user.UserName,
                        role = user.Role.ToString()
                    },

                    token = token
                };
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_AUTENTICAR_O_USUARIO });
            }
        }
    }
}