using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Services;
using CsgoEssentials.Infra.Utils;
using CsgoEssentials.IntegrationTests.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CsgoEssentials.API.Controllers
{
    [Route(ApiRoutes.Articles.Route)]
    public class ArticleController : Controller
    {
        [HttpGet]
        [Route(ApiRoutes.Articles.GetAll)]
        public async Task<ActionResult<IEnumerable<Article>>> GetAll([FromServices] IArticleService articleService)
        {
            try
            {
                var articles = await articleService.GetAll();
                return Ok(articles);
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_BUSCAR_OS_ARTIGOS });
            }
        }

        [HttpGet]
        [Route(ApiRoutes.Articles.GetById)]
        public async Task<ActionResult<Article>> GetById(int id, [FromServices] IArticleService articleService)
        {
            try
            {
                var article = await articleService.GetById(id);
                if (article == null)
                    return BadRequest(new { message = Messages.ARTIGO_NAO_ENCONTRADO });

                return Ok(article);
            }
            catch
            {
                return BadRequest(new { message = Messages.OCORREU_UM_ERRO_INESPERADO });
            }
        }

        [HttpGet]
        [Route(ApiRoutes.Articles.GetByIdWithRelationship)]
        public async Task<ActionResult<Article>> GetByIdWithRelationship(int id, [FromServices] IArticleService articleService)
        {
            try
            {
                var article = await articleService.GetByIdWithRelationship(id);
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
        [Authorize(Roles = "Administrator,Editor")]
        [Route(ApiRoutes.Articles.Create)]
        public async Task<ActionResult<Article>> Create(
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_CRIAR_UM_ARTIGO });
            }
        }

        [HttpPut]
        [Route(ApiRoutes.Articles.Update)]
        [Authorize(Roles = "Administrator,Editor")]
        public async Task<ActionResult<Article>> Update(
            int id,
            [FromServices] IArticleService articleService,
            [FromBody] Article article)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != article.Id)
                return NotFound(new { Messages.ARTIGO_NAO_ENCONTRADO });

            try
            {
                await articleService.Update(article);
                return article;
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_ATUALIZAR_O_ARTIGO });
            }
        }

        [HttpDelete]
        [Route(ApiRoutes.Articles.Delete)]
        [Authorize(Roles = "Administrator,Editor")]
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
