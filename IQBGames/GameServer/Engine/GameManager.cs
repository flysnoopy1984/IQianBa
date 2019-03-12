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

    
      
        private string _UserOpenId;
        private GameUserSession _Session;

      
        

        public string RoomCode
        {
           get {
                return RoomUserRedis.GetUserRoomCode(_UserOpenId);
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
                    return _Session.GameServer.GameDataDic[RoomCode];
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
              //  _RoomCode = roomCode;

                r = RoomUserRedis.UserLogin(_UserOpenId, roomCode);
                if (r.IsSuccess)
                    r = RoomUserRedis.UserEntryRoom(_UserOpenId, roomCode);
            }
            catch(Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }

        /// <summary>
        /// 每次调用 游戏状态会继续
        /// </summary>
        public GameStatus MoveNextGameStatus()
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
                    nextStatus = GameStatus.Shuffle;
                    break;
                case GameStatus.Shuffle:
                    nextStatus = GameStatus.Playing;
                    break;
                case GameStatus.Playing:
                    nextStatus = GameStatus.Settlement;
                    break;
                case GameStatus.Settlement:
                    nextStatus = GameStatus.Shuffle;
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

        public ResultGameShuffleEnd ShuffleEnd(string roomCode)
        {
            ResultGameShuffleEnd msg = new ResultGameShuffleEnd(roomCode);
            return msg;
        }

        public void DoShuffle()
        {
         //  var data =  _Session.GameServer.GetGameData(_RoomCode);
          //  data.TableCardList = 

        }
    }
}
