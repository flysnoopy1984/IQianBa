using IQBWX.Models.Results;
using IQBWX.Models.User;
using IQBWX.Models.WX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.BLL.ExternalWeb
{
    abstract public class BaseExternalWeb
    {
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ui"></param>
        /// <returns>"OK",EXIST</returns>
        abstract public string regeisterWebMember(EUserInfo ui, long QRAuthId = 0);

        abstract public RExternalWebResult WXInfo(EUserInfo ui, WXMessage msg);

        public static BaseExternalWeb GetExternalWeb(string appId)
        {
            switch(appId)
            {
                case "pp":
                    return GetExternalWeb(IQBCore.IQBWX.BaseEnum.ExternalWeb.Pay);
                default:
                    return GetExternalWeb(IQBCore.IQBWX.BaseEnum.ExternalWeb.Book);
            }
        }

        public static BaseExternalWeb GetExternalWeb(IQBCore.IQBWX.BaseEnum.ExternalWeb _ExternalWeb)
        {
            switch(_ExternalWeb)
            {
                case IQBCore.IQBWX.BaseEnum.ExternalWeb.Book:
                    return new ExtWebBook();
                case IQBCore.IQBWX.BaseEnum.ExternalWeb.Pay:
                    return new ExtWebPay();
                default:
                    throw new Exception("No ExternalWeb could be found!");
            }
        }

    }
}