using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Infra.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CsgoEssentials.API.Controllers
{
    [Route("v1/maps")]
    public class MapController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<List<Map>>> Get([FromServices] DataContext context)
        {
            try
            {
                var maps = await context
                    .Maps
                    .AsNoTracking()
                    .ToListAsync();

                return maps;
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível buscar os mapas." });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Map>> Post(
            [FromServices] DataContext context,
            [FromBody] Map model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Maps.Add(model);
                await context.SaveChangesAsync();

                return model;
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível adicionar o mapa." });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Map>> Put(
            int id,
            [FromServices] DataContext context,
            [FromBody] Map model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != model.Id)
                return NotFound(new { message = "Mapa não encontrado" });

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível atualizar o Mapa." });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Map>> Delete(
            int id,
            [FromServices] DataContext context)
        {
            try
            {
                var Map = await context.Maps.FirstOrDefaultAsync(x => x.Id.Equals(id));

                if (Map == null)
                    return NotFound(new { message = "Mapa não encontrado." });

                context.Maps.Remove(Map);
                await context.SaveChangesAsync();

                return Ok(Map);
            }
            catch
            {
                return BadRequest(new { message = "Ocorreu um erro." });
            }
        }

    }
}
