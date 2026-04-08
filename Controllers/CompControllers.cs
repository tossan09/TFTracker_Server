using Microsoft.AspNetCore.Mvc;
using TFTDataTrackerApi.Models;
using TFTDataTrackerApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace TFTDataTrackerApi.Controllers
{
    [ApiController]
    [Route("/comps")]
    public class CompControllers : ControllerBase
    {
        private readonly CompRepository _compRepository;
        private readonly ILogger<CompControllers> _logger;

        public CompControllers(CompRepository compRepository, ILogger<CompControllers> logger)
        {
            _compRepository = compRepository;
            _logger = logger;
        }

        [HttpGet("set/{setNumber}")]
        public async Task<IActionResult> GetCompsBySet(int setNumber)
        {
            var comps = await _compRepository.ListarCompsPorSet(setNumber);
            return Ok(comps);
        }

        [HttpGet("byPatch/{setid}")]
        public async Task<IActionResult> GetCompsByPatch(int setid)
        {
            var comps = await _compRepository.ListarCompsPorPatch(setid);
            return Ok(comps);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] Comps comps)
        {
            var username = User.Identity?.Name ?? "unknown";
            var roles = string.Join(",", User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value));

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("User {User} with roles {Roles} tried to add an invalid comp", username, roles);
                return BadRequest(ModelState);
            }

            try
            {
                await _compRepository.AddComp(comps);
                _logger.LogInformation("User {User} with roles {Roles} added comp {Name}", username, roles, comps.name);
                return Ok(comps);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comp {Name} by user {User} with roles {Roles}", comps.name, username, roles);
                return StatusCode(500, new { message = "Erro interno", detail = ex.Message });
            }
        }

        //[HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Update(int id, [FromBody] Comps comp)
        //{
        //    var edit = await _compRepository.EditComp(id, comp);
        //    if (!edit)
        //        return NotFound("Comp não encontrada");

        //    return Ok("Atualizado!");
        //}

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var username = User.Identity?.Name ?? "unknown";
            var roles = string.Join(",", User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value));

            var excluir = await _compRepository.DeleteComp(id);
            if (!excluir)
            {
                _logger.LogWarning("User {User} with roles {Roles} tried to delete non-existing comp {Id}", username, roles, id);
                return NotFound("Comp not found");
            }

            _logger.LogInformation("User {User} with roles {Roles} deleted comp {Id}", username, roles, id);
            return Ok("Comp removed");
        }
    }
}
