using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TradeTips.Features.Articles;
using TradeTips.IntegrationTests;
using TradeTips.IntegrationTests.Features.Articles;
using TradeTips.IntegrationTests.Features.Comments;
using TradeTips.IntegrationTests.Features.Users;
using Xunit;

namespace TradeTips.IntegrationTests.Features.Articles
{
    public class DeleteTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Delete_Article()
        {
            var createCmd = new TradeTips.Features.Articles.CreateCommand()
            {
                Article = new TradeTips.Features.Articles.ArticleEditDTO()
                {
                    Title = "Test article dsergiu77",
                    Teaser = "Description of the test article",
                    Link = "Body of the test article",
                }
            };

            var article = await ArticleHelpers.CreateArticle(this, createCmd);
            var id = article.ArticleId;

            var deleteCmd = new TradeTips.Features.Articles.DeleteCommand(id);

            var dbContext = GetDbContext();

            var articleDeleteHandler = new TradeTips.Features.Articles.DeleteHandler(dbContext);
            await articleDeleteHandler.Handle(deleteCmd, new System.Threading.CancellationToken());

            var dbArticle = await ExecuteDbContextAsync(db => db.Articles.Where(d => d.ArticleId == deleteCmd.Id).SingleOrDefaultAsync());

            Assert.Null(dbArticle);
        }

        [Fact]
        public async Task Expect_Delete_Article_With_Tags()
        {
            var createCmd = new TradeTips.Features.Articles.CreateCommand()
            {
                Article = new TradeTips.Features.Articles.ArticleEditDTO()
                {
                    Title = "Test article dsergiu77",
                    Teaser = "Description of the test article",
                    Link = "Body of the test article",
                    TagList = new string[] { "tag1", "tag2" }
                }
            };

            var article = await ArticleHelpers.CreateArticle(this, createCmd);
            var dbArticleWithTags = await ExecuteDbContextAsync(
                db => db.Articles.Include(a => a.ArticleTags)
                .Where(d => d.Slug == article.Slug).SingleOrDefaultAsync()
            );

            var deleteCmd = new TradeTips.Features.Articles.DeleteCommand(article.ArticleId);

            var dbContext = GetDbContext();

            var articleDeleteHandler = new TradeTips.Features.Articles.DeleteHandler(dbContext);
            await articleDeleteHandler.Handle(deleteCmd, new System.Threading.CancellationToken());

            var dbArticle = await ExecuteDbContextAsync(db => db.Articles.Where(d => d.ArticleId == deleteCmd.Id).SingleOrDefaultAsync());
            Assert.Null(dbArticle);
        }

        [Fact]
        public async Task Expect_Delete_Article_With_Comments()
        {
            var createArticleCmd = new TradeTips.Features.Articles.CreateCommand()
            {
                Article = new TradeTips.Features.Articles.ArticleEditDTO()
                {
                    Title = "Test article dsergiu77",
                    Teaser = "Description of the test article",
                    Link = "Body of the test article",
                }
            };

            var article = await ArticleHelpers.CreateArticle(this, createArticleCmd);
            var dbArticle = await ExecuteDbContextAsync(
                db => db.Articles.Include(a => a.ArticleTags)
                .Where(d => d.Slug == article.Slug).SingleOrDefaultAsync()
            );

            var articleId = dbArticle.ArticleId;

            // create article comment
            var createCommentCmd = new TradeTips.Features.Comments.CreateCommand()
            {
                Comment = new TradeTips.Features.Comments.CommentDTO()
                {
                    Body = "article comment"
                },
                Id = articleId
            };

            var comment = await CommentHelpers.CreateComment(this, createCommentCmd, UserHelpers.DefaultUserName);

            // delete article with comment
            var deleteCmd = new TradeTips.Features.Articles.DeleteCommand(articleId);

            var dbContext = GetDbContext();

            var articleDeleteHandler = new TradeTips.Features.Articles.DeleteHandler(dbContext);
            await articleDeleteHandler.Handle(deleteCmd, new System.Threading.CancellationToken());

            var deleted = await ExecuteDbContextAsync(db => db.Articles.Where(d => d.ArticleId == deleteCmd.Id).SingleOrDefaultAsync());
            Assert.Null(deleted);
        }
    }
}
