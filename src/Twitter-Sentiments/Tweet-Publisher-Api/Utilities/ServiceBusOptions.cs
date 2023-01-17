namespace Tweet_Publisher_Api.Utilities
{
    public class ServiceBusOptions
    {
        public const string Section = "ServiceBus";

        public string TopicName { get; set; }
        public string ListenAndSendConnectionString { get; set; }
    }
}
