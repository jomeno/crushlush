using Crushlush.Core.Managers;
using Crushlush.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crushlush.Web.Controllers
{
    public class HomeController : Controller
    {
        Managers managers;
        PlaylistManager playlistManager;

        public HomeController()
        {
            managers = new Managers();
            playlistManager = managers.PlaylistManager;
        }

        public ActionResult Index()
        {
            var model = new Dashboard();

            // get playlist collection
            var getPlaylists = playlistManager.GetPlaylists();
            if (getPlaylists.Succeeded)
            {
                model.Playlists = getPlaylists.Result;
                var firstPlaylist = model.Playlists.FirstOrDefault();

                if (firstPlaylist != null)
                {
                    model.Playlist = firstPlaylist;

                    // get tracks of first playlist
                    var getPlaylistTracks = playlistManager.GetPlaylistTracks(firstPlaylist.PlaylistID);
                    if (getPlaylists.Succeeded) model.Playlist.Tracks = getPlaylistTracks.Result;
                }
            }

            return View(model);
        }

    }
}