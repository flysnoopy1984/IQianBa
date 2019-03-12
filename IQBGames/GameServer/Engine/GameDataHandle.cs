using GameModel;
using GameModel.Enums;
using GameRedis.Games;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.Model;
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

        
        private EOneGame _OneGame;
        public GameDataHandle(EOneGame game)
        {
            _OneGame = game;
        }

        public static NResult<GameDataHandle> GenerateNew(GameServer server,string RoomCode)
        {
            NResult<GameDataHandle> r = new NResult<GameDataHandle>();
            EOneGame game = new EOneGame(RoomCode);
           
            GameDataHandle handle = new GameDataHandle(game);
            try
            {
                handle.InitNewGameData();
                handle.InitRemainCard();
               

                if (server.GameDataDic.ContainsKey(RoomCode))
                    r.ErrorMsg = "房间已存在！";
                else
                    server.GameDataDic.Add(RoomCode, handle);
            }
            catch(Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }
           
            return r;
        }

        public EOneGame GetGameData(bool NeedRefrush = false)
        {
          
            if (NeedRefrush)
            {
                var roomCode = _OneGame.RoomCode;
                _OneGame.CurD = GameTableRedis.DotPosition(roomCode).IntMsg;
                _OneGame.GameStatus = (GameStatus)GameRedis.GetGameStatus(roomCode).IntMsg;
                _OneGame.PlayerList = RoomUserRedis.GetAllPlayer(roomCode).resultList;
                _OneGame.TableCardList = CardManager.NoToCard(GameTableRedis.TableCardList(roomCode).resultList);
                _OneGame.RemainCardList = CardManager.InitNewCards();
             
            }
            return _OneGame;
        }

        public void InitRemainCard()
        {
            try
            {
                _OneGame.RemainCardList = CardManager.InitNewCards();
            }
            catch
            {

            }
        }

        private void InitNewGameData()
        {
             GameRedis.SetGameStatus(_OneGame.RoomCode, GameStatus.NoGame);
        }
        public GameStatus GameStatus
        {
            get
            {
                var r = GameRedis.GetGameStatus(_OneGame.RoomCode);
                return (GameStatus)r.IntMsg;
            }
        }

    }
}
