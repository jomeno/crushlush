namespace Crushlush.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
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
                "dbo.PlaylistTracks",
                c => new
                    {
                        PlaylistTrackID = c.Int(nullable: false, identity: true),
                        PlaylistID = c.Int(nullable: false),
                        TrackID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlaylistTrackID)
                .ForeignKey("dbo.Tracks", t => t.TrackID, cascadeDelete: false)
                .ForeignKey("dbo.Playlists", t => t.PlaylistID, cascadeDelete: false)
                .Index(t => t.PlaylistID)
                .Index(t => t.TrackID);
            
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
                    })
                .PrimaryKey(t => t.TrackID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlaylistTracks", "PlaylistID", "dbo.Playlists");
            DropForeignKey("dbo.PlaylistTracks", "TrackID", "dbo.Tracks");
            DropIndex("dbo.PlaylistTracks", new[] { "TrackID" });
            DropIndex("dbo.PlaylistTracks", new[] { "PlaylistID" });
            DropTable("dbo.Tracks");
            DropTable("dbo.PlaylistTracks");
            DropTable("dbo.Playlists");
        }
    }
}
