using Microsoft.AspNetCore.Mvc;
using TwitterSentiments.Services;

namespace TwitterSentiments.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TwittterSentimentController : Controller
    {
        private readonly ILogger<TwittterSentimentController> logger;
        private readonly ITwitterService twitterService;
        private readonly ITweetCountService tweetCountService;
        private readonly ITweetHashTagService tweetHashTagService;

        public TwittterSentimentController(ILogger<TwittterSentimentController> logger
            , ITwitterService twitterService
            , ITweetCountService tweetCountService
            , ITweetHashTagService tweetHashTagService
            )
        {
            this.logger = logger;
            this.twitterService = twitterService;
            this.tweetCountService = tweetCountService;
            this.tweetHashTagService = tweetHashTagService;
        }

        [HttpGet("FetchTweets")]
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

        [HttpGet("GetTweetCount")]
        public IActionResult GetTweetCount()
        {
            var result = $"Tweets count so far: {this.tweetCountService.GetCount()}";
            return Ok(result);
        }

        [HttpGet("GetTopTags")]
        public IActionResult GetTopTags([FromQuery(Name = "top")] int top)
        {
            if (top == 0)
            {
                top = 10;
            }

            var result = this.tweetHashTagService.GetTopTags(top);
            return Ok(result);
        }
    }
}