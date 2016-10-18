using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crushlush.Core.Data
{
    public class Playlist
    {
        [Key]
        public int PlaylistID { get; set; }
        public string Name { get; set; }        
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; }
        public ICollection<PlaylistTrack> PlaylistTracks { get; set; }
    }
}
