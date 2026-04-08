using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TFTDataTrackerApi.Models;
using TFTDataTrackerApi.Repository;

namespace TFTDataTrackerApi.Controllers
{
    [ApiController]
    [Route("api/patches")]

    public class PatchControllers : ControllerBase
    {
        private readonly PatchRepository repository;
        private readonly ILogger<PatchControllers> logger;

        public PatchControllers(PatchRepository repository, ILogger<PatchControllers> logger)
        {
            this.repository = repository;
            this.logger = logger;
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
            var username = User.Identity?.Name ?? "unknown";
            var roles = string.Join(",", User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value));

            var ok = await repository.AdicionarPatch(patches);
            if (!ok)
            {
                logger.LogWarning("User {User} with roles {Roles} failed to create patch ", username, roles);
                return BadRequest("Erro ao criar patch");
            }

            logger.LogInformation("User {User} with roles {Roles} created patch", username, roles);
            return Ok(new { message = "Patch criado" });
        }

    }
}
