using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Services;
using CsgoEssentials.Infra.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CsgoEssentials.API.Controllers
{
    [Route("v1/videos")]
    public class VideoController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetAll([FromServices] IVideoService VideoService)
        {
            try
            {
                var videos = await VideoService.GetAllAsNoTracking();
                return Ok(videos);
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_BUSCAR_OS_VIDEOS });
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<IEnumerable<Video>>> GetById(int id, [FromServices] IVideoService videoService)
        {
            try
            {
                var video = await videoService.GetByIdAsNoTracking(id);
                if (video == null)
                    return BadRequest(new { message = Messages.VIDEO_NAO_ENCONTRADO });

                return Ok(video);
            }
            catch
            {
                return BadRequest(new { message = Messages.OCORREU_UM_ERRO_INESPERADO });
            }
        }


        [HttpPost]
        public async Task<ActionResult<Video>> Post(
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
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_CRIAR_O_VIDEO });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public ActionResult<Video> Put(
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
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_ATUALIZAR_O_VIDEO });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Video>> Delete(
            int id,
            [FromServices] IVideoService VideoService)
        {
            try
            {
                var video = await VideoService.GetById(id);

                if (video == null)
                    return NotFound(new { message = Messages.VIDEO_NAO_ENCONTRADO });

                VideoService.Delete(video);

                return Ok(new { message = Messages.VIDEO_REMOVIDO_COM_SUCESSO });
            }
            catch
            {
                return BadRequest(new { message = Messages.OCORREU_UM_ERRO_INESPERADO });
            }
        }

    }
}
