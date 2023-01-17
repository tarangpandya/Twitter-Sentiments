using System.Collections.Concurrent;

namespace TweetHashtagCountSubscriberAPI.Data
{
    public class TagStoreInMemory : ITagStore
    {
        private ConcurrentDictionary<string, int> Tags { get; set; }

        public TagStoreInMemory()
        {
            Tags = new ConcurrentDictionary<string, int>();
        }

        public void AddTag(string tag)
        {
            Tags.AddOrUpdate(tag, 1, (key, oldvalue) => oldvalue + 1);
        }

        public IDictionary<string, int> GetTags()
        {
            return Tags;
        }

        public void Remove(string tag)
        {
            Tags.Remove(tag, out _);
        }

        public int Count => Tags.Count;
    }
}