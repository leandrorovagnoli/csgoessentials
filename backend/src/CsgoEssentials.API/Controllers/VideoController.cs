using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Services;
using CsgoEssentials.Domain.Utils;
using CsgoEssentials.IntegrationTests.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsgoEssentials.API.Controllers
{
    [Route(ApiRoutes.Videos.Route)]
    public class VideoController : Controller
    {
        [HttpGet]
        [Route(ApiRoutes.Videos.GetAll)]
        public async Task<ActionResult<IEnumerable<Video>>> GetAll([FromServices] IVideoService VideoService)
        {
            try
            {
                var videos = await VideoService.GetAll();
                return Ok(videos);
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_BUSCAR_OS_VIDEOS });
            }
        }

        [HttpGet]
        [Route(ApiRoutes.Videos.GetById)]
        public async Task<ActionResult<Video>> GetById(int id, [FromServices] IVideoService videoService)
        {
            try
            {
                var video = await videoService.GetById(id);
                if (video == null)
                    return BadRequest(new { message = Messages.VIDEO_NAO_ENCONTRADO });

                return Ok(video);
            }
            catch
            {
                return BadRequest(new { message = Messages.OCORREU_UM_ERRO_INESPERADO });
            }
        }

        [HttpGet]
        [Route(ApiRoutes.Videos.GetByIdWithRelationship)]
        public async Task<ActionResult<Article>> GetByIdWithRelationship(int id, [FromServices] IVideoService videoService)
        {
            try
            {
                var video = await videoService.GetByIdWithRelationship(id);
                if (video == null)
                    return BadRequest(new { message = Messages.VIDEO_NAO_ENCONTRADO });

                return Ok(video);
            }
            catch
            {
                return BadRequest(new { message = Messages.OCORREU_UM_ERRO_INESPERADO });
            }
        }

        [HttpGet]
        [Route(ApiRoutes.Videos.Filter)]
        public async Task<ActionResult<IEnumerable<Video>>> Filter(
            [FromQuery] Query query,
            [FromServices] IVideoService videoService)
        {
            try
            {
                var videos = await videoService.Filter(query);

                if (!videos.Any())
                    return BadRequest(new { message = Messages.NENHUM_VIDEO_ENCONTRADO });

                return Ok(videos);
            }
            catch
            {
                return BadRequest(new { message = Messages.OCORREU_UM_ERRO_INESPERADO });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [Route(ApiRoutes.Videos.Create)]
        public async Task<ActionResult<Video>> Create(
            [FromServices] IVideoService VideoService,
            [FromBody] Video video)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await VideoService.Add(video);

                return video;
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_CRIAR_O_VIDEO });
            }
        }

        [HttpPut]
        [Route(ApiRoutes.Videos.Update)]
        [Authorize(Roles = "Administrator")]
        public ActionResult<Video> Update(
            int id,
            [FromServices] IVideoService VideoService,
            [FromBody] Video video)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != video.Id)
                return NotFound(new { Messages.VIDEO_NAO_ENCONTRADO });

            try
            {
                VideoService.Update(video);
                return video;
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_ATUALIZAR_O_VIDEO });
            }
        }

        [HttpDelete]
        [Route(ApiRoutes.Videos.Delete)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Video>> Delete(
            int id,
            [FromServices] IVideoService VideoService)
        {
            try
            {
                var video = await VideoService.GetById(id);

                if (video == null)
                    return NotFound(new { message = Messages.VIDEO_NAO_ENCONTRADO });

                await VideoService.Delete(video);

                return Ok(new { message = Messages.VIDEO_REMOVIDO_COM_SUCESSO });
            }
            catch
            {
                return BadRequest(new { message = Messages.OCORREU_UM_ERRO_INESPERADO });
            }
        }
    }
}
