using TweetHashtagCountSubscriberAPI.Data;
using TweetHashtagCountSubscriberAPI.Utilities;

namespace TweetHashtagCountSubscriberAPI.Services
{
    public class TweetHashTagService : ITweetHashTagService
    {
        private readonly ITagStore tagStore;
        private readonly ILogger<TweetHashTagService> logger;

        public TweetHashTagService(ITagStore tagStore, ILogger<TweetHashTagService> logger)
        {
            this.tagStore = tagStore;
            this.logger = logger;
        }

        public void ExtractTags(string tweetText)
        {
            try
            {
                var tags = StringFunctions.ExtractTwitterTagsFromText(tweetText); ;
                if (tags != null && tags.Any())
                {
                    this.SaveTag(tags);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while saving tag from {tweetText}.");
            }
        }

        public IQueryable GetTopTags(int top)
        {
            var tags = this.tagStore.GetTags()
                .OrderByDescending(v => v.Value)
                .Take(top)
                .Select(p => new
                {
                    Tag = p.Key,
                    Occurance = p.Value,
                }).AsQueryable();
            return tags;
        }

        private void SaveTag(string[] tags)
        {
            Array.ForEach(tags, new Action<string>(t =>
            {
                if (!string.IsNullOrEmpty(t))
                {
                    this.tagStore.AddTag(t);
                }
            }));
        }
    }
}