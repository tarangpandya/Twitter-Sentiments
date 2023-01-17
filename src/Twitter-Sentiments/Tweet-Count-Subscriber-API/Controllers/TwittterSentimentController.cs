using Microsoft.AspNetCore.Mvc;
using TweetCountSubscriberAPI.Services;

namespace TweetCountSubscriberAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TwittterSentimentController : Controller
    {
        private readonly ITweetCountService tweetCountService;

        public TwittterSentimentController(ITweetCountService tweetCountService)
        {
            this.tweetCountService = tweetCountService;
        }

        [HttpGet]
        public IActionResult GetTweetCount()
        {
            var result = $"Tweets count so far: {this.tweetCountService.GetCount()}";
            return Ok(result);
        }
    }
}