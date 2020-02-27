using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TradeTips.IntegrationTests.Features.Articles
{
    public class CreateTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_Article()
        {
            var command = new TradeTips.Features.Articles.CreateCommand()
            {
                Article = new TradeTips.Features.Articles.ArticleEditDTO()
                {
                    Title = "Test article dsergiu77",
                    Teaser = "Description of the test article",
                    Link = "Body of the test article",
                    TagList = new string[] { "tag1", "tag2" }
                }
            };

            var article = await ArticleHelpers.CreateArticle(this, command);

            Assert.NotNull(article);
            Assert.Equal(article.Title, command.Article.Title);
            Assert.Equal(article.TagList.Count(), command.Article.TagList.Count());
        }
    }
}