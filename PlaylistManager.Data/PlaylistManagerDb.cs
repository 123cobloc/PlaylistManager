using Microsoft.EntityFrameworkCore;
using PlaylistManager.Data.ToPlaylistManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistManager.Data
{
    public class PlaylistManagerDb : DbContext
    {
        public PlaylistManagerDb(DbContextOptions<PlaylistManagerDb> options) : base(options) { }

        public DbSet<Watchlist> Watchlist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Watchlist>()
                .HasKey(w => new { w.UserId, w.ItemId, w.ItemType });
        }
    }


}
