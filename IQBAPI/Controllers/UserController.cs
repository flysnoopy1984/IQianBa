using IQBAPI.APIUtility;
using IQBCore.Common.Helper;
using IQBCore.DataBase;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.Model;
using IQBCore.OO.Models.Entity;
using IQBCore.OO.Models.In;
using IQBCore.OO.Models.QueryResult;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;

namespace IQBAPI.Controllers
{
    public class UserController : BaseAPIController
    {
        /// <summary>
        /// 用户登录，返回用户基本信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public NResult<RUserInfo> Login(string loginName,string pwd)
        {
            NResult<RUserInfo> result = new NResult<RUserInfo>();
            try
            {
                using (OOContent db = new OOContent())
                {
                    var sql = @"select ui.Id,ui.NickName,
                                       ui.Phone,ui.UserRole,
                                       ui.HeaderImgUrl,
                                       ui.RecordStatus 
                                from UserInfo as ui
                                where ui.LoginName = @LoginName and ui.Pwd = @Pwd";
                    sql = string.Format(sql, loginName, pwd);
                    List<SqlParameter> pList = new List<SqlParameter>();
                    pList.Add(new SqlParameter("@LoginName", loginName));
                    pList.Add(new SqlParameter("@Pwd", pwd));

                    RUserInfo ui = db.Database.SqlQuery<RUserInfo>(sql, pList.ToArray()).FirstOrDefault();
                    if(ui == null)
                    {
                        result.ErrorMsg = "用户名或密码错误";
                        return result;
                    }
                    ui.LoginName = loginName;
                    ui.Pwd = pwd;
                    result.resultObj = ui;
                }

              
            }
            catch(Exception ex)
            {
               
                result.ErrorMsg = ex.Message;
                ErrorToDb(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userReg"></param>
        /// <returns></returns>
        [HttpPost]
        public NResult<EUserInfo> Register(InUserReg userReg)
        {

            NResult<EUserInfo> result = new NResult<EUserInfo>();
            EUserInfo ui = null;
            EUserQRInvite pQR = null;

            try
            {
                if(string.IsNullOrEmpty(userReg.Phone))
                {
                    result.ErrorMsg = "手机号不能未空";
                    return result;
                }
                if (string.IsNullOrEmpty(userReg.Pwd))
                {
                    result.ErrorMsg = "密码不能为空";
                    return result;
                }
           
                using (OOContent db = new OOContent())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        //创建用户基本信息
                        ui = db.DBUserInfo.Where(a => a.Phone == userReg.Phone).FirstOrDefault();
                        if (ui != null)
                        {
                            result.ErrorMsg = "手机号已存在";
                            return result;
                        }
                     
                        ui = new EUserInfo();
                        //如果昵称或登陆名为空，则用手机号补充
                        if (string.IsNullOrEmpty(userReg.LoginName))
                            ui.LoginName = userReg.Phone;
                        if (string.IsNullOrEmpty(userReg.NickName))
                            ui.NickName = userReg.Phone;
                        

                        ui.Phone = userReg.Phone;
                        ui.Pwd = userReg.Pwd;
                        ui.UserRole = IQBCore.OO.BaseEnum.UserRole.User;
                        ui.RecordStatus = IQBCore.OO.BaseEnum.RecordStatus.Normal;

                        db.DBUserInfo.Add(ui);

                        //检查邀请码
                        if (!string.IsNullOrEmpty(userReg.InviteCode))
                        {
                            pQR = db.DBUserQRInvite.Where(a => a.InviteCode == userReg.InviteCode).FirstOrDefault();
                            if (pQR == null)
                            {
                                result.ErrorMsg = "邀请码没有找到对应的用户";
                                return result;
                            }
                        }
                        db.SaveChanges();

                        //创建用户邀请码
                        EUserQRInvite qr = new EUserQRInvite
                        {
                            InviteCode = StringHelper.GenerateUserInviteCode(ui.Phone),
                            QRPath = "",
                            QRUrl = "",
                            UserId = ui.Id
                        };
                        db.DBUserQRInvite.Add(qr);

                        //用户关系
                        EUserRelation ur = new EUserRelation();
                        ur.UserId = ui.Id;
                        ur.UserName = ui.NickName;
                       
                        if (pQR != null)
                            ur.PId = pQR.UserId;
                        db.DBUserRelation.Add(ur);

                        //用户账户
                        EUserBalance ub = new EUserBalance();
                        ub.UserId = ui.Id;
                        ub.Balance = 0;
                        ub.CurrencyCode = CoreStatic.Instance.Sys.CurCurrencyCode;
                        db.DBUserBalance.Add(ub);

                        //用户回报
                        EUserReward reward = new EUserReward();
                        reward.UserId = ui.Id;
                        reward.ADRewardRate = CoreStatic.Instance.Sys.ADRewardRate;
                        reward.OrderRewardRate = CoreStatic.Instance.Sys.L1RewardRate;
                        reward.IntroRate = CoreStatic.Instance.Sys.IntroRate;
                        db.DBUserReward.Add(reward);

                        db.SaveChanges();

                        ts.Complete();
                    }
  
                    result.resultObj = ui;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                ErrorToDb(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 用户团队成员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public NResult<EUserInfo> QueryTeamMember()
        {
            NResult<EUserInfo> result = new NResult<EUserInfo>();
            try
            {

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        /// <summary>
        /// 获取用户邀请信息
        /// </summary>
        /// <returns></returns>
        public OutAPIResult InviteInfo()
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (OOContent db = new OOContent())
                {

                }
            }
            catch(Exception ex)
            {
                ErrorToDb(ex.Message);
            }
            return result;
        }

        [HttpPost]
        public OutAPIResult UpdateHeaderImage()
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
               
                string phone = System.Web.HttpContext.Current.Request["UserPhone"];
                string saveFullPath = SysConfig.ImageSaveDir + "\\user_"+phone;

                NLogHelper.InfoTxt("Start UploadImge");
                NLogHelper.InfoTxt("Save Paht:" + saveFullPath);
                ImgHelper.UploadImg(saveFullPath);
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

    }
}
