using CameraSystem.Server.Data;
using CameraSystem.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CameraSystem.Server.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly VideoDataDb _context;

        public ApiController(VideoDataDb context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("SaveTelemetry")]
        public async Task<IActionResult> SaveTelemetry(CameraTelemetry telemetry)
        {
            if(telemetry is null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            LoggedCameraTelemetry loggedCameraTelemetry = new LoggedCameraTelemetry()
            {
                LogId = Guid.NewGuid().ToString(),
                CardId = telemetry.CardId,
                PassDateTime = telemetry.PassDateTime,
                Log = telemetry.Log
            };
           
            await _context.AddAsync(loggedCameraTelemetry);
            await _context.SaveChangesAsync();

            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}
