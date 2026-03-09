using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TFTDataTrackerApi.Models;
using TFTDataTrackerApi.Repository;

namespace TFTDataTrackerApi.Controllers
{
    [ApiController]
    [Route("api/patches")]

    public class PatchControllers : ControllerBase
    {
        private readonly PatchRepository repository;

        public PatchControllers(PatchRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatches()
        {
            var patches = await repository.ListarPatches();
            return Ok(patches);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Patches patches)
        {
            var ok = await repository.AdicionarPatch(patches);
            if (!ok) return BadRequest("Erro ao criar patch");
            return Ok("Patch criado");
        }

    }
}
