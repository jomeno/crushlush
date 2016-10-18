using Crushlush.Core.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crushlush.Web.Models
{
    public class Dashboard : Model
    {
        public PlaylistModel Playlist { get; set; }
        public List<PlaylistModel> Playlists { get; set; }

        public Dashboard()
        {
            Playlists = new List<PlaylistModel>();
        }

    }
}