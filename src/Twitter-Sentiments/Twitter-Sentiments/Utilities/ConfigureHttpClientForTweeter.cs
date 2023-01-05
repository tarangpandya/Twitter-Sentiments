namespace TwitterSentiments.Utilities
{
    /// <summary>
    /// Configure HTTPclient for Twitter APIs.
    /// </summary>
    public static class ConfigureHttpClientForTwitter
    {
        public static IServiceCollection ConfigureTwitterHttpClient(this IServiceCollection services, IConfiguration configuration)
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
            }); ;
            return services;
        }
    }
}