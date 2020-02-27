using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TradeTips.Features.Comments;
using TradeTips.IntegrationTests.Features.Users;

namespace TradeTips.IntegrationTests.Features.Comments
{
    public static class CommentHelpers
    {
        /// <summary>
        /// creates an article comment based on the given Create command. 
        /// Creates a default user if parameter userName is empty.
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="command"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static async Task<TradeTips.Domain.Comment> CreateComment(SliceFixture fixture, 
            CreateCommand command, string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                var user = await UserHelpers.CreateDefaultUser(fixture);
                userName = user.Username;
            }

            var dbContext = fixture.GetDbContext();
            var currentAccessor = new StubCurrentUserAccessor(userName);

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TradeTips.Features.Articles.MappingProfile());
                cfg.AddProfile(new TradeTips.Features.Comments.MappingProfile());
            });

            var mapper = new Mapper(configuration);

            var commentCreateHandler = new CreateHandler(dbContext, currentAccessor, mapper);
            var created = await commentCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbArticleWithComments = await fixture.ExecuteDbContextAsync(
                db => db.Articles
                    .Include(a => a.Comments).Include(a => a.Author)
                    .Where(a => a.ArticleId == command.Id)
                    .SingleOrDefaultAsync()
            );

            var dbComment = dbArticleWithComments.Comments
                .Where(c => c.ArticleId == dbArticleWithComments.ArticleId && c.Author == dbArticleWithComments.Author)
                .FirstOrDefault();

            return dbComment;
        }
    }
}
