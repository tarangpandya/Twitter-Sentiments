using Microsoft.AspNetCore.Mvc;
using Tweet_Publisher_Api.Services;

namespace Tweet_Publisher_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TwittterSentimentController : Controller
    {
        private readonly ILogger<TwittterSentimentController> logger;
        private readonly ITwitterService twitterService;

        public TwittterSentimentController(
                ILogger<TwittterSentimentController> logger
                , ITwitterService twitterService
            )
        {
            this.logger = logger;
            this.twitterService = twitterService;
        }

        [HttpGet]
        public async Task<IActionResult> FetchTweets()
        {
            try
            {
                await this.twitterService.FetchTweets();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while fetching tweets");
                return BadRequest("Unable to fetch records.");
            }
        }
    }
}