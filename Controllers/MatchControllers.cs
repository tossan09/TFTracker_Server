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
