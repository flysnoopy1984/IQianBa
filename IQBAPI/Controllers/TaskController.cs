using IQBCore.Common.Helper;
using IQBCore.DataBase;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.Model;
using IQBCore.OO.BaseEnum;
using IQBCore.OO.Models.Entity;
using IQBCore.OO.Models.Query;
using IQBCore.OO.Models.QueryResult;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;

namespace IQBAPI.Controllers
{
    public class TaskController : BaseAPIController
    {
        [HttpPost]
        public NResult<EUserTask> Test(long UserId)
        {
            ErrorToDb("aaa");
            NLogHelper.InfoTxt("prod 1");
            return null;
        }

        /// <summary>
        /// 用户接受任务
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        [HttpPost]
        public NResult<EUserTask> CreateUserTask(long UserId,long TaskId)
        {
            NResult<EUserTask> result = new NResult<EUserTask>();
            try
            {
                using (OOContent db = new OOContent())
                {
                  
                    ETaskInfo task = db.DBTaskInfo.Where(a => a.Id == TaskId).FirstOrDefault();
                    if(task == null || task.RecordStatus == RecordStatus.Blocked)
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = "任务已无效，请刷新界面再接受任务";
                        return result;
                    }
                    //查找状态为【处理中】的订单，如果有则此任务不能接受
                    EUserTask userTask = db.DBUserTask.Where(a => a.TaskId == TaskId && 
                                                             a.UserId == UserId && 
                                                             a.UserTaskStatus == UserTaskStatus.Process).FirstOrDefault();

                    if(userTask!=null)
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = "已经有相同的任务在执行";
                        result.IntMsg = -10;
                        result.resultObj = userTask;
                        return result;
                    }
                    using (TransactionScope ts = new TransactionScope())
                    {
                        //用户接受任务
                        userTask = new EUserTask();
                        userTask.UserId = UserId;
                        userTask.TaskId = TaskId;
                        userTask.UserTaskStatus = UserTaskStatus.Process;
                        userTask.CreatedTime = DateTime.Now;
                        db.DBUserTask.Add(userTask);
                        db.SaveChanges();

                        //如果是订单任务，创建任务对应的订单
                        if (task.TaskType == TaskType.Order)
                        {
                            EUserTaskOrder userTaskOrder = new EUserTaskOrder()
                            {
                                OrderId = StringHelper.GenerateOONo(),
                                Qty = 1,
                                UserTaskId = userTask.Id
                            };
                            db.DBUserTaskOrder.Add(userTaskOrder);
                        }

                        db.SaveChanges();
                        ts.Complete();
                    }

                   

                    result.resultObj = userTask;
                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
                ErrorToDb(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 删除用户的任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public OutAPIResult DeleteUserTask(long UserId, long TaskId)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
           
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
                ErrorToDb(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 获取用户任务
        /// </summary>
        /// <param name="qTask"></param>
        /// <returns></returns>
        [HttpPost]
        public NResult<RUserTask> QueryUserTask(QTask qTask)
        {
            NResult<RUserTask> result = new NResult<RUserTask>();
            try
            {
                using (OOContent db = new OOContent())
                {
                    var sql = @"select t.Title,
                    t.Description as TaskDescription,
                    t.TaskType,
                    t.Id as TaskId,
                    item.Name as ItemName,
                    item.RealUrl,
                    item.Price,
                    ut.UserTaskStatus,
                    ut.UserId,
                    ut.Id  
                    from TaskInfo as t
                    join UserTask as ut on ut.TaskId = t.Id
                    join ItemInfo as item on item.Id = t.RefItemId
                    where ut.UserId = @UserId 
                    order by ut.CreatedTime";

                    //sql = string.Format(sql, qTask.AcceptUserId);
                    SqlParameter[] param = new SqlParameter[] {
                        new SqlParameter("@UserId",qTask.AcceptUserId)
                    };

                    var list =  db.Database.SqlQuery<RUserTask>(sql, param);

                    if (qTask.pageIndex == 0)
                    {
                        result.resultList = list.Take(qTask.pageSize).ToList();
                    }
                    else
                    {
                        result.resultList = list.Skip(qTask.pageIndex * qTask.pageSize).Take(qTask.pageSize).ToList();
                    }

                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
               
            }
            return result;

        }


        /// <summary>
        /// 创建任务主数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public NResult<ETaskInfo> CreateOrUpdate(ETaskInfo obj)
        {
            NResult<ETaskInfo> result = new NResult<ETaskInfo>();
            try
            {
                using (OOContent db = new OOContent())
                {
                    ETaskInfo updateObj = null;
                    if (obj.RefItemId > 0)
                    {
                        string sql = string.Format("select count(1) from ItemInfo where Id={0}", obj.RefItemId);
                        int ItemCount = db.Database.SqlQuery<int>(sql).FirstOrDefault();
                        if (ItemCount <=0)
                        {
                            result.IsSuccess = false;
                            result.ErrorMsg = "创建任务失败。没有找到对相应的对象！";
                            return result;
                        }
                    }

                    if (obj.Id != 0)
                    {
                        updateObj = db.DBTaskInfo.Where(a => a.Id == obj.Id).FirstOrDefault();
                    }
                    
                    
                    //新增
                    if(updateObj == null)
                    {
                        db.DBTaskInfo.Add(obj);
                        db.SaveChanges();
                    }
                    //修改
                    else
                    {
                        updateObj.Title = obj.Title;
                        updateObj.Description = obj.Description;
                        updateObj.TaskType = obj.TaskType;
                        updateObj.RecordStatus = obj.RecordStatus;
                        updateObj.RefItemId = obj.RefItemId;
                        db.SaveChanges();
                    }
                    result.resultObj = obj;
                    
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
                ErrorToDb(ex.Message);
            }
            return result;
        }


        /// <summary>
        /// 获取任务主数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public NResult<ETaskInfo> QueryTask(QTask qTask)
        {
            NResult<ETaskInfo> result = new NResult<ETaskInfo>();
            try
            {
                using (OOContent db = new OOContent())
                {
                    var list = db.DBTaskInfo.Where(a=>a.TaskType!= TaskType.ALL);

                    if(qTask.RecordStatus != RecordStatus.ALL)
                    {
                        list = list.Where(a => a.RecordStatus == RecordStatus.Normal);
                    }

                    list = list.OrderByDescending(a => a.CreatedTime);

                    if (qTask.pageIndex == 0)
                    {
                        result.resultList =  list.Take(qTask.pageSize).ToList();
                    }
                    else
                    {
                        result.resultList = list.Skip(qTask.pageIndex * qTask.pageSize).Take(qTask.pageSize).ToList();
                    }
                   
                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;

                ErrorToDb(ex.Message);
            }
            return result;

        }
    }
}
