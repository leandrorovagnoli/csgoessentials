using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Interfaces.Services;
using CsgoEssentials.Infra.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks; 

namespace CsgoEssentials.API.Controllers
{
    [Route("v1/maps")]
    [Authorize(Roles = "Administrator")]
    public class MapController : Controller
    {
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Map>>> GetAll([FromServices] IMapService MapService)
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
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<Map>>> GetById(int id, [FromServices] IMapService mapService)
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

        [HttpGet]
        [Route("{id:int}/include")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Map>> GetByIdWithVideos(int id, [FromServices] IMapService mapService)
        {
            try
            {
                var map = await mapService.GetByIdAsNoTrackingWithRelationship(id);
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
        [Authorize(Roles = "Administrator")]
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_CRIAR_O_MAPA });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Map>> Put(
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
                await MapService.Update(map);
                return map;
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_ATUALIZAR_O_MAPA });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Map>> Delete(
            int id,
            [FromServices] IMapService MapService)
        {
            try
            {
                var map = await MapService.GetById(id);

                if (map == null)
                    return NotFound(new { message = Messages.MAPA_NAO_ENCONTRADO });

                await MapService.Delete(map);

                return Ok(new { message = Messages.MAPA_REMOVIDO_COM_SUCESSO });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return BadRequest(new { message = Messages.OCORREU_UM_ERRO_INESPERADO });
            }
        }

    }
}
