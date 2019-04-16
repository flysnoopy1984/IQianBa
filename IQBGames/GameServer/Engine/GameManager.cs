using GameCommon.Config;
using GameModel;
using GameModel.Enums;
using GameModel.Message;
using GameModel.WebSocketData.SendData;
using GameModel.WebSocketData.SendData.Playing;
using GameRedis.Games;
using GameServer.Engine.Sync;
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
    

        private GameRedis.Games.GameRedis _GameRedis;
        private GameRedis.Games.GameRedis GameRedis
        {
            get
            {
                if (_GameRedis == null)
                    _GameRedis = new GameRedis.Games.GameRedis(RoomCode);
                return _GameRedis;
            }
        }
        #endregion

        private string _UserOpenId;
        private string _RoomCode;
        private GameUserSession _Session;

        public string UserOpenId
        {
            get { return _UserOpenId; }
        }

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

        public ERoom GetRoom()
        {
            return RoomRedis.GetRoom(RoomCode).Instance;
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
      
        public OutAPIResult SetGameInfo(EGameInfo gi)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {

               r = GameDataHandle.SetGameInfo(gi);
            
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }
            return r;
        }

        public EGameInfo GetGameBasic()
        {
            EGameInfo gi = null;
            try

            {
                return GameDataHandle.GetGameInfo();
              //  gi = GameRedis.GetGameBasic(RoomCode).resultObj;

            }
            catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取下个游戏状态
        /// </summary>
        public GameStatus GetNextGameStatus(EGameInfo  gi, bool NeedSave = false)
        {
            //检查游戏状态
        
            var gs = gi.GameStatus;
     
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
                    nextStatus = GameStatus.GameEndSettlement;
                    break;
                case GameStatus.GameEndSettlement:
                    nextStatus = GameStatus.StartShuffle;
                    break;
            }
            if(NeedSave)
            {
                gi.GameStatus = nextStatus;
                this.SetGameInfo(gi);
            }
      
            return nextStatus;

        }

       
        public OutAPIResult UserSitDown(int SeatNo, decimal coins)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                if (coins <= 0)
                {
                    result.ErrorMsg = "金币不足，无法入座";
                    return result;
                }
                result = RoomUserRedis.UserSitDown(_UserOpenId, SeatNo, coins);
               

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
        /// 前端开始洗牌，同时系统分发派
        /// </summary>
        /// <returns></returns>
        public List<ERoomUser> DoShuffle()
        {
            List<ERoomUser> Players = RoomUserRedis.GetAllPlayer(RoomCode).resultList;

            CardDataManager.ShuffleCard(Players);

            foreach(ERoomUser p in Players)
            {
                p.PlayerStauts = PlayerStauts.PrepareBet;
                
                RoomUserRedis.SetPlayer(RoomCode, p);
            }
            GameRedis.SetTableCards(CardDataManager.TableCards.Values.ToList());
     
            return Players;

        }

        

        public ResultGameShuffling WhileShuffling()
        {
            ResultGameShuffling result = new ResultGameShuffling();
            return result;
        }

        public bool CanSitDown()
        {
            var gi = this.GetGameBasic();
            if (gi.GameStatus == GameStatus.ShuffleEnd ||
               gi.GameStatus == GameStatus.Playing)
                return false;
            else
                return true;
        }

       

        public void PrePareNewGame(EGameInfo gi = null)
        {
            if (gi == null) gi = this.GetGameBasic();

            gi.GameStatus = GameStatus.Shuffling;
            gi.GameTurn = GameTurn.NotStart;
           
        }


        /// <summary>
        /// 判断 GameStatus 是否为GameEndShowingCard
        /// </summary>
        /// <returns></returns>
        public EGameInfo PreNextPlayer(bool NeedSave = false)
        {
            var gi = this.GameDataHandle.GetGameInfo();
            var player = this.GameDataHandle.PeekAvaliblePlayer(gi);
            //返回null表示没有找到下一个玩家，此轮结束
            if (player == null)
            {
                gi = this.GameDataHandle.MoveNextTurn(gi);
                if(gi.GameTurn != GameTurn.End)
                {
                    this.GameDataHandle.ReloadPlayer();
                    player = this.GameDataHandle.PeekAvaliblePlayer(gi);
                    //如果再是null,游戏结束
                    if (player == null)
                    {
                        this.GameEndShowCard();
                    }
                }               
            }
            else 
                gi.CurBetUserOpenId = player.UserOpenId;

            if(NeedSave)  this.SetGameInfo(gi);
            return gi;
        }

        public IGameMessage WaitNextPlayer(EGameInfo gi)
        {
            if (gi.GameStatus == GameModel.Enums.GameStatus.GameEndShowingCard)
            {
                return GameMessageHandle.CreateGameEndShowCardMsg(this.RoomCode); 
            }
            else
            {
                UserWaitBetTask waitTask = UserWaitBetTask.CreateNewInstance(_Session.GameServer, gi.CurBetUserOpenId);
                waitTask.Run(GameConfig.Turn_Wait_Server);
            }
            return null;
        }

        public SResult<EGameInfo> GetFirstDotAndBet(List<ERoomUser> playerList)
        {
            SResult<EGameInfo> result = new SResult<EGameInfo>();
            try
            {
             
                var CurDIndex =-1;
                var bIndex = -1;
                var sIndex = -1;
                var cIndex = -1;
                EGameInfo gi = this.GameDataHandle.GetGameInfo();
              
               
                if(gi == null || playerList == null)
                {
                    result.ErrorMsg = "没有获取玩家信息";
                    return result;
                }
                
                if (gi.DotUserOpenId == null)
                    gi.DotUserOpenId = playerList[0].UserOpenId;

                for (int i = 0; i < playerList.Count; i++)
                {
                    if (playerList[i].UserOpenId == gi.DotUserOpenId)
                    {
                        CurDIndex = i + 1;
                        if (CurDIndex == playerList.Count) CurDIndex = 0;

                        sIndex = CurDIndex + 1;
                        if (sIndex == playerList.Count) sIndex = 0;

                        bIndex = sIndex + 1;
                        if (bIndex == playerList.Count) bIndex = 0;

                        cIndex = bIndex + 1;
                        if (cIndex == playerList.Count) cIndex = 0;

                    }
                }
                gi.DotUserOpenId = playerList[CurDIndex].UserOpenId;
                gi.BigBetUserOpenId = playerList[bIndex].UserOpenId;
                gi.SmallBetUserOpenId = playerList[sIndex].UserOpenId;
                gi.CurBetUserOpenId = playerList[cIndex].UserOpenId;

                gi.FirstPlayerIndex = cIndex;

                result.Instance = gi;
              
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
           
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void PlayerPass()
        {
            try
            {
                var player = this.GameDataHandle.PopPlayer();
                player.PlayerStauts = PlayerStauts.Pass;
                RoomUserRedis.SetPlayer(RoomCode, player);
            }
            catch (Exception ex)
            {
                //错误需要将改玩家剔除
            }
        }

        public void PlayerGiveUp()
        {
            try
            {
                var player = this.GameDataHandle.PopPlayer();
                player.PlayerStauts = PlayerStauts.GiveUp;
                RoomUserRedis.SetPlayer(RoomCode, player);
            }
            catch(Exception ex)
            {
                //错误需要将改玩家剔除
            }
            
        }

        public void PlayerFollow(EGameInfo gi,string userOpenId,decimal FollowCoins)
        {
            try
            {
                var player = this.GameDataHandle.PopPlayer();
                player.PlayerStauts = PlayerStauts.Follow;
                RoomUserRedis.SetPlayer(RoomCode, player);

                //加注
                ECoinDetail cd = new ECoinDetail
                {
                    Coins = FollowCoins,
                    Diff = 0,
                    PileNo = (int)gi.GameTurn,
                    UserOpenId = userOpenId,
                    CoinType = CoinType.Follow,
                };
                GameDataHandle.AddPlayerCoins(cd);

            }
            catch (Exception ex)
            {
                //错误需要将改玩家剔除
            }

        }

        public EGameInfo GameEndShowCard(EGameInfo gameInfo = null,bool NeedSave=false)
        {
            EGameInfo gi = null;
            if (gameInfo == null)
                gi = this.GameDataHandle.GetGameInfo();
            else
                gi = gameInfo;
            gi.CurBetUserOpenId = "";
            gi.GameStatus = GameStatus.GameEndShowingCard;
            gi.GameTurn = GameTurn.End;

            if(NeedSave) this.GameDataHandle.SetGameInfo(gi);
            return gi;
        }

        public ERoomUser PlayerAddCoins(EGameInfo gi,string userOpenId,decimal coins,decimal diff)
        {
            

            //队列重新排列
            GameDataHandle.ReloadPlayer();

            //获取当前玩家，并修改状态
            var player = this.GameDataHandle.PopPlayer();
            player.PlayerStauts = PlayerStauts.AddCoins;
            RoomUserRedis.SetPlayer(RoomCode, player);

            //加注
            ECoinDetail cd = new ECoinDetail
            {
                Coins = coins,
                Diff = diff,
                PileNo = (int)gi.GameTurn,
                UserOpenId = userOpenId,
                CoinType =  CoinType.Add
            };
            GameDataHandle.AddPlayerCoins(cd);

            //重新获取和设定下一个玩家(必须有下一个玩家，没有则直接结束？)
            var nextUser = GameDataHandle.PeekAvaliblePlayer(gi);
            if(nextUser!=null)
            {
                gi.CurBetUserOpenId = nextUser.UserOpenId;
                GameDataHandle.SetGameInfo(gi);
            }
          
            return nextUser;

        }
    }
}
