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
    [Route(ApiRoutes.Maps.Route)]
    [Authorize(Roles = "Administrator")]
    public class MapController : Controller
    {
        [HttpGet]
        [Route(ApiRoutes.Maps.GetAll)]
        public async Task<ActionResult<IEnumerable<Map>>> GetAll([FromServices] IMapService MapService)
        {
            try
            {
                var maps = await MapService.GetAll();
                return Ok(maps);
            }
            catch
            {
                return BadRequest(new { message = Messages.NAO_FOI_POSSIVEL_BUSCAR_OS_MAPAS });
            }
        }

        [HttpGet]
        [Route(ApiRoutes.Maps.GetById)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Map>> GetById(int id, [FromServices] IMapService mapService)
        {
            try
            {
                var map = await mapService.GetById(id);
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
        [Route(ApiRoutes.Maps.GetByIdWithRelationship)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Map>> GetByIdWithRelationship(int id, [FromServices] IMapService mapService)
        {
            try
            {
                var map = await mapService.GetByIdWithRelationship(id);
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
        [Route(ApiRoutes.Maps.Create)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Map>> Create(
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
        [Route(ApiRoutes.Maps.Update)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Map>> Update(
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
        [Route(ApiRoutes.Maps.Delete)]
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
