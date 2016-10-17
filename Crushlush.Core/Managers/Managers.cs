using Crushlush.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crushlush.Core.Managers
{
    public class Managers
    {
        public Entities _db;
        private PlaylistManager playlistManager;

        public Managers()
        {
            _db = new Entities();
        }

        public PlaylistManager PlaylistManager
        {
            get
            {
                if (this.playlistManager == null)
                    this.playlistManager = new PlaylistManager(_db);
                return playlistManager;
            }
        }
    }
}
