namespace TwitterSentiments.Domain
{
    public class Data
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public string[] Edit_History_Tweet_Ids { get; set; }
    }

    public class Tweet
    {
        public Data Data { get; set; }
    }
}