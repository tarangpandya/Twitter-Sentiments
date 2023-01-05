using System.Collections;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace TwitterSentiments.Services
{
    public interface ITweetHashTagService
    {
        public IQueryable GetTopTags(int top);

        public void ExtractTags(string tweetText);
    }
}
