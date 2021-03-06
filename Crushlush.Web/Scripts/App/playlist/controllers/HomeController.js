var HomeController = (function () {
    function HomeController(_data) {
        this.data = _data;
        this.Dashboard = JSON.parse(angular.element("#model").html());
    }
    // prepare to create a new playlist
    HomeController.prototype.createPlaylist = function () {
        // temporarily hold the active playlist
        this.playlistHolder = angular.copy(this.Dashboard.Playlist);
        this.Dashboard.Playlist = { Tracks: [{}], IsEditing: true };
    };
    // cancel playlist create mode
    HomeController.prototype.cancelCreatePlaylist = function () {
        // restore any help playlist
        this.Dashboard.Playlist = angular.copy(this.playlistHolder);
        this.Dashboard.Playlist.IsEditing = !this.Dashboard.Playlist.IsEditing;
        this.Dashboard.Playlist = this.Dashboard.Playlists[0];
    };
    // prepare or toggle to playlist edit mode
    HomeController.prototype.editPlaylist = function () {
        this.Dashboard.Playlist.IsEditing = !this.Dashboard.Playlist.IsEditing;
    };
    // prepare to create a new track
    HomeController.prototype.createTrack = function () {
        if (this.Dashboard.Playlist == null) {
            this.data.toast.pop(true, { Message: 'First, create a new playlist to add tracks' });
            return false;
        }
        if (this.Dashboard.Playlist.Tracks.length == 10) {
            this.data.toast.pop(true, { Message: 'Hey slow down, too much spice spoil the broth' });
            return false;
        }
        this.Dashboard.Playlist.Tracks.push({});
        this.Dashboard.Playlist.IsEditing = true;
    };
    // save or update playlist
    HomeController.prototype.savePlaylist = function () {
        var _this = this;
        if (this.Dashboard.Playlist == null) {
            this.data.toast.pop(true, { Message: 'First, create a new playlist to add tracks' });
            return false;
        }
        // if PlaylistID is 0, create a new playlist otherwise update existing playlist
        if (this.Dashboard.Playlist.PlaylistID == 0 || this.Dashboard.Playlist.PlaylistID == null) {
            // fire off call to create new playlist in db
            this.data.post(this.data.Api.Playlists, this.Dashboard.Playlist).then(function (result) {
                _this.Dashboard.Playlists.push(result);
                _this.Dashboard.Playlist = result;
                console.log(_this.Dashboard.Playlist);
                _this.Dashboard.Playlist.IsEditing = false;
            }).catch(function (e) {
                // TODO: something really bad happened, send to error log, possbily call admin attention
                // console.log(e)
            });
        }
        else {
            // fire off call to update playlist in db
            this.data.put(this.data.Api.Playlists, this.Dashboard.Playlist).then(function (result) {
                // update playlist array
                for (var i = 0; i < _this.Dashboard.Playlists.length; i++) {
                    var playlist = _this.Dashboard.Playlists[i];
                    if (playlist.PlaylistID == result.PlaylistID) {
                        _this.Dashboard.Playlists[i] = result;
                    }
                }
                _this.Dashboard.Playlist.IsEditing = false;
            }).catch(function (e) {
                // TODO: something really bad happened, send to error log, possbily call admin attention
                // console.log(e)
            });
        }
    };
    // fetch playlist tracks
    HomeController.prototype.selectPlaylist = function (playlist) {
        var _this = this;
        if (playlist == null) {
            this.data.toast.pop(true, { Message: 'Select a playlist to view tracks' });
            return false;
        }
        this.data.get(this.data.Api.Playlist([playlist.PlaylistID])).then(function (result) {
            _this.Dashboard.Playlist = result;
        }).catch(function (e) {
            // TODO: something really bad happened, send to error log, possbily call admin attention
            // console.log(e)
        });
    };
    // delete a playlist
    HomeController.prototype.deletePlaylist = function (playlist) {
        var _this = this;
        if (playlist == null) {
            this.data.toast.pop(true, { Message: 'Select a playlist to delete' });
            return false;
        }
        this.data.delete(this.data.Api.DeletePlaylist([playlist.PlaylistID])).then(function (result) {
            // remove playlist from Playlist array
            for (var i = 0; i < _this.Dashboard.Playlists.length; i++) {
                var plist = _this.Dashboard.Playlists[i];
                if (plist.PlaylistID == _this.Dashboard.Playlist.PlaylistID) {
                    _this.Dashboard.Playlists.splice(i, 1);
                }
            }
            _this.Dashboard.Playlist = _this.Dashboard.Playlists[0];
            if (_this.Dashboard.Playlist != null) {
                // retrieve playlist tracks
                _this.selectPlaylist(_this.Dashboard.Playlist);
            }
        }).catch(function (e) {
            // TODO: something really bad happened, send to error log, possbily call admin attention
            // console.log(e)
        });
    };
    return HomeController;
})();
playlist.controller("HomeController", HomeController);
//# sourceMappingURL=HomeController.js.map