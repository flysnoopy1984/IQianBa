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

      
        private EOneGame _GameData;
        private string _RoomCode;
        private string _UserOpenId;

        public EOneGame GameData
        {
            get { return _GameData; }
        }

        public GameManager(string userOpenId)
        {
            _UserOpenId = userOpenId;


        }

        private void GetGameData()
        {
            try
            {
                _GameData = new EOneGame
                {
                    RoomCode = _RoomCode,
                    CurD = GameTableRedis.DotPosition(_RoomCode).IntMsg,
                    GameStatus = (GameStatus)GameRedis.GetGameStatus(_RoomCode).IntMsg,
                    PlayerList = RoomUserRedis.GetAllPlayer(_RoomCode).resultList,
                    TableCardList = GameTableRedis.TableCardList(_RoomCode).resultList,
                };
            }
            catch(Exception ex)
            {
                _GameData.ErrorMsg = ex.Message;
            }
           
           
        }

        private void InitNewGameData()
        {
            _GameRedis.SetGameStatus(_RoomCode, GameStatus.NoGame);

        }

        public OutAPIResult GameInfo(int weight)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                r = RoomRedis.FindOrCreateRoom(_UserOpenId, weight);
                if (r.IsSuccess)
                {
                    _RoomCode = r.SuccessMsg;

                    //房间是新建的
                    if(r.IntMsg==0)
                        InitNewGameData();

                    RoomUserRedis.UserEntryRoom(weight, _UserOpenId, _RoomCode);

                    MoveNextGameStatus();

                    GetGameData();
                }
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
        public void MoveNextGameStatus()
        {
            //检查游戏状态
            var gs = (GameStatus)GameRedis.GetGameStatus(_RoomCode).IntMsg;
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
            _GameRedis.SetGameStatus(_RoomCode, GameStatus.NoGame);

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
    }
}
