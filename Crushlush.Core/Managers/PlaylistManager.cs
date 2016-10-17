using Crushlush.Core.Business;
using Crushlush.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crushlush.Core.Managers
{
    public class PlaylistManager
    {
        private Entities _db;

        public PlaylistManager(Entities db)
        {
            _db = db;
        }

        #region Fetch Operation

        public Operation<List<PlaylistModel>> GetPlaylists()
        {
            // fetch all playlists

            var operation = Operation.Create(() =>
            {
                var playlists = (from playlist in _db.Playlists select playlist).ToList();
                if (playlists == null) throw new Exception("Failed to retrieve playlists.");
                var playlistModels = playlists.Select(p =>
                {
                    return new PlaylistModel(p);
                }).ToList();

                return playlistModels;
            });

            return operation;
        }

        public Operation<List<TrackModel>> GetPlaylistTracks(int playlistId)
        {
            // fetch trakcs by playlists

            var operation = Operation.Create(() =>
            {
                var tracks = (from playlistTrack in _db.PlaylistTracks
                              where playlistTrack.PlaylistID == playlistId
                              join track in _db.Tracks on playlistTrack.TrackID equals track.TrackID
                              select track).ToList();

                if (tracks == null) throw new Exception("Failed to retrieve tracks.");

                // transform track entities to models
                var trackModels = tracks.Select(t =>
                {
                    return new TrackModel(t);
                }).ToList();

                return trackModels;
            });

            return operation;
        }

        #endregion

        #region Create Operations

        public Operation<PlaylistModel> CreatePlaylist(PlaylistModel model)
        {
            var operation = Operation.Create(() =>
            {
                var playlist = (from plist in _db.Playlists where plist.Name == model.Name select plist).FirstOrDefault();
                if (playlist != null) throw new Exception(string.Format("A playlist named {0} already exists", model.Name));

                var newPlaylist = model.Create();
                _db.Playlists.Add(newPlaylist);

                // persist changes to db
                _db.SaveChanges();

                // assign id of newly created playlist to the model
                model.PlaylistID = newPlaylist.PlaylistID;

                return model;
            });
            return operation;
        }

        public Operation<PlaylistTrackModel> CreatePlaylistTrack(PlaylistTrackModel model)
        {
            var operation = Operation.Create(() =>
            {
                if (model.PlaylistID == 0)
                {
                    // create a new playlist first before creating a playlist track

                }


                var playlistTrack = (from plistTrack in _db.PlaylistTracks where plistTrack.PlaylistID == model.PlaylistID && plistTrack.TrackID == model.TrackID select plistTrack).FirstOrDefault();
                if (playlistTrack != null) throw new Exception("This track is already in your playlist");

                var newPlaylistTrack = model.Create();
                _db.PlaylistTracks.Add(newPlaylistTrack);

                // persist changes to db
                _db.SaveChanges();

                // assign id of newly created playlistTrack to the model
                model.PlaylistTrackID = newPlaylistTrack.PlaylistTrackID;

                return model;
            });
            return operation;
        }

        #endregion

        #region Update Operations

        public Operation<PlaylistModel> UpdatePlaylist(PlaylistModel model)
        {
            var operation = Operation.Create(() =>
            {
                var playlist = (from plist in _db.Playlists where plist.PlaylistID == model.PlaylistID select plist).FirstOrDefault();
                if (playlist == null) throw new Exception("Playlist not found");

                model.Update(playlist);

                // persist changes to db
                _db.SaveChanges();

                return model;
            });
            return operation;
        }

        public Operation<TrackModel> UpdateTrack(TrackModel model)
        {
            var operation = Operation.Create(() =>
            {
                var track = (from trk in _db.Tracks where trk.TrackID == model.TrackID select trk).FirstOrDefault();
                if (track == null) throw new Exception("Track not found");

                model.Update(track);

                // persist changes to db
                _db.SaveChanges();

                return model;
            });
            return operation;
        }


        #endregion

        #region Delete Operations

        public Operation<PlaylistModel> DeletePlaylist(PlaylistModel model)
        {
            var operation = Operation.Create(() =>
            {
                var playlist = (from plist in _db.Playlists where plist.PlaylistID == model.PlaylistID select plist).FirstOrDefault();
                if (playlist == null) throw new Exception("Playlist not found.");

                // remove specified playlist
                _db.Playlists.Remove(playlist);

                // persist changes to db
                _db.SaveChanges();

                return model;
            });
            return operation;
        }

        public Operation<PlaylistTrackModel> DeletePlaylistTrack(PlaylistTrackModel model)
        {
            var operation = Operation.Create(() =>
            {
                var playlistTrack = (from plistTrack in _db.PlaylistTracks where plistTrack.PlaylistID == model.PlaylistID && plistTrack.TrackID == model.TrackID select plistTrack).FirstOrDefault();
                if (playlistTrack == null) throw new Exception("Playlist track not found.");

                // remove specified playlist track
                _db.PlaylistTracks.Remove(playlistTrack);

                // persist changes to db
                _db.SaveChanges();

                return model;
            });
            return operation;
        }

        #endregion
    }
}
