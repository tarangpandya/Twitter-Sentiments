using Microsoft.AspNetCore.Mvc;
using TweetHashtagCountSubscriberAPI.Services;

namespace TweetHashtagCountSubscriberAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TwittterSentimentController : Controller
    {
        private readonly ITweetHashTagService tweetHashTagService;

        public TwittterSentimentController(ITweetHashTagService tweetHashTagService)
        {
            this.tweetHashTagService = tweetHashTagService;
        }

        [HttpGet]
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