using System.Diagnostics.Metrics;
using MediatR;
using Microsoft.Extensions.Logging;
using TwitterSentiments.Domain;

namespace TwitterSentiments.Mediators
{
    public class ProcessTweetSentimentNotification : INotification
    {
        public Tweet Tweet { get; set; }

        public ProcessTweetSentimentNotification(Tweet tweet)
        {
            Tweet = tweet;
        }
    }
}
