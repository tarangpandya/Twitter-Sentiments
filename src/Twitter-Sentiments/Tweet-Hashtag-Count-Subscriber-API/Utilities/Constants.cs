namespace TweetHashtagCountSubscriberAPI.Utilities
{
    public static class Constants
    {
        public static string Bearer = "Bearer";

        public static string HashTagPattern = @"(?!\s)#([A-Za-z0-9]|\d[A-Za-z0-9]|(?:[\p{L}\p{M}\p{N}_]))\w*\b";
    }
}