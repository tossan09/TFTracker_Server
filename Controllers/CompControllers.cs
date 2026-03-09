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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comps = await _compRepository.ListarComps();
            return Ok(comps);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Comps comps)
        {
            await _compRepository.AdicionarComp(comps);
            return Ok("Comp adicionada");
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
