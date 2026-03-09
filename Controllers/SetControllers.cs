using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using TFTDataTrackerApi.Models;

namespace TFTDataTrackerApi.Controllers
{
    [ApiController]
    [Route("api/set")]
    public class SetControllers : ControllerBase
    {
        private readonly SetRepository _setRepository;

        public SetControllers(SetRepository setRepository)
        {
            _setRepository = setRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sets = await _setRepository.ListarSets();
            return Ok(sets);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Sets sets)
        {
            var ok = await _setRepository.AdicionarSet(sets);
            return Ok("Set criado");
        }
    }
}
