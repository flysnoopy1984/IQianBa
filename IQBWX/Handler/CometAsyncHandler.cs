using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Handler
{
    public class CometAsyncHandler : IHttpAsyncHandler
    {
        public bool IsReusable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            //通过context可以取请求附加的数据，略
            //...

            //之后生成IAsyncResult对象，callback比较重要，调用这个回调，EndProcessRequest才被触发
            var result = new CometResult(context, cb, extraData);
            //在返回之前把刚生成的IAsyncResult对象保存起来，略
            //...
            return result;
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}