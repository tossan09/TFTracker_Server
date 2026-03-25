using Microsoft.AspNetCore.Mvc;
using TFTDataTrackerApi.Models;
using TFTDataTrackerApi.Repository;

namespace TFTDataTrackerApi.Controllers
{
    [ApiController]
    [Route("matches")]
    public class MatchControllers : ControllerBase
    {
        private readonly MatchRepository _matchRepository;
        public MatchControllers(MatchRepository matchRepository)
        {
            this._matchRepository = matchRepository;
        }

        [HttpGet("patch/{patchnumber}")]
        public async Task<IActionResult> GetByPatch(string patchnumber)
        {
            var partidas = await _matchRepository.ListarPartidasPorPatch(patchnumber);
            return Ok(partidas);
        }

        [HttpGet("patch/{patchId}/comp/{compId}")]
        public async Task<IActionResult> GetById(int patchId, int compId)
        {
            var partidas = await _matchRepository.ListarCompIDPorPatchID(patchId, compId);
            return Ok(partidas);

        }

        [HttpGet("comp/{compId}")]
        public async Task<IActionResult> GetMatchesByComp(int compId)
        {
            var partidas = await _matchRepository.ListarMatchesPorComp(compId);

            if (partidas == null || partidas.Count == 0)
                return NotFound();

            return Ok(partidas);
        }


        [HttpPost]
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
        public async Task<IActionResult> Delete(int id)
        {
            var exclude = await _matchRepository.DeleteMatch(id);
            if (!exclude)
                return NotFound("Partida nao encontrada!");
            return Ok("Partida excluida!");
        }
    }
}
