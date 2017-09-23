using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace IQBWX.Controllers
{
    public class IQBAsyncController : AsyncController
    {
        public void DoAsync()
        {
            //注册一次异步
            AsyncManager.OutstandingOperations.Increment();
            Timer timer = null;
            timer = new Timer(o =>
            {
                //一次异步完成
                AsyncManager.OutstandingOperations.Decrement();
                timer.Dispose();
            }, null, 5000, 5000);
        }

        public ActionResult DoCompleted()
        {
            return Content("OK");
        }
    }
}