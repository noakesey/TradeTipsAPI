using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Threading.Tasks;

namespace TradeTips.Domain
{
    public class TradeTipsContext : DbContext
    {
        public TradeTipsContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<ArticleFavorite> ArticleFavorites { get; set; }
        public DbSet<FollowedPeople> FollowedPeople { get; set; }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<DailyPrice> DailyPrices { get; set; }
        public DbSet<IntraDayPrice> IntraDayPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .Property(p => p.ArticleId)
                .ValueGeneratedNever();

            modelBuilder.Entity<ArticleTag>(b =>
            {
                b.HasKey(t => new { t.ArticleId, t.TagId });

                b.HasOne(pt => pt.Article)
                .WithMany(p => p.ArticleTags)
                .HasForeignKey(pt => pt.ArticleId);

                b.HasOne(pt => pt.Tag)
                .WithMany(t => t.ArticleTags)
                .HasForeignKey(pt => pt.TagId);
            });

            modelBuilder.Entity<ArticleFavorite>(b =>
            {
                b.HasKey(t => new { t.ArticleId, t.PersonId });

                b.HasOne(pt => pt.Article)
                    .WithMany(p => p.ArticleFavorites)
                    .HasForeignKey(pt => pt.ArticleId);

                b.HasOne(pt => pt.Person)
                    .WithMany(t => t.ArticleFavorites)
                    .HasForeignKey(pt => pt.PersonId);
            });

            modelBuilder.Entity<FollowedPeople>(b =>
            {
                b.HasKey(t => new { t.ObserverId, t.TargetId });

                // we need to add OnDelete RESTRICT otherwise for the SqlServer database provider, 
                b.HasOne(pt => pt.Observer)
                    .WithMany(p => p.Followers)
                    .HasForeignKey(pt => pt.ObserverId)
                    .OnDelete(DeleteBehavior.Restrict);

                // we need to add OnDelete RESTRICT otherwise for the SqlServer database provider, 
                b.HasOne(pt => pt.Target)
                    .WithMany(t => t.Following)
                    .HasForeignKey(pt => pt.TargetId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            #region Indexes
            modelBuilder.Entity<Article>()
                .HasIndex(i => i.PublicationDate);

            modelBuilder.Entity<DailyPrice>()
                .HasIndex(i => i.TransDate);

            modelBuilder.Entity<IntraDayPrice>()
                .HasIndex(i => i.TransDate);

            #endregion
        }

        #region Transaction support
        private IDbContextTransaction _currentTransaction;

        public void BeginTransaction()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            if (!Database.IsInMemory())
            {
                _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
            }
        }

        public void CommitTransaction()
        {
            try
            {
                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        #endregion
    }
}
