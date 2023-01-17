namespace TweetCountSubscriberAPI.Utilities
{
    public class ServiceBusOptions
    {
        public const string Section = "ServiceBus";

        public string TopicName { get; set; }
        public string ListenAndSendConnectionString { get; set; }
        public string SubscriptionName { get; set; }
    }
}
