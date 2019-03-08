using GameModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Engine
{
    public class GameDataHandle
    {
        private Dictionary<string, EOneGame> _GameDataDic;
        //public Dictionary<string,EOneGame> GameDataDic {
        //    get
        //    {
        //        if (_GameDataDic == null)
        //            _GameDataDic = new Dictionary<string, EOneGame>();
        //        return _GameDataDic;
        //    }
        //}

        public GameDataHandle()
        {
            _GameDataDic = new Dictionary<string, EOneGame>();
        }

        public EOneGame GetGameData(string roomCode)
        {
            if (_GameDataDic.ContainsKey(roomCode))
                return _GameDataDic[roomCode];
            else
            {
              //  var gameData = 
            }
            return null;
        }


    }
}
