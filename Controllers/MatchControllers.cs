using Microsoft.AspNetCore.Mvc;
using TFTDataTrackerApi.Models;
using TFTDataTrackerApi.Repository;

namespace TFTDataTrackerApi.Controllers
{
    [ApiController]
    [Route("/")]
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
        
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Matches matches)
        {
            await _matchRepository.AddPartida(matches);
            return Ok("Partida adicionada!");
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
