using MediatR;
using Newtonsoft.Json;
using Polly.CircuitBreaker;
using Polly;
using TwitterSentiments.Domain;
using TwitterSentiments.Mediators;
using TwitterSentiments.Utilities;
using System.Net.Http;
using System.Net;
using Polly.Retry;

namespace TwitterSentiments.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;
        private readonly TwitterOptions twitterOptions;
        private readonly ILogger<TwitterService> logger;

        public TwitterService(IHttpClientFactory httpClientFactory
            , IMediator mediator
            , TwitterOptions twitterOptions
            , ILogger<TwitterService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.mediator = mediator;
            this.twitterOptions = twitterOptions;
            this.logger = logger;
        }

        public async Task FetchTweets()
        {
            logger.LogInformation("Trying to fetch tweet");
            var httpClient = httpClientFactory.CreateClient(twitterOptions.HttpClientName);
            Tweet tweet;

            var responseStream = await httpClient.GetStreamAsync(twitterOptions.VolumeSampleUrl);
            using (BufferedStream bufferedStream = new(responseStream))
            using (StreamReader reader = new(bufferedStream))
            {
                while (reader != null && !reader.EndOfStream)
                {
                    tweet = JsonConvert.DeserializeObject<Tweet>(reader.ReadLine());
                    if (tweet != null)
                    {
                        await mediator.Publish(new ProcessTweetSentimentNotification(tweet));
                    }
                }
            }
        }
    }
}