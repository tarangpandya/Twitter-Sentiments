using MediatR;
using Newtonsoft.Json;
using Polly.CircuitBreaker;
using Polly;
using Tweet_Publisher_Api.Domain;
using Tweet_Publisher_Api.Utilities;
using System.Net.Http;
using System.Net;
using Polly.Retry;
using Azure.Messaging.ServiceBus;
using System.Text;

namespace Tweet_Publisher_Api.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly TwitterOptions twitterOptions;
        private readonly ILogger<TwitterService> logger;
        private readonly IMessagePublisher messagePublisher;

        public TwitterService(
                IHttpClientFactory httpClientFactory
                , TwitterOptions twitterOptions
                , ILogger<TwitterService> logger
                , IMessagePublisher messagePublisher
                )
        {
            this.httpClientFactory = httpClientFactory;
            this.twitterOptions = twitterOptions;
            this.logger = logger;
            this.messagePublisher = messagePublisher;
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
                        await messagePublisher.SendMessageAsync($"{tweet?.Data.Text}");
                    }
                }
            }
        }
    }
}