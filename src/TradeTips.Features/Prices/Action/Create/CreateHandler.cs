using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;
using TradeTips.Features.Stocks;

namespace TradeTips.Features.Prices
{
    public class CreateHandler : IRequestHandler<CreateCommand, StockEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly IMapper _mapper;

        public CreateHandler(TradeTipsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<StockEnvelopeDTO> Handle(CreateCommand message, CancellationToken cancellationToken)
        {
            var stock = await _context.Stocks
                .Where(x => x.StockId == message.StockId)
                .AnyAsync(cancellationToken);

            if (!stock)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { StockDTO = Constants.NOT_FOUND });
            }

            foreach (var price in message.Prices)
            {
                if (price.Open == 0 && price.Close == 0)
                {
                    var existingPrice = _context.IntraDayPrices
                        .Any(p => p.StockId == message.StockId
                             && p.TransDate == price.TransDate);

                    if (!existingPrice)
                    {
                        IntraDayPrice intraDayPrice = _mapper.Map<IntraDayPrice>(price);
                        intraDayPrice.StockId = message.StockId;

                        _context.IntraDayPrices.Add(intraDayPrice);
                    }
                }
                else
                {
                    var existingPrice = _context.DailyPrices
                        .Any(p => p.StockId == message.StockId
                             && p.TransDate == price.TransDate);

                    DailyPrice dailyPrice = _mapper.Map<DailyPrice>(price);
                    dailyPrice.StockId = message.StockId;

                    _context.DailyPrices.Add(dailyPrice);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            //Recalculate article performance
            await this.UpdateArticlePerformance(message.StockId, cancellationToken);

            //TODO Review this - does this break the pattern?
            Stocks.DetailsHandler handler = new Stocks.DetailsHandler(_context, _mapper);
            Stocks.DetailsQuery detailsQuery = new DetailsQuery(message.StockId);
            StockEnvelopeDTO dto = await handler.Handle(detailsQuery, cancellationToken);

            return dto;
        }

        protected async Task UpdateArticlePerformance(string stockId, CancellationToken cancellationToken)
        {
            var articles = await _context.Articles
                .Where(a => a.StockId == stockId)
                .ToListAsync();

            foreach (var article in articles)
            {
                article.PriceTMinus1 = await this.GetClosingPrice(stockId, article.PublicationDate.AddDays(-1));
                article.PriceTMinus7 = await this.GetClosingPrice(stockId, article.PublicationDate.AddDays(-7));
                article.PriceOpen = await this.GetOpeningPrice(stockId, article.PublicationDate);
                article.PriceClose = await this.GetClosingPrice(stockId, article.PublicationDate);
                article.PriceTPlus1 = await this.GetClosingPrice(stockId, article.PublicationDate.AddDays(1));
                article.PriceTPlus2 = await this.GetClosingPrice(stockId, article.PublicationDate.AddDays(2));
                article.PriceTPlus3 = await this.GetClosingPrice(stockId, article.PublicationDate.AddDays(3));
                article.PriceTPlus5 = await this.GetClosingPrice(stockId, article.PublicationDate.AddDays(5));
                article.PriceTPlus7 = await this.GetClosingPrice(stockId, article.PublicationDate.AddDays(7));
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        //TODO Move this elsewhere
        protected async Task<decimal> GetOpeningPrice(string stockId, DateTime date)
        {
            var price = await this.GetPrice(stockId, date);
            return price == null ? 0 : price.Open;
        }

        //TODO Move this elsewhere
        protected async Task<decimal> GetClosingPrice(string stockId, DateTime date)
        {
            var price = await this.GetPrice(stockId, date);
            return price == null ? 0 : price.Close;
        }

        protected async Task<DailyPrice> GetPrice(string stockId, DateTime date)
        {
            var price = await _context.DailyPrices
                .Where(p => p.StockId == stockId
                    && p.TransDate >= date)
                .OrderBy(p => p.TransDate)
                .FirstOrDefaultAsync();

            return price;
        }
    }
}
