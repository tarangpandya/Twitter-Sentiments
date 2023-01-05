using TwitterSentiments.Data;

namespace TwitterSentiments.Tests
{
    public class TagStoreTests
    {
        [Theory]
        [InlineData(50)]
        [InlineData(10000)]
        public void TagStore_WorksOk(int iteration)
        {
            var tagStore = new TagStore();
            var list = new List<string> { "one", "two", "three", "four" };
            var random = new Random();
            Parallel.For(0, iteration, i =>
            {
                tagStore.AddTag($"#{list[random.Next(list.Count)]}");
            });

            var total = tagStore.GetTags().Sum(a => a.Value);

            Assert.Equal(iteration, total);
        }
    }
}
