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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
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
        public NResult<EUserInfo> Login(string loginName,string pwd)
        {
            NResult<EUserInfo> result = new NResult<EUserInfo>();
            try
            {
                using (OOContent db = new OOContent())
                {

                    EUserInfo ui = db.DBUserInfo.Where(a => a.LoginName == loginName && a.Pwd == pwd).FirstOrDefault();
                    //var sql = @"select ui.Id,ui.NickName,
                    //                   ui.Phone,ui.UserRole,
                    //                   ui.HeaderImgUrl,
                    //                   ui.RecordStatus 
                    //            from UserInfo as ui
                    //            where ui.LoginName = @LoginName and ui.Pwd = @Pwd";
                    //sql = string.Format(sql, loginName, pwd);
                    //List<SqlParameter> pList = new List<SqlParameter>();
                    //pList.Add(new SqlParameter("@LoginName", loginName));
                    //pList.Add(new SqlParameter("@Pwd", pwd));

                    //RUserInfo ui = db.Database.SqlQuery<RUserInfo>(sql, pList.ToArray()).FirstOrDefault();
                    if(ui == null)
                    {
                        result.ErrorMsg = "用户名或密码错误";
                        return result;
                    }
                    else
                    {
                        ui.LastLoginDateTime = DateTime.Now;
                        db.SaveChanges();    
                    }
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
                        ui.RegisterDateTime = DateTime.Now;
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
        /// 获取用户团队成员
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

        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public OutAPIResult UpdateHeaderImage()
        {
            OutAPIResult result = new OutAPIResult();
            Stream stream;
            int maxSize = 10;
            string phone = HttpContext.Current.Request["UserPhone"];
            try
            {
                if (string.IsNullOrEmpty(phone))
                {
                    result.ErrorMsg = "【B】手机号没有获取";
                    return result;
                }

                HttpPostedFile file0 = HttpContext.Current.Request.Files[0];

                int size = file0.ContentLength / 1024; //文件大小KB
                if (size > maxSize*1024)
                {
                    result.ErrorMsg = string.Format("文件过大，不能超过{0}M", maxSize);
                    return result;
                }
                byte[] fileByte = new byte[2];//contentLength，这里我们只读取文件长度的前两位用于判断就好了，这样速度比较快，剩下的也用不到。
                stream = file0.InputStream;
                stream.Read(fileByte, 0, 2);//contentLength，还是取前两位
                                          
                string fileFlag = "";
                if (fileByte != null || fileByte.Length <= 0)//图片数据是否为空
                {
                    fileFlag = fileByte[0].ToString() + fileByte[1].ToString();
                }
                //extDir.Add("255216", "jpg");
                if (fileFlag != "255216")
                {
                    result.ErrorMsg = "图片格式不正确";
                    return result;
                }
              
                string fileName = "user_" + phone + ".jpg";
                string saveFullPath = SysConfig.ImageSaveDir + "\\"+ fileName;
                NLogHelper.InfoTxt("Save Paht:" + saveFullPath);

                OutAPIResult updateResult = ImgHelper.UploadImg(saveFullPath);
                if(updateResult.IsSuccess)
                {
                    using (OOContent db = new OOContent())
                    {
                        var dbImgUrl = SysConfig.ImageSaveDBRoot + fileName;
                        var pList = new List<SqlParameter>();
                        pList.Add(new SqlParameter("@imgUrl", dbImgUrl));
                        pList.Add(new SqlParameter("@Phone", phone));
                        var sql = @"update UserInfo
                                    set HeaderImgUrl = @imgUrl
                                    where Phone = @Phone
                                    ";
                        db.Database.ExecuteSqlCommand(sql, pList.ToArray());
                    }
                }
                else
                {
                    result.ErrorMsg = result.ErrorMsg;
                }
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 更新用户昵称
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public OutAPIResult SetNickName()
        {
            OutAPIResult result = new OutAPIResult();
            string NickName = HttpContext.Current.Request["NickName"];
            string phone = HttpContext.Current.Request["Phone"];
            NLogHelper.InfoTxt(string.Format("Set Nick Name:{0}.Phone:{1}",NickName, phone));
            try
            {
                if(string.IsNullOrEmpty(phone))
                {
                    result.ErrorMsg = "【B】手机号没有获取";
                    return result;
                }
                if (string.IsNullOrEmpty(NickName))
                {
                    result.ErrorMsg = "【B】昵称没有获取";
                    return result;
                }
                using (OOContent db = new OOContent())
                {
                    var pList = new List<SqlParameter>();
                    pList.Add(new SqlParameter("@NickName", NickName));
                    pList.Add(new SqlParameter("@Phone", phone));
                    var sql = @"update UserInfo
                                    set NickName = @NickName
                                    where Phone = @Phone
                                    ";
                    result.IntMsg = db.Database.ExecuteSqlCommand(sql, pList.ToArray());
                    if(result.IntMsg<=0)
                    {
                        result.ErrorMsg = "没有找到对应的用户更新";
                    }
                }
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;

        }

        [HttpPost]
        public OutAPIResult ModifyPwd()
        {
            string phone =HttpContext.Current.Request["Phone"];
            string newPwd = HttpContext.Current.Request["newPwd"];
            string oldPwd = HttpContext.Current.Request["oldPwd"];
            OutAPIResult result = new OutAPIResult();
            try
            {
                if(string.IsNullOrEmpty(phone))
                {
                    result.ErrorMsg = "手机号不能为空！";
                    return result;
                }
                if (string.IsNullOrEmpty(newPwd))
                {
                    result.ErrorMsg = "新密码不能为空！";
                    return result;
                }
                using (OOContent db = new OOContent())
                {
                    EUserInfo ui =  db.DBUserInfo.Where(a => a.Phone == phone).FirstOrDefault();
                    if(ui == null)
                    {
                        result.ErrorMsg = "手机对应的用户没有找到";
                        return result;
                    }
                    if(ui.Pwd != oldPwd)
                    {
                        result.ErrorMsg = "旧密码不匹配";
                        return result;
                    }
                    else
                    {
                        ui.Pwd = newPwd;
                        db.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }

            return result;

        }
    }
}
