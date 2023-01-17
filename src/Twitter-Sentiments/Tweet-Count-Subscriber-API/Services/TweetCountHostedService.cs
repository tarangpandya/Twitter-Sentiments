using System.Runtime;
using Azure.Messaging.ServiceBus;
using TweetCountSubscriberAPI.Services;

namespace TweetCountSubscriberAPI.Services
{
    public class TweetCountHostedService : BackgroundService, IHostedService, IDisposable
    {
        private readonly ILogger<TweetCountHostedService> _logger;
        private readonly ServiceBusProcessor serviceBusProcessor;
        private readonly ITweetCountService tweetCountService;
        
        public TweetCountHostedService(
            ILogger<TweetCountHostedService> logger
            , ServiceBusProcessor serviceBusProcessor
            , ITweetCountService tweetCountService)
        {
            this._logger = logger;
            this.serviceBusProcessor = serviceBusProcessor;
            this.tweetCountService = tweetCountService;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            serviceBusProcessor.ProcessMessageAsync -= MessageHandlerForSubscriptionOne;
            serviceBusProcessor.ProcessErrorAsync -= ErrorHandler;
            await serviceBusProcessor.StopProcessingAsync();
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
            this.tweetCountService.Add();

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
