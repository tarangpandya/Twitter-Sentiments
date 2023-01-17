using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Serilog.Core;

namespace Tweet_Publisher_Api.Utilities
{
    /// <summary>
    /// Configure HTTPclient for Twitter APIs.
    /// </summary>
    public static class ConfigureHttpClientForTwitter
    {
        public static IServiceCollection ConfigureTwitterHttpClient(this IServiceCollection services, IConfiguration configuration, Logger logger)
        {
            var twitterOptions = configuration.GetSection(TwitterOptions.Section).Get<TwitterOptions>();
            
            services.AddHttpClient(twitterOptions.HttpClientName, httpClient =>
            {
                httpClient.BaseAddress = new Uri(twitterOptions.BaseUrl);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {twitterOptions.AuthToken}");
                httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Automatic,
                ServerCertificateCustomValidationCallback = (httprequestmessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                }
            })
            .AddPolicyHandler(GetRetryPolicy(logger))
            .AddPolicyHandler(GetCircuitBreakerPolicy(logger));

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(Logger logger)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(2, retryAttempt => {
                    var timeToWait = TimeSpan.FromMinutes(15);
                    logger.Information($"Waiting {timeToWait.TotalMinutes} minutes");
                    return timeToWait;
                    });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(Logger logger)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
                .CircuitBreakerAsync(5, 
                    TimeSpan.FromMinutes(30),
                    onBreak: (_, duration) => {
                        logger.Error($"Circuit tripped. Circuit is open and requests won't be allowed through for duration={duration}");
                    },
                    onReset: () => {
                        logger.Error($"Circuit closed. Requests are now allowed through");
                    });
        }
    }
}