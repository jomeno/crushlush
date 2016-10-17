namespace Crushlush.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigrations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Playlists",
                c => new
                    {
                        PlaylistID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        ModifiedAt = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PlaylistID);
            
            CreateTable(
                "dbo.Tracks",
                c => new
                    {
                        TrackID = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Name = c.String(),
                        URL = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        ModifiedAt = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        Playlist_PlaylistID = c.Int(),
                        PlaylistTrack_PlaylistTrackID = c.Int(),
                    })
                .PrimaryKey(t => t.TrackID)
                .ForeignKey("dbo.Playlists", t => t.Playlist_PlaylistID)
                .ForeignKey("dbo.PlaylistTracks", t => t.PlaylistTrack_PlaylistTrackID)
                .Index(t => t.Playlist_PlaylistID)
                .Index(t => t.PlaylistTrack_PlaylistTrackID);
            
            CreateTable(
                "dbo.PlaylistTracks",
                c => new
                    {
                        PlaylistTrackID = c.Int(nullable: false, identity: true),
                        PlaylistID = c.Int(nullable: false),
                        TrackID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlaylistTrackID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tracks", "PlaylistTrack_PlaylistTrackID", "dbo.PlaylistTracks");
            DropForeignKey("dbo.Tracks", "Playlist_PlaylistID", "dbo.Playlists");
            DropIndex("dbo.Tracks", new[] { "PlaylistTrack_PlaylistTrackID" });
            DropIndex("dbo.Tracks", new[] { "Playlist_PlaylistID" });
            DropTable("dbo.PlaylistTracks");
            DropTable("dbo.Tracks");
            DropTable("dbo.Playlists");
        }
    }
}
