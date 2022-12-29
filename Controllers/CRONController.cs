using Microsoft.AspNetCore.Mvc;
using System.Timers;
using challenge_20220626.Services;

namespace challenge_20220626.Controllers{
    [Route("/scraper")]
    [ApiController]
    public class CRONController : Controller{
        
        private readonly CRONSystem _cronSystem;

        public CRONController(CRONSystem cronSystem){
            _cronSystem = cronSystem;
        }

        [HttpGet]
        public void Nome()
            => _cronSystem.WebScraper();

    }
}