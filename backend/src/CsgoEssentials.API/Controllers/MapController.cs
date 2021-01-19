using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Services;
using CsgoEssentials.Infra.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CsgoEssentials.API.Controllers
{
    [Route("v1/maps")]
    public class MapController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Map>>> Get([FromServices] IMapService MapService)
        {
            try
            {
                var maps = await MapService.GetAllAsNoTracking();
                return Ok(maps);
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_BUSCAR_OS_MAPAS });
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<IEnumerable<Map>>> Get(int id, [FromServices] IMapService mapService)
        {
            try
            {
                var map = await mapService.GetByIdAsNoTracking(id);
                if (map == null)
                    return BadRequest(new { message = Messages.MAPA_NAO_ENCONTRADO });

                return Ok(map);
            }
            catch
            {
                return BadRequest(new { message = Messages.OCORREU_UM_ERRO_INESPERADO });
            }
        }


        [HttpPost]
        public async Task<ActionResult<Map>> Post(
            [FromServices] IMapService MapService,
            [FromBody] Map map)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await MapService.Add(map);

                return map;
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_CRIAR_O_MAPA });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public ActionResult<Map> Put(
            int id,
            [FromServices] IMapService MapService,
            [FromBody] Map map)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != map.Id)
                return NotFound(new { Messages.MAPA_NAO_ENCONTRADO });

            try
            {
                MapService.Update(map);
                return map;
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_ATUALIZAR_O_MAPA });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Map>> Delete(
            int id,
            [FromServices] IMapService MapService)
        {
            try
            {
                var map = await MapService.GetById(id);

                if (map == null)
                    return NotFound(new { message = Messages.MAPA_NAO_ENCONTRADO });

                MapService.Delete(map);

                return Ok(new { message = Messages.MAPA_REMOVIDO_COM_SUCESSO });
            }
            catch
            {
                return BadRequest(new { message = Messages.OCORREU_UM_ERRO_INESPERADO });
            }
        }

    }
}
