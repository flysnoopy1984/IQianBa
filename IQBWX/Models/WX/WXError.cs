using IQBWX.Models.JsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.WX
{
    public class WXError:jsonError
    {
        public string WXErrorCode { get; set; }
    }
}