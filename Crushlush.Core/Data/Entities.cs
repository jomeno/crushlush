using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crushlush.Core.Data
{
    public partial class Entities : DbContext
    {
        static Entities()
        {
            Database.SetInitializer<Entities>(null);
        }

        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }

    }
}
