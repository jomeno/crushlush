using Crushlush.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crushlush.Core.Business
{
    public class PlaylistTrackModel : Model
    {
        public int PlaylistTrackID { get; set; }
        public int PlaylistID { get; set; }
        public int TrackID { get; set; }
        public List<TrackModel> Tracks { get; set; }

        public PlaylistTrackModel() { }

        public PlaylistTrackModel(PlaylistTrack entity)
        {
            this.Assign(entity);
        }

        public PlaylistTrack Create()
        {
            var entity = new PlaylistTrack();
            entity.PlaylistID = this.PlaylistID;
            entity.TrackID = this.TrackID;
            return entity;
        }

        public void Update(PlaylistTrack entity)
        {
            entity.PlaylistID = this.PlaylistID;
            entity.TrackID = this.TrackID;
        }
    }
}
