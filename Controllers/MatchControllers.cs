using Microsoft.AspNetCore.Mvc;
using TFTDataTrackerApi.Models;
using TFTDataTrackerApi.Repository;
using Microsoft.AspNetCore.Authorization;

namespace TFTDataTrackerApi.Controllers
{
    [ApiController]
    [Route("matches")]
    public class MatchControllers(MatchRepository matchRepository) : ControllerBase
    {
        private readonly MatchRepository _matchRepository = matchRepository;

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
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _matchRepository.AddPartida(matches);
                return Ok(matches);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno", detail = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var exclude = await _matchRepository.DeleteMatch(id);
            if (!exclude)
                return NotFound("Partida nao encontrada!");
            return Ok("Partida excluida!");
        }
    }
}
