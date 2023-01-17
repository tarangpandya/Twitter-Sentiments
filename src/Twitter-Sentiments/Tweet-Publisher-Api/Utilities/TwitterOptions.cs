namespace Tweet_Publisher_Api.Utilities
{
    public class TwitterOptions
    {
        public const string Section = "Twitter";

        public string BaseUrl { get; set; } = string.Empty;

        public string VolumeSampleUrl { get; set; } = string.Empty;

        public string AuthToken { get; set; } = string.Empty;

        public string HttpClientName { get; set; } = string.Empty;
    }
}