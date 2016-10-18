class HomeController {
    data: DataService;
    Dashboard: Dashboard;
    playlistHolder: Playlist;


    constructor(_data: DataService) {
        this.data = _data;

        this.Dashboard = JSON.parse(angular.element("#model").html());
    }

    // prepare to create a new playlist
    createPlaylist() {
        // temporarily hold the active playlist
        this.playlistHolder = angular.copy(this.Dashboard.Playlist);

        this.Dashboard.Playlist = <Playlist>{ Tracks: [<Track>{}], IsEditing: true };
    }

    // cancel playlist create mode
    cancelCreatePlaylist() {
        // restore any help playlist
        this.Dashboard.Playlist = angular.copy(this.playlistHolder);

        this.Dashboard.Playlist.IsEditing = !this.Dashboard.Playlist.IsEditing;
        this.Dashboard.Playlist = this.Dashboard.Playlists[0];
    }

    // prepare or toggle to playlist edit mode
    editPlaylist() {
        this.Dashboard.Playlist.IsEditing = !this.Dashboard.Playlist.IsEditing
    }

    // prepare to create a new track
    createTrack() {
        if (this.Dashboard.Playlist == null) {
            this.data.toast.pop(true, <any>{ Message: 'First, create a new playlist to add tracks' });
            return false;
        }

        if (this.Dashboard.Playlist.Tracks.length == 10) {
            this.data.toast.pop(true, <any>{ Message: 'Hey slow down, too much spice spoil the broth' });
            return false;
        }


        this.Dashboard.Playlist.Tracks.push(<Track>{});
        this.Dashboard.Playlist.IsEditing = true;
    }

    // save or update playlist
    savePlaylist() {
        if (this.Dashboard.Playlist == null) {
            this.data.toast.pop(true, <any>{ Message: 'First, create a new playlist to add tracks' });
            return false;
        }

        // if PlaylistID is 0, create a new playlist otherwise update existing playlist
        if (this.Dashboard.Playlist.PlaylistID == 0 || this.Dashboard.Playlist.PlaylistID == null) {

            // fire off call to create new playlist in db
            this.data.post<Playlist>(this.data.Api.Playlists, this.Dashboard.Playlist).then(result=> {
                this.Dashboard.Playlists.push(result);
                this.Dashboard.Playlist.IsEditing = false;
            }).catch(e=> {
                // TODO: something really bad happened, send to error log, possbily call admin attention
                // console.log(e)
            });
        } else {

            // fire off call to update playlist in db
            this.data.put<Playlist>(this.data.Api.Playlists, this.Dashboard.Playlist).then(result=> {
                // update playlist array
                for (var i = 0; i < this.Dashboard.Playlists.length; i++) {
                    var playlist = this.Dashboard.Playlists[i];
                    if (playlist.PlaylistID == result.PlaylistID) {
                        this.Dashboard.Playlists[i] = result;
                    }
                }

                this.Dashboard.Playlist.IsEditing = false;

            }).catch(e=> {
                // TODO: something really bad happened, send to error log, possbily call admin attention
                // console.log(e)
            });
        }


    }


}

playlist.controller("HomeController", HomeController);