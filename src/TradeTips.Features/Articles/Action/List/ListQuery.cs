using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Articles
{
    public class ListQuery : IRequest<ArticlesEnvelopeDTO>
    {
        public ListQuery(string tag, string stockId, string favorited, int? limit, int? offset)
        {
            Tag = tag;
            StockId = stockId;
            FavoritedUsername = favorited;
            Limit = limit;
            Offset = offset;
        }

        public string Tag { get; }
        public string StockId { get; }
        public string FavoritedUsername { get; }
        public int? Limit { get; }
        public int? Offset { get; }
        public bool IsFeed { get; set; }
    }
}
