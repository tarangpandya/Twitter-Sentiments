using System.Runtime;
using Azure.Core;
using Azure.Messaging.ServiceBus;
using TweetHashtagCountSubscriberAPI.Services;

namespace TweetHashtagCountSubscriberAPI.Services
{
    public class TweetHashTagCountHostedService : BackgroundService, IHostedService, IDisposable
    {
        private readonly ILogger<TweetHashTagCountHostedService> _logger;
        private readonly ServiceBusProcessor serviceBusProcessor;
        private readonly ITweetHashTagService tweetHashTagService;

        public TweetHashTagCountHostedService(
            ILogger<TweetHashTagCountHostedService> logger
            , ServiceBusProcessor serviceBusProcessor
            , ITweetHashTagService tweetHashTagService)
        {
            this._logger = logger;
            this.serviceBusProcessor = serviceBusProcessor;
            this.tweetHashTagService = tweetHashTagService;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            serviceBusProcessor.ProcessMessageAsync -= MessageHandlerForSubscriptionOne;
            serviceBusProcessor.ProcessErrorAsync -= ErrorHandler;
            await serviceBusProcessor.StopProcessingAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                serviceBusProcessor.ProcessMessageAsync += MessageHandlerForSubscriptionOne;
                serviceBusProcessor.ProcessErrorAsync += ErrorHandler;
                await serviceBusProcessor.StartProcessingAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MyHostedService");
            }
        }

        async Task MessageHandlerForSubscriptionOne(ProcessMessageEventArgs args)
        {
            _logger.LogInformation("Processing Tweet");
            tweetHashTagService.ExtractTags(args.Message.Body?.ToString());

            // complete the message. messages is deleted from the subscription. 
            await args.CompleteMessageAsync(args.Message);
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Error in MyHostedService");
            return Task.CompletedTask;
        }

        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
