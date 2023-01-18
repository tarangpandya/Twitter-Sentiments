using Azure.Messaging.ServiceBus;
using System.Text;

namespace Tweet_Publisher_Api.Services
{
    public class ServiceBusMessageSender : IMessagePublisher
    {
        private readonly ServiceBusSender serviceBusSender;
        private readonly ILogger<ServiceBusSender> logger;

        public ServiceBusMessageSender(
            ServiceBusSender serviceBusSender
            , ILogger<ServiceBusSender> logger)
        {
            this.serviceBusSender = serviceBusSender;
            this.logger = logger;
        }

        public async Task SendMessageAsync(string message)
        {
            var serviceBusmessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(message));
            await serviceBusSender.SendMessageAsync(serviceBusmessage);
            logger.LogInformation("Tweet published");
        }
    }
}
