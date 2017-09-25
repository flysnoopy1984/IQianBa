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
        abstract public string regeisterWebMember(EUserInfo ui);

        abstract public RExternalWebResult WXInfo(EUserInfo ui, WXMessage msg);

        public static BaseExternalWeb GetExternalWeb(IQBWX.Common.ExternalWeb _ExternalWeb)
        {
            switch(_ExternalWeb)
            {
                case Common.ExternalWeb.Book:
                    return new ExtWebBook();
                case Common.ExternalWeb.Pay:
                    return new ExtWebPay();
                default:
                    throw new Exception("No ExternalWeb could be found!");
            }
        }

    }
}