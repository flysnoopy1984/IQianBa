using GameModel;
using GameModel.Enums;
using GameModel.WebSocketData.SendData;
using GameRedis.Games;
using IQBCore.IQBPay.Models.OutParameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Engine
{
    public class GameManager
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

        public OutAPIResult SetGameStatus(GameStatus gs)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                if (string.IsNullOrEmpty(RoomCode))
                    r.ErrorMsg = "没有获取房间号";
                else
                    GameRedis.SetGameStatus(RoomCode, gs);

            }
            catch(Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }
            return r;
        }

        public static OutAPIResult SetGameStatus(GameStatus gs,string RoomCode)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                var GameRedis = new GameRedis.Games.GameRedis();
                if (string.IsNullOrEmpty(RoomCode))
                    r.ErrorMsg = "没有获取房间号";
                else
                    GameRedis.SetGameStatus(RoomCode, gs);

            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }
            return r;
        }


        private string _UserOpenId;
        private string _RoomCode;
        private GameUserSession _Session;

        private CardDataManager CardDataManager
        {
            get
            {
                return GameDataHandle.CardDataManager;
            }
        }
        public string RoomCode
        {
           get {
                if(string.IsNullOrEmpty(_RoomCode))
                    _RoomCode = RoomUserRedis.GetUserRoomCode(_UserOpenId); 
           
                return _RoomCode;
            }
        }

        public GameManager(string userOpenId,GameUserSession session)
        {
           // _RoomCode = RoomCode;
            _UserOpenId = userOpenId;
            _Session = session;
        }

        public GameDataHandle GameDataHandle
        {
            get
            {
                if (!string.IsNullOrEmpty(RoomCode))
                {
                    //全局数据修改
                    //if (!_Session.GameServer.GameDataDic.ContainsKey(RoomCode))
                    //    _Session.GameServer.GameDataDic[RoomCode] = new GameDataHandle(new EOneGame());
                    return _Session.GameServer.GameDataDic[RoomCode];
                }
                    
                else
                    throw new Exception("未进入房间，不能获取数据！");
            }
        }

        public OutAPIResult FindAvailableRoom(int weight)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                r = RoomRedis.FindOrCreateRoom(_UserOpenId, weight);
                if(r.IsSuccess)
                {
                   // this._RoomCode = r.SuccessMsg; 
                }
                
            }
            catch(Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }
            return r;
            
        }
        public OutAPIResult UserEntryRoom(string roomCode)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
           

                //登记用户
                r = RoomUserRedis.UserLogin(_UserOpenId, roomCode);
                if (r.IsSuccess)
                {
                    //用户进入房间
                    r = RoomUserRedis.UserEntryRoom(_UserOpenId, roomCode);
                    _RoomCode = roomCode;
                }
                   
            }
            catch(Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }
  
        /// <summary>
        /// 获取下个游戏状态
        /// </summary>
        public GameStatus GetNextGameStatus(GameStatus currentStatus)
        {
            //检查游戏状态
            var roomCode = RoomCode;
            var gs = (GameStatus)GameRedis.GetGameStatus(roomCode).IntMsg;
            GameStatus nextStatus = gs;
            switch (gs)
            {
                case GameStatus.NoGame:
                    nextStatus = GameStatus.WaitPlayer;
                    break;
                case GameStatus.WaitPlayer:
                    nextStatus = GameStatus.StartShuffle;
                    break;
                case GameStatus.StartShuffle:
                    nextStatus = GameStatus.Shuffling;
                    break;
                case GameStatus.Shuffling:
                    nextStatus = GameStatus.ShuffleEnd;
                    break;
                case GameStatus.ShuffleEnd:
                    nextStatus = GameStatus.Playing;
                    break;
                case GameStatus.Playing:
                    nextStatus = GameStatus.Settlement;
                    break;
                case GameStatus.Settlement:
                    nextStatus = GameStatus.StartShuffle;
                    break;
            }
            _GameRedis.SetGameStatus(roomCode, nextStatus);
            return nextStatus;

        }

        public ResultUserSitDown UserSitDown(int SeatNo, decimal coins)
        {
            ResultUserSitDown result = new ResultUserSitDown();
            try
            {
                var r = RoomUserRedis.UserSitDown(_UserOpenId, SeatNo,coins);
                if (r.IsSuccess)
                    result.SeatNo = r.IntMsg;
                else
                    result.ErrorMsg = r.ErrorMsg;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }

            return result;
        }

        public ResultUserSitUp UserSitUp()
        {
            ResultUserSitUp result = new ResultUserSitUp();
            try
            {
                var r = RoomUserRedis.UserSitUp(_UserOpenId);
                if (!r.IsSuccess)
                    result.ErrorMsg = r.ErrorMsg;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }

            return result;
        }

        public ResultBackHall UserBackHall()
        {
            ResultBackHall result = new ResultBackHall();
            try
            {
                var r = RoomUserRedis.UserExitRoom(_UserOpenId);
                if (!r.IsSuccess)
                    result.ErrorMsg = r.ErrorMsg;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }

            return result;

        }

        /// <summary>
        /// 发牌结束
        /// </summary>
        /// <param name="roomCode"></param>
        /// <returns></returns>
        public ResultGameShuffleEnd ShuffleEnd(string roomCode)
        {
            ResultGameShuffleEnd msg = new ResultGameShuffleEnd(roomCode);
            return msg;
        }

        /// <summary>
        /// 前端开始洗牌，同时系统分发派
        /// </summary>
        /// <returns></returns>
        public ResultGameShuffleStart DoShuffle()
        {
            ResultGameShuffleStart result = new ResultGameShuffleStart(RoomCode);

            CardDataManager.ShuffleCard();

            result.TableCards = CardDataManager.TableCards.Values.ToList();

            int seat =  RoomUserRedis.GetUserSeatNo(_UserOpenId);
            if(seat>0)
            {
                result.PlayerCards = CardDataManager.PlayCards[seat];
            }
            return result;

        }

        public ResultGameShuffling WhileShuffling()
        {
            ResultGameShuffling result = new ResultGameShuffling();
            return result;
        }

        public bool CanSitDown()
        {
            if (this.GameDataHandle.GameStatus == GameStatus.ShuffleEnd ||
               this.GameDataHandle.GameStatus == GameStatus.Playing)
                return false;
            else
                return true;
        }
    }
}
