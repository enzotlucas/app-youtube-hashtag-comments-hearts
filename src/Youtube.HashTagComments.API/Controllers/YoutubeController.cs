using Microsoft.AspNetCore.Mvc;

namespace Youtube.HashTagComments.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class YoutubeController : ControllerBase
    {
        private readonly ILogger<YoutubeController> _logger;

        public YoutubeController(ILogger<YoutubeController> logger)
        {
            _logger = logger;
        }
    }
}