interface Model {
    IsActive: boolean;
}

interface Operation<T> {
    Succeeded: boolean;
    Result: T;
    Message: string;
}

interface Dashboard {
    Playlist: Playlist;
    Playlists: Playlist[];
}

interface Playlist {
    PlaylistID: number;
    Name: string;
    Tracks: Track[];

    IsEditing: boolean;
}

interface Track {
    Name: string;
    Number: number;
    Duration: number;
    URL: string;

}