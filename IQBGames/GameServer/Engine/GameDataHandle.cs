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
                    _GameRedis = new GameRedis.Games.GameRedis(_OneGame.GameInfo.RoomCode);
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

        private Queue<ERoomUser> _PlayerQueue = null;

        private Queue<ERoomUser> _PlayerDoneQueue = null;

        private Queue<ERoomUser> _PlayerQuitQueue = null;


        public GameDataHandle(EOneGame game)
        {
            _OneGame = game;
            _CardDataManager = new CardDataManager();
        }

        protected Queue<ERoomUser> PlayerQueue {
            get {
                if (_PlayerQueue == null)
                    _PlayerQueue = new Queue<ERoomUser>();
                return _PlayerQueue; }
        }

        protected Queue<ERoomUser> PlayerDoneQueue
        {
            get
            {
                if (_PlayerDoneQueue == null)
                    _PlayerDoneQueue = new Queue<ERoomUser>();
                return _PlayerDoneQueue;
            }
        }

        protected Queue<ERoomUser> PlayerQuitQueue
        {
            get
            {
                if (_PlayerQuitQueue == null)
                    _PlayerQuitQueue = new Queue<ERoomUser>();
                return _PlayerQuitQueue;
            }
        }

        public EGameInfo GetGameInfo()
        {
            return _OneGame.GameInfo;
        }

        public OutAPIResult SetGameInfo(EGameInfo gi)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                GameRedis.SetGameBasic(gi);
                _OneGame.GameInfo = gi;
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }
            return r;

        }

        public EGameCoins GetGameCoins()
        {
            return _OneGame.GameCoins;
        }

        public void AddPlayerCoins(ECoinDetail coinDetail)
        {
            _OneGame.GameCoins.AddCoins(coinDetail);

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

        public EOneGame GetGameData(bool NeedRefrush = false)
        {
            if (NeedRefrush)
            {
                var roomCode = _OneGame.GameInfo.RoomCode;
                EGameInfo basicInfo = GameRedis.GetGameBasic(roomCode).resultObj;
                _OneGame.GameInfo = basicInfo;
              
                _OneGame.PlayerList = RoomUserRedis.GetAllPlayer(roomCode).resultList;

            
                _OneGame.TableCardList = _CardDataManager.TableCards;
               
               

            }
            return _OneGame;
        }

        private void InitNewGameData()
        {
            _OneGame.GameInfo.GameTurn = 0;
            _OneGame.GameInfo.GameStatus = GameStatus.NoGame;
        

            //设置游戏状态为等待用户
            GameRedis.SetGameBasic(_OneGame.GameInfo);
            
          //  GameRedis.SetGameStatus(_OneGame.BasicInfo.RoomCode, GameStatus.NoGame);

        }

        /// <summary>
        /// 一局开始缓存玩家,根据GameInfo需要对玩家重新排列
        /// </summary>
        /// <param name="PlayerList"></param>
        public void SetCachePlayer(List<ERoomUser> PlayerList,EGameInfo gi)
        {
            InitQueue();

            var p = PlayerList[gi.FirstPlayerIndex];
            PlayerQueue.Enqueue(p);
            int i = gi.FirstPlayerIndex+1;

            while (PlayerQueue.Count< PlayerList.Count)
            {
                if (i == PlayerList.Count)
                    i = 0;
                p = PlayerList[i];
                PlayerQueue.Enqueue(p);
            }
          
        }

        public Queue<ERoomUser> GetCachePlayer()
        {
            return PlayerQueue;
        }

        public ERoomUser PopPlayer(PlayerStauts playerStatus)
        {
            if(PlayerQueue.Count != 0)
            {
                var Player = PlayerQueue.Dequeue();
                if (playerStatus == PlayerStauts.OffLine ||
                    playerStatus == PlayerStauts.GiveUp)
                    PlayerQuitQueue.Enqueue(Player);
                else
                    PlayerDoneQueue.Enqueue(Player);

                return Player;
            }
            return null;
        }

        public void ReloadPlayer()
        {
            while(PlayerDoneQueue.Count>0)
            {
                var p = PlayerDoneQueue.Dequeue();
                if (p.PlayerStauts >= 0)
                {
                    p.PlayerStauts = PlayerStauts.PrepareBet;
                    PlayerQueue.Enqueue(p);
                }
                else
                    PlayerQuitQueue.Enqueue(p);
            }
        }

        public EGameInfo MoveNextTurn(EGameInfo gi,bool needSave = false)
        {
            if(gi.GameTurn != GameTurn.End)
            {
                if (gi.GameTurn == GameTurn.FourthTurn)
                    gi.GameTurn = GameTurn.End;
                else
                {
                  
                    gi.GameTurn++;
                }
                    

                gi.CurRequireCoins = 0;
            }

            if(needSave) this.SetGameInfo(gi);
            return gi;
        }

        public ERoomUser PeekAvaliblePlayer(EGameInfo gi)
        {
            ERoomUser result = null;
            //只剩一个人时，比赛也是结束了
            if (PlayerQueue.Count + PlayerDoneQueue.Count == 1)
                return null;

            while (PlayerQueue.Count > 0)
            {
                var Player = PlayerQueue.Peek();
                if (Player.PlayerStauts == PlayerStauts.PrepareBet)
                {
                    result = Player;
                    break;
                }    
                else
                {
                    PlayerQuitQueue.Enqueue(PopPlayer(Player.PlayerStauts));
                }
            }
           

            return null;
        }

        private void InitQueue()
        {
            PlayerQueue.Clear();
            PlayerDoneQueue.Clear();
            PlayerQuitQueue.Clear();
        }




    }
}
