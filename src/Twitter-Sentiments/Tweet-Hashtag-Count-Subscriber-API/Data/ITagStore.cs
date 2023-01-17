namespace TweetHashtagCountSubscriberAPI.Data
{
    public interface ITagStore
    {
        public void AddTag(string tag);

        public IDictionary<string, int> GetTags();

        public void Remove(string tag);

        public int Count { get; }
    }
}