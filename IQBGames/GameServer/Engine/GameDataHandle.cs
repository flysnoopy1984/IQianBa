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
        private CardDataManager _CardDataManager;

        public CardDataManager CardDataManager
        {
            get
            {
                return _CardDataManager;
            }
        }

        public GameDataHandle(EOneGame game)
        {
            _OneGame = game;
            _CardDataManager = new CardDataManager();
        }


        public static OutAPIResult ReCoverData(GameServer server, string RoomCode)
        {
            OutAPIResult r = new OutAPIResult();
         
            try
            {

                if (server.GameDataDic.ContainsKey(RoomCode))
                    return r;
                else
                {
                    EOneGame game = new EOneGame(RoomCode);
                    GameDataHandle handle = new GameDataHandle(game);

                    server.GameDataDic.Add(RoomCode, handle);
                }
                   
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }

        public static OutAPIResult GenerateEmptyGame(GameServer server,string RoomCode)
        {
            OutAPIResult r = new OutAPIResult();
            EOneGame game = new EOneGame(RoomCode);
           
            GameDataHandle handle = new GameDataHandle(game);
            try
            {
                handle.InitNewGameData();
              
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

        public EGameInfo GetGameInfo()
        {
            return _OneGame.BasicInfo;
        }

        public OutAPIResult SetGameInfo(EGameInfo gi)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                GameRedis.SetGameBasic(gi);
                _OneGame.BasicInfo = gi;
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
                var roomCode = _OneGame.BasicInfo.RoomCode;
                EGameInfo basicInfo = GameRedis.GetGameBasic(roomCode).resultObj;
                _OneGame.BasicInfo = basicInfo;
                //_OneGame.BasicInfo.CurD = basicInfo.CurD;
                //_OneGame.BasicInfo.GameStatus = basicInfo.GameStatus;
                //_OneGame.BasicInfo.GameTurn = basicInfo.GameTurn;

                _OneGame.PlayerList = RoomUserRedis.GetAllPlayer(roomCode).resultList;
                if(_CardDataManager.PlayCards.Count>0)
                {
                    foreach (var player in _OneGame.PlayerList)
                    {
                        player.CardList = _CardDataManager.PlayCards[player.SeatNo];
                    }
                }
               
                _OneGame.TableCardList = _CardDataManager.TableCards; //CardDataManager.NoToCard(GameTableRedis.TableCardList(roomCode).resultList);
               
            }
            return _OneGame;
        }

        private void InitNewGameData()
        {
            _OneGame.BasicInfo.GameTurn = GameTurn.NotStart;
            _OneGame.BasicInfo.GameStatus = GameStatus.NoGame;
            _OneGame.BasicInfo.CurD = -1;
          

            //设置游戏状态为等待用户
            GameRedis.SetGameBasic(_OneGame.BasicInfo);
            
          //  GameRedis.SetGameStatus(_OneGame.BasicInfo.RoomCode, GameStatus.NoGame);
           


        }
      
    }
}
