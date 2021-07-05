using Microsoft.EntityFrameworkCore;
using Cards.Infra.Models;

namespace Cards.Infra.Context
{
    public class CardContext : DbContext
    {
        public CardContext(DbContextOptions<CardContext> options) : base(options) { }

        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>().HasAlternateKey(c => new { c.CostumerId, c.CardNumber });
        }
    }
}
