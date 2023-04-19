using Microsoft.EntityFrameworkCore;
using PlaylistManager.Data.ToPlaylistManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistManager.Data
{
    public class PlaylistManagerCosmos : DbContext
    {
        public PlaylistManagerCosmos(DbContextOptions<PlaylistManagerCosmos> options) : base(options) { }

        public DbSet<Watchlist> Watchlist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Watchlist>()
                .ToContainer("Watchlist")
                .HasPartitionKey(w => w.UserId)
                .HasKey(w => new { w.UserId, w.ItemId, w.ItemType });
        }
    }
}
