using GameModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCommon.DataBase
{
    public class GameContent: DbContext
    {
        public GameContent() : base("GameConnection")
        {

        }

        public DbSet<ERoom> DBRoom { get; set; }

        public DbSet<ERoomUser> DBRoomUser { get; set; }

        public DbSet<EUserInfo> DBUserInfo { get; set; }
    }
}
