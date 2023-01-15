using TwitterSentiments.Data;

namespace TwitterSentiments.Services
{
    public class TweetRemovalService : ITweetRemovalService
    {
        private readonly ITagStore tagStore;
        private readonly ILogger<TweetRemovalService> logger;

        public TweetRemovalService(ITagStore tagStore, 
            ILogger<TweetRemovalService> logger)
        {
            this.tagStore = tagStore;
            this.logger = logger;
        }

        public void RemoveRandomTweet()
        {
            if(tagStore.Count > 10000)
            {
                var tag = tagStore.GetTags()
                    .OrderBy(v => v.Value)
                    .FirstOrDefault();

                tagStore.Remove(tag.Key);
                logger.LogInformation($"Removed tweet with key {tag.Key} and value {tag.Value }.");
            }
        }
    }
}
