using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TradeTips.IntegrationTests.Features.Users;

namespace TradeTips.IntegrationTests.Features.Articles
{
    public static class ArticleHelpers
    {
        /// <summary>
        /// creates an article based on the given Create command. It also creates a default user
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static async Task<TradeTips.Domain.Article> CreateArticle(SliceFixture fixture,
            TradeTips.Features.Articles.CreateCommand command)
        {
            // first create the default user
            var user = await UserHelpers.CreateDefaultUser(fixture);

            var dbContext = fixture.GetDbContext();
            var currentAccessor = new StubCurrentUserAccessor(user.Username);

            var myProfile = new TradeTips.Features.Articles.MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var articleCreateHandler = new TradeTips.Features.Articles.CreateHandler(dbContext, currentAccessor, mapper);
            var created = await articleCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbArticle = await fixture.ExecuteDbContextAsync(
                db => db.Articles.Where(a => a.ArticleId == created.Article.ArticleId)
                .SingleOrDefaultAsync());

            return dbArticle;
        }
    }
}
