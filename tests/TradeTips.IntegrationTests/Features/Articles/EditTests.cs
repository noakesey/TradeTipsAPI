using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using TradeTips.Features.Articles;
using TradeTips.IntegrationTests;
using TradeTips.IntegrationTests.Features.Articles;
using Xunit;

namespace TradeTips.IntegrationTests.Features.Articles
{
    public class EditTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Edit_Article()
        {
            var createCommand = new TradeTips.Features.Articles.CreateCommand()
            {
                Article = new TradeTips.Features.Articles.ArticleEditDTO()
                {
                    Title = "Test article dsergiu77",
                    Teaser = "Description of the test article",
                    Link = "Body of the test article",
                    TagList = new string[] { "tag1", "tag2" }
                }
            };

            var createdArticle = await ArticleHelpers.CreateArticle(this, createCommand);

            var command = new TradeTips.Features.Articles.EditCommand()
            {
                Article = new TradeTips.Features.Articles.ArticleEditDTO()
                {
                    Title = "Updated " + createdArticle.Title,
                    Teaser = "Updated" + createdArticle.Teaser,
                    Link = "Updated" + createdArticle.Summary,
                },
                Id = createdArticle.ArticleId
            };
            // remove the first tag and add a new tag
            command.Article.TagList = new string[] { createdArticle.TagList[1], "tag3" };

            var dbContext = GetDbContext();

            var myProfile = new TradeTips.Features.Articles.MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var articleEditHandler = new TradeTips.Features.Articles.EditHandler(dbContext, mapper);
            var edited = await articleEditHandler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(edited);
            Assert.Equal(edited.Article.Title, command.Article.Title);
            Assert.Equal(edited.Article.TagList.Count(), command.Article.TagList.Count());
            // use assert Contains because we do not know the order in which the tags are saved/retrieved
            Assert.Contains(edited.Article.TagList[0], command.Article.TagList);
            Assert.Contains(edited.Article.TagList[1], command.Article.TagList);
        }
    }
}
