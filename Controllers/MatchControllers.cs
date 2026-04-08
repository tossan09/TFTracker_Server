using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TFTDataTrackerApi.Models;
using TFTDataTrackerApi.Repository;

namespace TFTDataTrackerApi.Controllers
{
    [ApiController]
    [Route("matches")]
    public class MatchControllers(MatchRepository matchRepository, ILogger<MatchControllers> logger) : ControllerBase
    {
        private readonly MatchRepository _matchRepository = matchRepository;
        private readonly ILogger<MatchControllers> _logger = logger;

        [HttpGet("patch/{patchnumber}")]
        public async Task<IActionResult> GetByPatch(string patchnumber)
        {
            var partidas = await _matchRepository.ListarPartidasPorPatch(patchnumber);
            return Ok(partidas);
        }

        [HttpGet("comp/{compname}")]
        public async Task<IActionResult> GetMatchesByComp(string compname)
        {
            var partidas = await _matchRepository.ListarMatchesPorComp(compname);
            return Ok(partidas);
        }

        [HttpGet("comp/{compname}/patch/{patchnumber}")]
        public async Task<IActionResult> GetById(string compname, string patchnumber)
        {
            var partidas = await _matchRepository.ListarCompIDPorPatchID(compname, patchnumber);
            return Ok(partidas);

        }
        //componente recent placement
        [HttpGet("placement/{patchnumber}")]
        public async Task<IActionResult> GetPlacement(string patchnumber)
        {
            var placement = await _matchRepository.ListarPlacementRecent(patchnumber);
            if (placement == null || placement.Count == 0)
                return Ok(new List<int>());
            return Ok(placement);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] Matches matches)
        {
            var username = User.Identity?.Name ?? "unknown";
            var roles = string.Join(",", User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value));

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("User {User} with roles {Roles} tried to add invalid match data", username, roles);
                return BadRequest(ModelState);
            }
            try
            {
                await _matchRepository.AddPartida(matches);
                _logger.LogInformation("User {User} with roles {Roles} added match", username, roles);
                return Ok(matches);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error adding match by user {User} with roles {Roles}", username, roles);
                return StatusCode(500, new { message = "Erro interno", detail = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var username = User.Identity?.Name ?? "unknown";
            var roles = string.Join(",", User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value));

            var exclude = await _matchRepository.DeleteMatch(id);
            if (!exclude)
            {
                _logger.LogWarning("User {User} with roles {Roles} tried to delete non-existing match {Id}", username, roles, id);
                return NotFound("Partida nao encontrada!");
            }

            _logger.LogInformation("User {User} with roles {Roles} deleted match {Id}", username, roles, id);
            return Ok("Partida excluida!");
        }
    }
}
