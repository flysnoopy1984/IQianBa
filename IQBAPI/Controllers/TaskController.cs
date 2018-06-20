using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.Model;
using IQBCore.OO.BaseEnum;
using IQBCore.OO.Models.Entity;
using IQBCore.OO.Models.In;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBAPI.Controllers
{
    public class TaskController : ApiController
    {
        /// <summary>
        /// 创建用户的任务
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        public OutAPIResult CreateUserTask(TaskType taskType)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {

            }
            catch(Exception ex)
            {

            }
            return result;
        }

        /// <summary>
        /// 删除用户的任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public OutAPIResult DeleteUserTask(long taskId)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
           
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        /// <summary>
        /// 获取任务主数据
        /// </summary>
        /// <returns></returns>
        public NResult<ETaskInfo> QueryTask(QTask qTask)
        {
            NResult<ETaskInfo> result = new NResult<ETaskInfo>();
            try
            {

            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return result;

        }
    }
}
