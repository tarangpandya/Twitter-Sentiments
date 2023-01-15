using MediatR;
using Newtonsoft.Json;
using TwitterSentiments.Domain;
using TwitterSentiments.Mediators;
using TwitterSentiments.Utilities;

namespace TwitterSentiments.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;
        private readonly TwitterOptions twitterOptions;

        public TwitterService(IHttpClientFactory httpClientFactory
            , IMediator mediator
            , TwitterOptions twitterOptions)
        {
            this.httpClientFactory = httpClientFactory;
            this.mediator = mediator;
            this.twitterOptions = twitterOptions;
        }

        public async Task FetchTweets()
        {
            var httpClient = httpClientFactory.CreateClient(twitterOptions.HttpClientName);
            var responseStrem = await httpClient.GetStreamAsync(twitterOptions.VolumeSampleUrl);
            Tweet tweet;
            using (BufferedStream bufferedStream = new(responseStrem))
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