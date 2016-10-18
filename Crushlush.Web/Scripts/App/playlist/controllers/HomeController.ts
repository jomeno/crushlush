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
                this.Dashboard.Playlist = result;
                console.log(this.Dashboard.Playlist)
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

    // fetch playlist tracks
    selectPlaylist(playlist: Playlist) {
        if (playlist == null) {
            this.data.toast.pop(true, <any>{ Message: 'Select a playlist to view tracks' });
            return false;
        }

        this.data.get<Playlist>(this.data.Api.Playlist([playlist.PlaylistID])).then(result => {
            this.Dashboard.Playlist = result;

        }).catch(e=> {
            // TODO: something really bad happened, send to error log, possbily call admin attention
            // console.log(e)
        });
    }

    // delete a playlist
    deletePlaylist(playlist: Playlist) {
        if (playlist == null) {
            this.data.toast.pop(true, <any>{ Message: 'Select a playlist to delete' });
            return false
        }

        this.data.delete<number>(this.data.Api.DeletePlaylist([playlist.PlaylistID])).then(result=> {

            // remove playlist from Playlist array
            for (var i = 0; i < this.Dashboard.Playlists.length; i++) {
                var plist = this.Dashboard.Playlists[i];
                if (plist.PlaylistID == this.Dashboard.Playlist.PlaylistID) {
                    this.Dashboard.Playlists.splice(i, 1);
                }
            }

            this.Dashboard.Playlist = this.Dashboard.Playlists[0];
            if (this.Dashboard.Playlist != null) {
                // retrieve playlist tracks
                this.selectPlaylist(this.Dashboard.Playlist);
            }

        }).catch(e=> {
            // TODO: something really bad happened, send to error log, possbily call admin attention
            // console.log(e)
        });
    }

}

playlist.controller("HomeController", HomeController);