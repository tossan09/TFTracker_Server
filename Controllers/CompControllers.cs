using Microsoft.AspNetCore.Mvc;
using TFTDataTrackerApi.Models;
using TFTDataTrackerApi.Repository;

namespace TFTDataTrackerApi.Controllers
{
    [ApiController]
    [Route("/comps")]
    public class CompControllers : ControllerBase
    {
        private readonly CompRepository _compRepository;

        public CompControllers(CompRepository compRepository)
        {
            _compRepository = compRepository;
        }

        // tabela comps
        [HttpGet("set/{setNumber}")]
        public async Task<IActionResult> GetCompsBySet(int setNumber)
        {
            var comps = await _compRepository.ListarCompsPorSet(setNumber);
            return Ok(comps);
        }

        // rolldwon de add form
        [HttpGet("byPatch/{setid}")]
        public async Task<IActionResult> GetCompsByPatch(int setid)
        {
            var comps = await _compRepository.ListarCompsPorPatch(setid);
            return Ok(comps);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Comps comps)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _compRepository.AddComp(comps);
                return Ok(comps);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno", detail = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Comps comp)
        {
            var edit = await _compRepository.EditComp(id, comp);
            if (!edit)
                return NotFound("Comp não encontrada");

            return Ok("Atualizado!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var excluir = await _compRepository.DeleteComp(id);
            if (!excluir)
                return NotFound("Comp nao encontrada");
            return Ok("Comp removida");
        }
    }
}
