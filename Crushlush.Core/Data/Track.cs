using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crushlush.Core.Data
{
    public class Track
    {
        [Key]
        public int TrackID { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
