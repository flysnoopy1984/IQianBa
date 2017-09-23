using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace IQBWX.Handler
{
    public class CometResult : IAsyncResult
    {
        #region 实现IAsyncResult接口
        public object AsyncState { get; private set; }
        public WaitHandle AsyncWaitHandle { get; private set; }
        public bool CompletedSynchronously { get; private set; }
        public bool IsCompleted { get; private set; }
        #endregion
        public AsyncCallback Callback { get; private set; }
        public HttpContext Context { get; private set; }
        public object ExtraData { get; private set; }
        public CometResult(HttpContext context, AsyncCallback callback, object extraData)
        {
            this.Context = context;
            this.Callback = callback;
            this.ExtraData = extraData;

           
        }

        public void StartAsyncTask()
        {

        }



        public void Call()
        {
            if (this.Callback != null)
                this.Callback(this);
        }
    }
}