namespace TwitterSentiments.Services
{
    public class TweetCountService : ITweetCountService
    {
        private int Tweets { get; set; }

        public TweetCountService()
        {
            Tweets = 0;
        }

        public void Add()
        {
            this.Tweets++;
        }

        public int GetCount()
        {
            return this.Tweets;
        }
    }
}