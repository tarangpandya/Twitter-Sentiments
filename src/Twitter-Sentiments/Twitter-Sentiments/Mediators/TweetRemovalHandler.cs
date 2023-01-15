using MediatR;
using TwitterSentiments.Services;

namespace TwitterSentiments.Mediators
{
    public class TweetRemovalHandler : INotificationHandler<ProcessTweetSentimentNotification>
    {
        private readonly ITweetRemovalService tweetRemovalService;

        public TweetRemovalHandler(ITweetRemovalService tweetRemovalService)
        {
            this.tweetRemovalService = tweetRemovalService;
        }


        public Task Handle(ProcessTweetSentimentNotification notification, CancellationToken cancellationToken)
        {
            tweetRemovalService.RemoveRandomTweet();

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            return Task.CompletedTask;
        }
    }
}
