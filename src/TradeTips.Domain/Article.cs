using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TradeTips.Domain
{
    //TODO Add Lazy loading
    public class Article
    {
        #region Fields
        public int ArticleId { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Teaser { get; set; }
        public string Summary { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Uri Link { get; set; }
        public Uri StockLink { get; set; }
        public string StockId { get; set; }
        public bool IsAlpha { get; set; }

        [Column(TypeName = "decimal(12, 5)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(12, 5)")]
        public decimal? PriceTMinus7 { get; set; }
        [Column(TypeName = "decimal(12, 5)")]
        public decimal? PriceTMinus1 { get; set; }
        [Column(TypeName = "decimal(12, 5)")]
        public decimal? PriceOpen { get; set; }
        [Column(TypeName = "decimal(12, 5)")]
        public decimal? PriceClose { get; set; }
        [Column(TypeName = "decimal(12, 5)")]
        public decimal? PriceTPlus1 { get; set; }
        [Column(TypeName = "decimal(12, 5)")]
        public decimal? PriceTPlus2 { get; set; }
        [Column(TypeName = "decimal(12, 5)")]
        public decimal? PriceTPlus3 { get; set; }
        [Column(TypeName = "decimal(12, 5)")]
        public decimal? PriceTPlus5 { get; set; }
        [Column(TypeName = "decimal(12, 5)")]
        public decimal? PriceTPlus7 { get; set; }

        #endregion

        #region Calculated
        [NotMapped]
        public bool Favorited => ArticleFavorites?.Any() ?? false;

        [NotMapped]
        public int FavoritesCount => ArticleFavorites?.Count ?? 0;

        [NotMapped]
        public List<string> TagList => (ArticleTags?.Select(x => x.TagId) ?? Enumerable.Empty<string>()).ToList();
        #endregion

        #region Relations
        public virtual Stock Stock { get; }

        public Person Author { get; set; }

        public List<Comment> Comments { get; set; }

        public List<ArticleTag> ArticleTags { get; set; }

        public List<ArticleFavorite> ArticleFavorites { get; set; }
        #endregion
    }
}