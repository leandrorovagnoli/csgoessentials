using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Services;
using CsgoEssentials.Infra.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsgoEssentials.API.Controllers
{
    [Route("v1/article")]
    public class ArticleController : Controller
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> Get([FromServices] IUserService userService)
        {
            try
            {
                var artigo = await userService.GetAllAsNoTracking();
                return Ok(artigo);
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_BUSCAR_OS_USUARIOS });
            }
        }






    }
}
