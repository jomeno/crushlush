using Crushlush.Core.Business;
using Crushlush.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public Operation<PlaylistModel> GetPlaylists(int playlistId)
        {
            var operation = Operation.Create(() =>
            {
                var playlist = (from plist in _db.Playlists where plist.PlaylistID == playlistId select plist).Include("PlaylistTracks.Track").FirstOrDefault();
                if (playlist == null) throw new Exception("Playlist not found");

                var playlistModel = new PlaylistModel(playlist);
                var trackModels = playlist.PlaylistTracks.Select(plistTrack =>
                {
                    var track = new TrackModel(plistTrack.Track);
                    return track;
                }).ToList();

                playlistModel.Tracks = trackModels;

                return playlistModel;
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
            // 1. create a playlist if it doesn't already exist,
            // 2. create the tracks if they don't exist
            // 3. add tracks to playlisttracks

            var operation = Operation.Create(() =>
            {
                // 1. create a playlist if it doesn't already exist,
                var playlist = (from plist in _db.Playlists where plist.Name == model.Name select plist).FirstOrDefault();
                if (playlist != null) throw new Exception(string.Format("A playlist named {0} already exists", model.Name));

                var newPlaylist = model.Create();
                _db.Playlists.Add(newPlaylist);

                // 2. create the tracks if they don't exist TODO: requires enhancement
                var newTracks = model.Tracks.Select(t => t.Create()).ToList();
                _db.Tracks.AddRange(newTracks);

                // persist changes to database
                _db.SaveChanges();

                // 3. add tracks to playlisttracks
                var playlistTracks = newTracks.Select(t =>
                {
                    var playlistTrack = new PlaylistTrack()
                    {
                        PlaylistID = newPlaylist.PlaylistID,
                        TrackID = t.TrackID
                    };

                    return playlistTrack;

                }).ToList();
                _db.PlaylistTracks.AddRange(playlistTracks);

                // persist changes to database
                _db.SaveChanges();

                // assign id of newly created playlist to the model
                model.PlaylistID = newPlaylist.PlaylistID;
                model.Tracks = newTracks.Select(t => { return new TrackModel(t); }).ToList();

                return model;
            });
            return operation;
        }

        //public Operation<PlaylistTrackModel> CreatePlaylistTrack(PlaylistTrackModel model)
        //{
        //    var operation = Operation.Create(() =>
        //    {
        //        if (model.PlaylistID == 0)
        //        {
        //            // create a new playlist first before creating a playlist track

        //        }

        //        var playlistTrack = (from plistTrack in _db.PlaylistTracks where plistTrack.PlaylistID == model.PlaylistID && plistTrack.TrackID == model.TrackID select plistTrack).FirstOrDefault();
        //        if (playlistTrack != null) throw new Exception("This track is already in your playlist");

        //        var newPlaylistTrack = model.Create();
        //        _db.PlaylistTracks.Add(newPlaylistTrack);

        //        // persist changes to database
        //        _db.SaveChanges();

        //        // assign id of newly created playlistTrack to the model
        //        model.PlaylistTrackID = newPlaylistTrack.PlaylistTrackID;

        //        return model;
        //    });
        //    return operation;
        //}

        #endregion

        #region Update Operations

        public Operation<PlaylistModel> UpdatePlaylist(PlaylistModel model)
        {
            var operation = Operation.Create(() =>
            {
                var playlist = (from plist in _db.Playlists where plist.PlaylistID == model.PlaylistID select plist).Include("PlaylistTracks.Track").FirstOrDefault();
                if (playlist == null) throw new Exception("Playlist not found");

                model.Update(playlist);

                // create any new tracks if they don't exist
                //TODO: requires enhancement

                var newTracks = new List<Track>();
                model.Tracks.ForEach(t =>
                {
                    // if track id is 0 then it must be a new track
                    if (t.TrackID == 0)
                    {
                        newTracks.Add(t.Create());
                    }
                });

                // only add if tehre are any tracks
                if (newTracks.Count > 0) _db.Tracks.AddRange(newTracks);

                // update any changes to existing tracks
                var tracks = playlist.PlaylistTracks.Select(plistTrack => { return plistTrack.Track; }).ToList();
                tracks.ForEach(track =>
                {
                    var trackModel = model.Tracks.Where(trackM => trackM.TrackID == track.TrackID).FirstOrDefault();
                    if (trackModel != null)
                    {
                        trackModel.Update(track);
                    }
                });

                // persist changes to database
                _db.SaveChanges();

                // add newly added tracks to playlisttracks
                var playlistTracks = newTracks.Select(t =>
                {
                    var playlistTrack = new PlaylistTrack()
                    {
                        PlaylistID = playlist.PlaylistID,
                        TrackID = t.TrackID
                    };

                    return playlistTrack;

                }).ToList();
                _db.PlaylistTracks.AddRange(playlistTracks);

                // persist changes to database
                _db.SaveChanges();

                // get all tracks afresh
                var allTracks = (from plist in _db.PlaylistTracks
                                 where plist.PlaylistID == model.PlaylistID
                                 join track in _db.Tracks on plist.TrackID equals track.TrackID
                                 select track).ToList();

                // project out trackmodel
                model.Tracks = allTracks.Select(t => { return new TrackModel(t); }).ToList();

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

                // persist changes to database
                _db.SaveChanges();

                return model;
            });
            return operation;
        }


        #endregion

        #region Delete Operations

        public Operation<int> DeletePlaylist(int playlistId)
        {
            var operation = Operation.Create(() =>
            {
                var playlist = (from plist in _db.Playlists where plist.PlaylistID == playlistId select plist).FirstOrDefault();
                if (playlist == null) throw new Exception("Playlist not found.");

                // remove specified playlist
                _db.Playlists.Remove(playlist);

                // persist changes to database
                _db.SaveChanges();

                return playlistId;
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

                // persist changes to database
                _db.SaveChanges();

                return model;
            });
            return operation;
        }

        #endregion
    }
}
