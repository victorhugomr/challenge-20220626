using Microsoft.AspNetCore.Mvc;
using challenge_20220626.Services;

namespace challenge_20220626.Controllers{
    [ApiController]
    [Route("/cron")]
    public class CRONController : Controller{
        
        private readonly ScrapingService _cronSystem;

        public CRONController(ScrapingService cronSystem){
            _cronSystem = cronSystem;
        }

        public void CRON()
            => _cronSystem.WebScraper();

    }
}