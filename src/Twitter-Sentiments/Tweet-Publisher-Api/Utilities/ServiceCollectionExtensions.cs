using System.Reflection;
using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tweet_Publisher_Api.Utilities;
using Tweet_Publisher_Api.Services;

namespace Tweet_Publisher_Api.Utilities
{
    /// <summary>
    /// Service collection extension class for configuring application services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection CofigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSingleton(configuration.GetSection(TwitterOptions.Section).Get<TwitterOptions>());
            services.AddSingleton(configuration.GetSection(ServiceBusOptions.Section).Get<ServiceBusOptions>());
            services.AddScoped<ITwitterService, TwitterService>();
            services.AddSwaggerGen();


            services.TryAddSingleton(implementationFactory =>
            {
                var serviceBusConfiguration = implementationFactory.GetRequiredService<ServiceBusOptions>();
                var serviceBusClient = new ServiceBusClient(serviceBusConfiguration.ListenAndSendConnectionString);
                var serviceBusSender = serviceBusClient.CreateSender(serviceBusConfiguration.TopicName);
                return serviceBusSender;
            });


            return services;
        }
    }
}