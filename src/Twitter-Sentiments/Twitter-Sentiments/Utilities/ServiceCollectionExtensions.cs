using System.Reflection;
using MediatR;
using TwitterSentiments.Data;
using TwitterSentiments.Services;

namespace TwitterSentiments.Utilities
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
            services.AddSingleton<ITweetCountService, TweetCountService>();
            services.AddSingleton<ITagStore, TagStoreInMemory>();
            services.AddSingleton<ITweetHashTagService, TweetHashTagService>();
            services.AddSingleton<ITweetRemovalService, TweetRemovalService>();
            services.AddScoped<ITwitterService, TwitterService>();
            services.AddSwaggerGen();
            return services;
        }
    }
}