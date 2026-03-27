using Microsoft.AspNetCore.Mvc;
using TFTDataTrackerApi.Repository;

namespace TFTDataTrackerApi.Controllers
{
    [ApiController]
    [Route("api/stats")]
    public class StatsController : ControllerBase
    {
        private readonly StatsRepository _statsRepository;

        public StatsController(StatsRepository statsRepository)
        {
            _statsRepository = statsRepository;
        }

        [HttpGet("patch")]
        public async Task<IActionResult> GetStatsPorPatch()
        {
            var stats = await _statsRepository.GetStatsPorPatch();
            return Ok(stats);
        }

        [HttpGet("comp/{compId}")]
        public async Task<IActionResult> GetStatsPorComp(string compId, [FromQuery] string? patchId)
        {
            var stats = await _statsRepository.GetStatsPorComp(compId, patchId);
            return Ok(stats);
        }
    }
}
