using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TFTDataTrackerApi.Models;
using TFTDataTrackerApi.Repository;
using Microsoft.AspNetCore.Authorization;

namespace TFTDataTrackerApi.Controllers
{
    [ApiController]
    [Route("api/set")]
    public class SetControllers(SetRepository setRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sets = await setRepository.ListarSets();
            return Ok(sets);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Sets sets)
        {
            var ok = await setRepository.AdicionarSet(sets);
            if (!ok) return BadRequest("Erro ao criar set");
            return Ok(sets);

        }
    }
}
