using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TFTDataTrackerApi.Models;
using TFTDataTrackerApi.Repository;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet("set/{setId}")]
        public async Task<IActionResult> GetPatchesBySet(int setId)
        {
            var patches = await repository.ListarPatchesPorSet(setId);
            return Ok(patches);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Patches patches)
        {
            var ok = await repository.AdicionarPatch(patches);
            if (!ok) return BadRequest("Erro ao criar patch");
            return Ok(new { message = "Patch criado" });
        }

    }
}
