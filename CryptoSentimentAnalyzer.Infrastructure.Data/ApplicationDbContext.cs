using CryptoSentimentAnalyzer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Coin> Coins { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<SentimentResult> SentimentResults { get; set; }
        public DbSet<SentimentSummary> SentimentSummaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Coin entity
            modelBuilder.Entity<Coin>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Symbol).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(1000);
            });

            // Configure Tweet entity
            modelBuilder.Entity<Tweet>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Text).IsRequired();
                entity.Property(e => e.Author).IsRequired().HasMaxLength(100);
                entity.HasOne(e => e.Coin)
                      .WithMany()
                      .HasForeignKey(e => e.CoinId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure SentimentResult entity
            modelBuilder.Entity<SentimentResult>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Coin)
                      .WithMany(c => c.SentimentResults)
                      .HasForeignKey(e => e.CoinId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Tweet)
                      .WithOne(t => t.SentimentResult)
                      .HasForeignKey<SentimentResult>(e => e.TweetId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure SentimentSummary entity
            modelBuilder.Entity<SentimentSummary>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Coin)
                      .WithMany()
                      .HasForeignKey(e => e.CoinId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed some initial coin data
            modelBuilder.Entity<Coin>().HasData(
                new Coin { Id = 1, Symbol = "BTC", Name = "Bitcoin", Description = "The first cryptocurrency", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Coin { Id = 2, Symbol = "ETH", Name = "Ethereum", Description = "A decentralized software platform", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Coin { Id = 3, Symbol = "BNB", Name = "Binance Coin", Description = "Cryptocurrency used on the Binance exchange", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );
        }
    }
}
