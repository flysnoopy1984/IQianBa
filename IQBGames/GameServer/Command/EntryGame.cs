
using Newtonsoft.Json;
using SuperSocket.SocketBase.Command;
using SuperSocket.WebSocket.SubProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.WebSocket;
using GameServer.Engine;
using GameModel.WebSocketData.ReceiveData;
using IQBCore.Common.Helper;
using GameServer.Command;
using GameModel;
using GameModel.Message;

namespace GameServer.Commond
{
    public class EntryGame : BaseGameCommand<dataNewConnect>
    {
        public override bool VerifyCommandData(dataNewConnect InData)
        {
            if (string.IsNullOrEmpty(InData.OpenId))
            {
                base.GameMessageHandle.PushErrorMsg("错误，没有获取您的身份，请重新登陆");
                return false;
            }
            return true;
        }

        public override List<BaseNormalMsg> HandleData(GameUserSession session, dataNewConnect data)
        {

            session.GameAttr.OpenId = data.OpenId;
            session.GameAttr.UserName = data.UserName;
            session.GameAttr.Weight = data.Weight;

            session.GameServer.SetOpenIdSession(data.OpenId, session.SessionID);

            List<BaseNormalMsg> result = new List<BaseNormalMsg>();

            var oneGame = FindGame(data.OpenId, data.Weight);
            
            result.Add(oneGame);
            return result;
        
        }

        private EOneGame FindGame(string userOpenId,int weight)
        {
            EOneGame game = null;
            try
            {
                GameManager gameManager = new GameManager(userOpenId);
                var r = gameManager.GameInfo(weight);
                if(!r.IsSuccess)
                    gameManager.GameData.ErrorMsg = r.ErrorMsg;
                return gameManager.GameData;


            }
            catch(Exception ex)
            {
                game.ErrorMsg = ex.Message;
            }
            return game;
            
        }

        public override string Name
        {
            get
            {
                return "EntryGame";
            }
        }

        #region Old
        //try
        //{
        //    var jsonStr = requestInfo.Body;
        //    if (!string.IsNullOrEmpty(jsonStr))
        //    {

        //        cmdNewConnect data = JsonConvert.DeserializeObject<cmdNewConnect>(requestInfo.Body);

        //        if (string.IsNullOrEmpty(data.OpenId))
        //        {

        //            session.Send($"错误，没有获取您的身份，请重新登陆");
        //            session.Close();
        //            return;
        //        }
        //        else
        //        {

        //            if (DataManager.UserOpenIdList.ContainsKey(data.OpenId))
        //            {

        //                var userSession = session.AppServer.GetSessionByID(DataManager.UserOpenIdList[data.OpenId]);
        //                if (userSession != null)
        //                    userSession.Close();
        //            }

        //            DataManager.UserOpenIdList[data.OpenId] = session.SessionID;

        //            session.UserName = data.UserName;
        //            session.OpenId = data.OpenId;

        //            string msg = FindGame(data.OpenId, data.Weight);
        //            session.Send(msg);
        //        }
        //    }
        //    else
        //    {

        //    }
        //}
        //catch(Exception ex)
        //{
        //    NLogHelper.GameError($"EntryGame:{ex.Message}");
        //}
        #endregion
        

       
    }
}
