using TwitterSentiments.Data;
using TwitterSentiments.Services;
using TwitterSentiments.Utilities;

namespace Twitter_Sentiments.Tests
{
    public class StringFunctionsTest
    {
        [Theory]
        [InlineData("No Tags", new string[] { })]
        [InlineData("#Tag1 and this is #Tag2", new string[] { "#Tag1", "#Tag2" })]
        [InlineData("", new string[] { })]
        [InlineData("This contains #1234 and #T12F", new string[] { "#1234", "#T12F" })]
        [InlineData("This contains #1234 #T12F", new string[] { "#1234","#T12F" })]
        [InlineData("This contains #1234#T12F", new string[] { "#1234", "#T12F" })]
        [InlineData("This contains non english text like #٢٢٢", new string[] { "#٢٢٢" })]
        public void ExtractTags_WorksOk(string testString, string[] expected)
        {
            var actual = StringFunctions.ExtractTwitterTagsFromText(testString);
            Assert.Equal(expected, actual);
        }
    }
}