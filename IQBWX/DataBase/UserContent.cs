using EntityFramework.Extensions;
using IQBCore.IQBWX.BaseEnum;
using IQBCore.IQBWX.Const;
using IQBWX.Common;
using IQBWX.Models.JsonData;
using IQBWX.Models.Results;
using IQBWX.Models.User;
using IQBWX.Models.WX;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Transactions;
using System.Web;


namespace IQBWX.DataBase
{
    public class UserContent : DbContext
    {
        public UserContent() : base("MainDBConnection")
        {
             this.Configuration.AutoDetectChangesEnabled = true;
            ChangeTracker.DetectChanges();
            //Database.SetInitializer<UserContent>(new CreateDatabaseIfNotExists<UserContent>());
        }

      

        public void ForInit()
        {
            UserInfo.FirstOrDefault<EUserInfo>(o=>o.openid=="1");           
            MemberInfo.FirstOrDefault<EMemberInfo>(o=>o.openId == "1");
            MemberChildren.FirstOrDefault<EMemberChildren>(o => o.cOpenId == "1");
            UserSMSVerify.FirstOrDefault<EUserSMSVerify>(v=>v.Id==1);
        }

        public DbSet<EUserInfo> UserInfo { get; set; }
        public DbSet<EMemberInfo> MemberInfo { get; set; }
        public DbSet<EMemberChildren> MemberChildren { get; set; }
        public DbSet<EUserSMSVerify> UserSMSVerify { get; set; }

        #region UserInfo    

        public int UserCount()
        {
            return UserInfo.Count();
        }

        public bool IsExistUserInfo(string openId)
        {
            int i = UserInfo.Count(u => u.openid == openId);
            return (i > 0);
        }
        public bool IsMember(string openId)
        {
            int i = UserInfo.Count(u => u.openid == openId && u.IsMember == true);
            return (i >= 1);
        }

        //public bool IsUserInfoPaid(string openId)
        //{
        //    int i = UserInfo.Count(u => u.openid == openId && u.PaymentState == PaymentState.paid);
        //    return i > 0;
        //}

        public string GetNickName(string openId)
        {
            var list = UserInfo.Where(u => u.openid == openId).Select(u => new { u.nickname }).ToList();
            return list.FirstOrDefault().nickname;
        }

        public int GetUserId(string openId)
        {
            var list = UserInfo.Where(u => u.openid == openId).Select(u => new { u.UserId }).ToList();
            return list.FirstOrDefault().UserId;
        }

        /// <summary>
        /// 提交会员申请时，更新userinfo.
        /// </summary>
        /// <returns></returns>
        public EUserInfo UpdateForApplyMember(jsonApplyMember data)
        {
            EUserInfo ui = UserInfo.FirstOrDefault<EUserInfo>(u => u.UserId ==data.UserId);
            ui.WXMemberSelected = data.WXNum;
            ui.ProvinceSeleced = data.Province;
            ui.PhoneNumber = data.userPhone;
            ui.UserName = data.userName;
            ui.ProvinceValue = data.ProvinceValue;
            ui.WXMemberRange = data.WXRange;
            ui.UserName = data.userName;
            return ui;           
        
        }

        /// <summary>
        /// 申请成功使用，更新Ismember，和支付状态,
        /// 同事创建Member;
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="selTc">套餐</param>
        /// <param name="sceneId">sceneId</param>
        /// <returns></returns>
        public EMemberInfo UpdateToMember(string openId, int selTc, string sceneId)
        {
            EUserInfo ui = UserInfo.FirstOrDefault(u => u.openid == openId && u.IsMember == false);
            EMemberInfo mi = null;
            if (ui != null)
            {
                //using (TransactionScope sc = new TransactionScope())
                //{
                    //更新userInfo 会员标记和支付状态
                    ui.IsMember = true;
                  //  ui.PaymentState = PaymentState.paid;

                    //新增会员到会员表
                    mi = ui.InitMember();
                    if (selTc == 1)
                        mi.MemberType = MemberType.Channel;
                    else
                        mi.MemberType = MemberType.City;
                    mi.SceneId = sceneId;
                    mi.UserId = ui.UserId;
                    mi.ParentOpenId = ui.ParentOpenId;
                    MemberInfo.Add(mi);
            }
            return mi;
        }

        ///// <summary>
        ///// 申请会员界面使用
        ///// </summary>
        ///// <param name="openId"></param>
        ///// <param name="ps"></param>
        //public void UpdatePaymentState(string openId, PaymentState ps)
        //{
        //    EUserInfo ui = UserInfo.FirstOrDefault<EUserInfo>(u => u.openid == openId);
        //    if (ui != null)
        //    {
        //        ui.PaymentState = ps;
        //        this.SaveChanges();
        //    }
        //    //UserInfo.Where<EUserInfo>(u=>u.openid == openId)
        //}

     

        public EUserInfo Get(string openId)
        {
            return UserInfo.FirstOrDefault<EUserInfo>(u => u.openid == openId);
        }

        public void InsertUserInfo(EUserInfo ui, bool needCheck = true)
        {
            IQBLog log = new IQBLog();
            try
            {
                bool isExist;
                if (!string.IsNullOrEmpty(ui.openid))
                {
                    if (needCheck)
                        isExist = IsExistUserInfo(ui.openid);
                    else
                        isExist = false;

                    if (!isExist)
                    {
                        this.UserInfo.Add(ui);

                        this.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                log.log("InsertUserInfo Error:" + ex.Message);
                log.log("InsertUserInfo Inner Error:" + ex.InnerException.Message);

            }
        }
        #endregion

        #region MemberInfo
        
    
        public RMemberQR GetMemberQRData(string openId)
        {
            List<RMemberQR> list = null;
            try
            {
                list = MemberInfo.Where(m=>m.openId == openId)
                .Select(m => new RMemberQR {openId= openId, NickName = m.nickname,UserId = m.UserId,HeaderImg =m.headimgurl }).ToList();
            }
            catch(Exception ex)
            {
                throw new Exception("GetMemberQRData Error" + ex.Message);
            }
            return list.FirstOrDefault();
            
        }

        public EUserInfo GetBySceneId(string sceneId)
        {
            EUserInfo ui = null;
            EMemberInfo mi = MemberInfo.FirstOrDefault<EMemberInfo>(m => m.SceneId == sceneId);
            if (mi != null)
            {
                ui = UserInfo.FirstOrDefault<EUserInfo>(u => u.UserId == mi.UserId);
            }
            return ui;
        }

        public MemberType GetMemberType(string openId)
        {
            var list = MemberInfo.Where(m => m.openId == openId).Select(u => new { u.MemberType }).ToList();
            return  list.FirstOrDefault().MemberType;       

        }

        public EMemberInfo GetMemberInfoByOpenId(string openId)
        {

            EMemberInfo mi = MemberInfo.FirstOrDefault<EMemberInfo>(m => m.openId == openId);
            return mi;
        }

      

        /// <summary>
        /// 根据当前用户openid获取父亲
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public string GetParentOpenId(string openId)
        {
            var member = MemberInfo.Where(m => m.openId == openId).Select(m => new { m.ParentOpenId }).ToList();
            return member.FirstOrDefault().ParentOpenId;

        }
        #endregion

        #region MemberRelation
        /// <summary>
        /// 递归函数，从叶子往上pop
        /// </summary>
        /// <param name="mi"></param>
        /// <param name="level"></param>
        public void BuildMemberRelation(EMemberInfo currentMember,EMemberInfo mi, ref Dictionary<int,EMemberInfo> results, int level)
        {
            IQBLog log = new IQBLog();
            try
            { 
                if (level == 0) level = 1;

                if (level > 9) return;

                if (string.IsNullOrEmpty(mi.ParentOpenId) || mi.ParentOpenId == "0" || mi.ParentOpenId == "null")
                    return;

                EMemberInfo pmi = this.GetMemberInfoByOpenId(mi.ParentOpenId);
                log.log("BuildMemberRelation pmi:" + pmi.nickname);

                //如果往上返3级后，开始出现普通会员，则不建立关系，直接找上级父节点
                if (level > 3 && mi.MemberType == MemberType.Channel)
                {
                    level++;
                    this.BuildMemberRelation(currentMember,pmi, ref results, level);
                }
          

                EMemberChildren rel = new EMemberChildren()
                {
                    pOpenId = mi.ParentOpenId,
                    cOpenId = currentMember.openId,
                    ChildLevel = level,
                    cMemberType = mi.MemberType,
                };
                log.log("BuildMemberRelation created relation");
                MemberChildren.Add(rel);
                if (results == null)
                    results = new Dictionary<int, EMemberInfo>();
                results.Add(level, pmi);
                log.log("BuildMemberRelation add relation level :"+level);

                level++;

                this.BuildMemberRelation(currentMember,pmi, ref results, level);
            }
            catch(Exception ex)
            {
              
                log.log("BuildMemberRelation Error:" + ex.Message);
                log.log("BuildMemberRelation InnerException:" + ex.InnerException);
            }
        }

        public int GetChildCount(string openId)
        {
            return this.MemberChildren.Count(m => m.pOpenId == openId);
        }

        public void UpdateToMemberLevel2(string openId)
        {   
          this.MemberInfo
                .Where(m=>m.openId == openId)
                 .Update(m => new EMemberInfo { MemberType = MemberType.City });

            this.MemberChildren
                .Where(m => m.cOpenId == openId)
                .Update(m=>new EMemberChildren { cMemberType = MemberType.City }) ;
        }

        public List<RMyChildren> GetMyChildrenData(string openId, int cLevel,int pageIndex=0)
        {
              List<RMyChildren> rlist = new List<RMyChildren>();
              IQueryable< RMyChildren> list = this.MemberChildren
                .Where(m => m.pOpenId == openId && m.ChildLevel == cLevel)
                .Join(this.MemberInfo, a => a.cOpenId, b => b.openId,
                   (a, b) => new RMyChildren
                   {
                       cNickName = b.nickname,
                       cOpenId = b.openId,
                     
                       cMemberType = b.MemberType,
                       cAddDateTime = b.RegisterDateTime,                      
                   });
            list = list.OrderByDescending(l => l.cAddDateTime);
            if (pageIndex == 0)
            {
                rlist = list.Take(10).ToList();
               
            }
            else
                rlist = list.Skip(pageIndex * 10).Take(10).ToList();


            foreach (RMyChildren obj in rlist)
            {
                string s = obj.cNickName;                
                obj.cNickName = s.Substring(0, 1) + "xxxxx" + s.Substring(s.Length - 1, 1);
                obj.cMemberTypeValue = IQBWXConst.GetMemberTypeValue(obj.cMemberType);
            }
            return rlist;
         
        }

        #endregion

        #region EUserSMSVerify

        /// <summary>
        /// 当验证码倒计时被刷新
        /// </summary>
        /// <returns></returns>
        public int GetVerifyDiff(int userId, string smsEvent)
        {
            var member = UserSMSVerify.Where(v => v.UserId == userId && v.SMSEvent == smsEvent).Select(v => new { v.SendDateTime }).ToList();
            if(member.Count>0)
            { 
                TimeSpan nowtimespan = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan endtimespan = new TimeSpan(member.FirstOrDefault().SendDateTime.Ticks);
                TimeSpan timespan = nowtimespan.Subtract(endtimespan).Duration();
                return Convert.ToInt32(timespan.TotalSeconds);
            }
            return -1;
        }
        public void InsertEUserSMSVerify(EUserSMSVerify obj)
        {
            var objs = UserSMSVerify.Where<EUserSMSVerify>(v => v.UserId == obj.UserId && v.SMSEvent == obj.SMSEvent);
            UserSMSVerify.RemoveRange(objs);

            UserSMSVerify.Add(obj);
            this.SaveChanges();

        }
        #endregion
    }

}