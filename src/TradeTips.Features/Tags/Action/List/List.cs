using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;

namespace TradeTips.Features.Tags
{
    public class ListHandler : IRequestHandler<ListQuery, TagsEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;

        public ListHandler(TradeTipsContext context)
        {
            _context = context;
        }

        public async Task<TagsEnvelopeDTO> Handle(ListQuery message, CancellationToken cancellationToken)
        {
            var tags = await _context.Tags.OrderBy(x => x.TagId).AsNoTracking().ToListAsync(cancellationToken);
            return new TagsEnvelopeDTO()
            {
                Tags = tags.Select(x => x.TagId).ToList()
            };
        }
    }
}