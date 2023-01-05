namespace TwitterSentiments.Services
{
    public interface ITweetHashTagService
    {
        public IQueryable GetTopTags(int top);

        public void ExtractTags(string tweetText);
    }
}