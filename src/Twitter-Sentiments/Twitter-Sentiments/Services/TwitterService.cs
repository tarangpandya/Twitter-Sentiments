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
        private readonly AsyncCircuitBreakerPolicy circuitBreakerPolicy;
        private readonly AsyncRetryPolicy retryPolicy;

        public TwitterService(IHttpClientFactory httpClientFactory
            , IMediator mediator
            , TwitterOptions twitterOptions
            , ILogger<TwitterService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.mediator = mediator;
            this.twitterOptions = twitterOptions;
            this.logger = logger;

            circuitBreakerPolicy = Policy.Handle<HttpRequestException>(ex => ex.StatusCode == HttpStatusCode.TooManyRequests)
             .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 1,
                durationOfBreak: TimeSpan.FromMinutes(5),
                onBreak: (_, duration) => this.logger.LogInformation($"Circuit tripped. Circuit is open and requests won't be allowed through for duration={duration}"),
                onReset: () => this.logger.LogInformation("Circuit closed. Requests are now allowed through"),
                onHalfOpen: () => this.logger.LogInformation("Circuit is now half-opened and will test the service with the next request"));

            retryPolicy = Policy
                            .Handle<HttpRequestException>(ex => ex.StatusCode == HttpStatusCode.TooManyRequests)
                            .WaitAndRetryAsync(2, retryAttempt => {
                                var timeToWait = TimeSpan.FromMinutes(5);
                                this.logger.LogInformation($"Waiting {timeToWait.TotalSeconds} seconds");
                                return timeToWait;
                            });

        }

        public async Task FetchTweets()
        {
            var httpClient = httpClientFactory.CreateClient(twitterOptions.HttpClientName);
            Tweet tweet;

            await retryPolicy.ExecuteAsync(async () =>
            {
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
            });
        }
    }
}