using MediatR;
using TwitterSentiments.Services;

namespace TwitterSentiments.Mediators
{
    public class TweetHashTagHandler : INotificationHandler<ProcessTweetSentimentNotification>
    {
        private readonly ITweetHashTagService tweetHashTagService;

        public TweetHashTagHandler(ITweetHashTagService tweetHashTagService)
        {
            this.tweetHashTagService = tweetHashTagService;
        }

        public Task Handle(ProcessTweetSentimentNotification request, CancellationToken cancellationToken)
        {
            tweetHashTagService.ExtractTags(request.Tweet?.Data?.Text);

            if(cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            return Task.CompletedTask;
        }
    }
}
