using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;

namespace TradeTips.Features.Articles
{
    public class EditHandler : IRequestHandler<EditCommand, ArticleEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly IMapper _mapper;

        public EditHandler(TradeTipsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ArticleEnvelopeDTO> Handle(EditCommand message, CancellationToken cancellationToken)
        {
            var article = await _context.Articles
                .Include(x => x.ArticleTags) // include also the article tags since they also need to be updated
                .Where(x => x.ArticleId == message.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (article == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { ArticleSummaryDTO = Constants.NOT_FOUND });
            }

            article.Teaser = message.Article.Teaser ?? article.Teaser;
            article.Summary = message.Article.Link ?? article.Summary;
            article.Title = message.Article.Title ?? article.Title;
            article.Slug = article.Title.GenerateSlug();

            // list of currently saved article tags for the given article
            var articleTagList = (message.Article.TagList ?? Enumerable.Empty<string>());

            var articleTagsToCreate = GetArticleTagsToCreate(article, articleTagList);
            var articleTagsToDelete = GetArticleTagsToDelete(article, articleTagList);

            if (_context.ChangeTracker.Entries().First(x => x.Entity == article).State == EntityState.Modified
                || articleTagsToCreate.Any() || articleTagsToDelete.Any())
            {
                article.UpdatedDate = DateTime.UtcNow;
            }

            // add the new article tags
            await _context.ArticleTags.AddRangeAsync(articleTagsToCreate, cancellationToken);
            // delete the tags that do not exist anymore
            _context.ArticleTags.RemoveRange(articleTagsToDelete);

            await _context.SaveChangesAsync(cancellationToken);

            var updatedArticle = await _context.Articles
                .Include(x => x.Author)
                .Include(x => x.ArticleFavorites)
                .Include(x => x.ArticleTags)
                .AsNoTracking()
                .Where(x => x.Slug == article.Slug)
                .FirstOrDefaultAsync(cancellationToken);

            ArticleDetailDTO dto = _mapper.Map<ArticleDetailDTO>(updatedArticle);

            return new ArticleEnvelopeDTO(dto);
        }

        /// <summary>
        /// check which article tags need to be added
        /// </summary>
        private static List<ArticleTag> GetArticleTagsToCreate(Article article, IEnumerable<string> articleTagList)
        {
            var articleTagsToCreate = new List<ArticleTag>();
            foreach (var tag in articleTagList)
            {
                var at = article.ArticleTags.FirstOrDefault(t => t.TagId == tag);
                if (at == null)
                {
                    at = new ArticleTag()
                    {
                        Article = article,
                        ArticleId = article.ArticleId,
                        Tag = new Tag() { TagId = tag },
                        TagId = tag
                    };
                    articleTagsToCreate.Add(at);
                }
            }

            return articleTagsToCreate;
        }

        /// <summary>
        /// check which article tags need to be deleted
        /// </summary>
        private static List<ArticleTag> GetArticleTagsToDelete(Article article, IEnumerable<string> articleTagList)
        {
            var articleTagsToDelete = new List<ArticleTag>();
            foreach (var tag in article.ArticleTags)
            {
                var at = articleTagList.FirstOrDefault(t => t == tag.TagId);
                if (at == null)
                {
                    articleTagsToDelete.Add(tag);
                }
            }

            return articleTagsToDelete;
        }
    }
}
