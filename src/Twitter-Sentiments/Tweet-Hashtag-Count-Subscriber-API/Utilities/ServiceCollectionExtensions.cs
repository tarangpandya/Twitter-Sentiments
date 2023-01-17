using Azure.Messaging.ServiceBus;
using TweetHashtagCountSubscriberAPI.Data;
using TweetHashtagCountSubscriberAPI.Services;

namespace TweetHashtagCountSubscriberAPI.Utilities
{
    /// <summary>
    /// Service collection extension class for configuring application services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection CofigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration.GetSection(ServiceBusOptions.Section).Get<ServiceBusOptions>());
            services.AddSingleton<ITweetHashTagService, TweetHashTagService>();
            services.AddSingleton<ITagStore, TagStoreInMemory>();
            services.AddHostedService<TweetHashTagCountHostedService>();

            services.AddSingleton(implementationFactory =>
            {
                var serviceBusConfiguration = implementationFactory.GetRequiredService<ServiceBusOptions>();
                var serviceBusClient = new ServiceBusClient(serviceBusConfiguration.ListenAndSendConnectionString);
                var serviceBusReceiver = serviceBusClient.CreateReceiver(serviceBusConfiguration.TopicName, serviceBusConfiguration.SubscriptionName);
                var processor = serviceBusClient.CreateProcessor(serviceBusConfiguration.TopicName, serviceBusConfiguration.SubscriptionName, new ServiceBusProcessorOptions());

                return processor;
            });


            return services;
        }
    }
}