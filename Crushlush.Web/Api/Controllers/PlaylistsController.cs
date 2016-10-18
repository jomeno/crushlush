using Crushlush.Core.Business;
using Crushlush.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Crushlush.Web.Api.Controllers
{
    public class PlaylistsController : ApiController
    {
        Managers managers;
        PlaylistManager playlistManager;

        public PlaylistsController()
        {
            managers = new Managers();
            playlistManager = managers.PlaylistManager;
        }


        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(int id)
        {
            var playlistId = id;
            var operation = playlistManager.GetPlaylists(playlistId);
            return Ok(operation);
        }

        // POST api/<controller>
        public IHttpActionResult Post(PlaylistModel model)
        {
            var operaiton = new Operation();
            if (ModelState.IsValid == false)
            {
                operaiton.Message = "Please complete playlist details";
            }

            var operation = playlistManager.CreatePlaylist(model);

            if (operation.Succeeded) operation.Message = "Yay! Your new playlist is ready.";
            return Ok(operation);
        }

        // PUT api/<controller>/5
        public IHttpActionResult Put(PlaylistModel model)
        {
            var operaiton = new Operation();
            if (ModelState.IsValid == false)
            {
                operaiton.Message = "Please complete playlist details";
            }

            var operation = playlistManager.UpdatePlaylist(model);

            if (operation.Succeeded) operation.Message = "Your changes have been saved.";
            return Ok(operation);
        }

        // DELETE api/<controller>/5
        public IHttpActionResult Delete(int id)
        {
            var playlistId = id;
            var operation = playlistManager.DeletePlaylist(playlistId);
            if (operation.Succeeded) operation.Message = "Playlist was deleted successfully.";
            return Ok(operation);
        }
    }
}