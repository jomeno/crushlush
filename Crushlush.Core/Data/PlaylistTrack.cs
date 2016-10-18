using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crushlush.Core.Data
{
    public class PlaylistTrack
    {
        public int PlaylistTrackID { get; set; }
        public int PlaylistID { get; set; }
        public int TrackID { get; set; }
        public Track Track { get; set; }

    }
}
