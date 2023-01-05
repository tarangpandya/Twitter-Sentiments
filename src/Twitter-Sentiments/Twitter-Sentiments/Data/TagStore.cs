using System.Collections.Concurrent;

namespace TwitterSentiments.Data
{
    public interface ITagStore
    {
        public void AddTag(string tag);

        public IDictionary<string, int> GetTags();
    }

    public class TagStore : ITagStore
    {
        private ConcurrentDictionary<string, int> Tags { get; set; }

        public TagStore()
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
    }
}