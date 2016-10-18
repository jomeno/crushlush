using Crushlush.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crushlush.Core.Business
{
    public class PlaylistModel : Model
    {
        public int PlaylistID { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        public List<TrackModel> Tracks { get; set; }

        public PlaylistModel() { }

        public PlaylistModel(Playlist entity)
        {
            this.Assign(entity);
        }

        public Playlist Create()
        {
            var entity = new Playlist();
            entity.Name = this.Name;
            entity.IsActive = true;
            entity.CreatedAt = DateTime.Now;
            return entity;
        }

        public void Update(Playlist entity)
        {
            entity.Name = this.Name;
            entity.IsActive = true;
            entity.ModifiedAt = DateTime.Now;
        }
    }
}
