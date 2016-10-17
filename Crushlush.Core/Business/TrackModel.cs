using Crushlush.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crushlush.Core.Business
{
    public class TrackModel : Model
    {
        public int TrackID { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }

        public TrackModel() { }

        public TrackModel(Track entity)
        {
            this.Assign(entity);
        }

        public Track Create()
        {
            var entity = new Track();
            entity.Number = entity.Number;
            entity.Name = this.Name;
            entity.URL = this.URL;
            entity.CreatedAt = DateTime.Now;
            entity.IsActive = true;
            return entity;
        }

        public void Update(Track entity)
        {
            entity.Number = entity.Number;
            entity.Name = this.Name;
            entity.URL = this.URL;
            entity.ModifiedAt = DateTime.Now;
            entity.IsActive = this.IsActive;
        }
    }
}
