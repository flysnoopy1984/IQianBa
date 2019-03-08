using GameModel;
using GameModel.Enums;
using GameRedis.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Engine
{
    public class GameDataHandle
    {
        #region Redis属性
        private RoomUserRedis _RoomUserRedis;
        private RoomUserRedis RoomUserRedis
        {
            get
            {
                if (_RoomUserRedis == null)
                    _RoomUserRedis = new RoomUserRedis();
                return _RoomUserRedis;
            }
        }
        private RoomRedis _RoomRedis;
        private RoomRedis RoomRedis
        {
            get
            {
                if (_RoomRedis == null)
                    _RoomRedis = new RoomRedis();
                return _RoomRedis;
            }
        }
        private GameTableRedis _GameTableRedis;
        private GameTableRedis GameTableRedis
        {
            get
            {
                if (_GameTableRedis == null)
                    _GameTableRedis = new GameTableRedis();
                return _GameTableRedis;
            }
        }

        private GameRedis.Games.GameRedis _GameRedis;
        private GameRedis.Games.GameRedis GameRedis
        {
            get
            {
                if (_GameRedis == null)
                    _GameRedis = new GameRedis.Games.GameRedis();
                return _GameRedis;
            }
        }
        #endregion

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
            if (!_GameDataDic.ContainsKey(roomCode))
            {
             
                var gamedata = new EOneGame
                {
                    RoomCode = roomCode,
                    CurD = GameTableRedis.DotPosition(roomCode).IntMsg,
                    GameStatus = (GameStatus)GameRedis.GetGameStatus(roomCode).IntMsg,
                    PlayerList = RoomUserRedis.GetAllPlayer(roomCode).resultList,
                    TableCardList = GameTableRedis.TableCardList(roomCode).resultList,
                    RemainCardList = new  Dictionary<int, int>()
                };
                InitRemainCard(roomCode);
                _GameDataDic.Add(roomCode, gamedata);

            }

            return _GameDataDic[roomCode];
        }

        public void InitRemainCard(string roomCode)
        {
            try
            {
                var gamedata = _GameDataDic[roomCode];
                gamedata.RemainCardList.Clear();
                for(int i=1;i<=52;i++)
                {
                    gamedata.RemainCardList.Add(i, i);
                }
            }
            catch
            {

            }
           
        }


    }
}
