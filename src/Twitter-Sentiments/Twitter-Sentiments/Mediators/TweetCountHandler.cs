using MediatR;
using TwitterSentiments.Services;

namespace TwitterSentiments.Mediators
{
    public class TweetCountHandler : INotificationHandler<ProcessTweetSentimentNotification>
    {
        private readonly ITweetCountService tweetCountService;

        public TweetCountHandler(ITweetCountService tweetCountService)
        {
            this.tweetCountService = tweetCountService;
        }

        public Task Handle(ProcessTweetSentimentNotification request, CancellationToken cancellationToken)
        {
            tweetCountService.Add();
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }
            return Task.CompletedTask;
        }
    }
}