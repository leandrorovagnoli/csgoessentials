using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Services;
using CsgoEssentials.Infra.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CsgoEssentials.API.Controllers
{
    [Route("v1/articles")]
    public class ArticleController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetAll([FromServices] IArticleService articleService)
        {
            try
            {
                var articles = await articleService.GetAllAsNoTracking();
                return Ok(articles);
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_BUSCAR_OS_ARTIGOS });
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetById(int id, [FromServices] IArticleService articleService)
        {
            try
            {
                var article = await articleService.GetByIdAsNoTracking(id);
                if (article == null)
                    return BadRequest(new { message = Messages.ARTIGO_NAO_ENCONTRADO });

                return Ok(article);
            }
            catch
            {
                return BadRequest(new { message = Messages.OCORREU_UM_ERRO_INESPERADO });
            }
        }


        [HttpPost]
        public async Task<ActionResult<Article>> Post(
            [FromServices] IArticleService articleService,
            [FromBody] Article article)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await articleService.Add(article);

                return Ok(article);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_CRIAR_UM_ARTIGO });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Article>> Put(
            int id,
            [FromServices] IArticleService articleService,
            [FromBody] Article article)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != article.Id)
                return NotFound(new { Messages.MAPA_NAO_ENCONTRADO });

            try
            {
                await articleService.Update(article);
                return article;
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_ATUALIZAR_O_ARTIGO });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Article>> Delete(
            int id,
            [FromServices] IArticleService articleService)
        {
            try
            {
                var article = await articleService.GetById(id);

                if (article == null)
                    return NotFound(new { message = Messages.ARTIGO_NAO_ENCONTRADO });

                await articleService.Delete(article);

                return Ok(new { message = Messages.ARTIGO_REMOVIDO_COM_SUCESSO });
            }
            catch
            {
                return BadRequest(new { message = Messages.OCORREU_UM_ERRO_INESPERADO });
            }
        }

    }

}
