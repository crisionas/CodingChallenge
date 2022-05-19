using CC.Common;
using CC.UploadService.Interfaces;
using CC.UploadService.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CC.UploadService.Controllers
{
    [Authorize]
    [Route("track")]
    [ApiController]
    public class TrackController : BaseController
    {
        private readonly ITrackWorker _trackWorker;
        public TrackController(ITrackWorker trackWorker)
        {
            _trackWorker = trackWorker;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(FileUploadResponse))]
        [ProducesResponseType(400, Type = typeof(FileUploadResponse))]
        public async Task<IActionResult> TrackFile([FromQuery] string trackId)
        {
            var response = await _trackWorker.GetTrackStatusAsync(trackId);
            return PrepareActionResult(response);
        }
    }
}
