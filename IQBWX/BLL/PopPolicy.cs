using IQBCore.IQBWX.BaseEnum;
using IQBWX.Common;
using IQBWX.DataBase;
using IQBWX.Models.Order;
using IQBWX.Models.Product;
using IQBWX.Models.Transcation;
using IQBWX.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace IQBWX.BLL
{
    public class PopPolicy
    {
        EMemberInfo _mi = null;
        EMemberInfo _parent = null;
        EOrderLine _order = null;
        IQBLog _log;

        public PopPolicy()
        {
            _log = new IQBLog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relations">从你上级开始到上9级的父节点</param>
        public void RunCommession(EMemberInfo curMI,Dictionary<int, EMemberInfo> relations)
        {
            EMemberInfo pmi = null;
            int level;
            UserContent userDB = null;
            TransContent transDB = null;
            decimal recAmt;
            try
            { 
                //没有上级
                if (_parent == null)
                    return;
               
                MemberType currentOrderType = _order.ItemId == EItemInfo.Item158 ? MemberType.Channel : MemberType.City;
                userDB = new UserContent();
                transDB = new TransContent();
                using (TransactionScope sc = new TransactionScope())
                { 
                    //Key为第几级孩子
                    foreach (KeyValuePair<int, EMemberInfo> obj in relations)
                    {
                        pmi = obj.Value;
                        level = obj.Key;
                        //获取佣金金额
                        recAmt = GetPopLevelCommission(currentOrderType, pmi.MemberType, level);
                        //更新受益者会员相关资金字段
                        pmi.InComeAmount(recAmt);
                        userDB.Entry<EMemberInfo>(pmi).State = System.Data.Entity.EntityState.Modified;
                        //更新受益者收入交易记录
                        EARUserTrans arObj = transDB.InitARTrans(_order, pmi, level, recAmt);
                        arObj.TransRemark = string.Format("{0} 从下级会员【{1}】获取佣金", DateTime.Now.ToString("yyyy-MM,dd"),curMI.nickname);
                        transDB.ARTransDbSet.Add(arObj);

                    }
                    transDB.SaveChanges();
                    userDB.SaveChanges();
                    sc.Complete();                    
                }
            }
            catch(Exception ex)
            {
                _log.log("RunCommession Error:" + ex.Message);
                _log.log("RunCommession Stack:" + ex.StackTrace);
            }
            finally
            {
                transDB.Dispose();
                userDB.Dispose();
            }

        }

        public PopPolicy(EOrderLine order)
        {
            using (UserContent db = new UserContent())
            {
                _mi = db.GetMemberInfoByOpenId(order.OpenId);
                _parent = db.GetMemberInfoByOpenId(_mi.ParentOpenId);
            }
            _order = order;
        }

        public PopPolicy(EOrderLine order, EMemberInfo mi, EMemberInfo parent)
        {
            _mi = mi;
            _parent = parent;
            _order = order;
        }


        /// <summary>
        /// 下级佣金 ,暂时写死，之后写入数据库配置
        /// </summary>
        /// <param name="currentMemberType">当前会员类型（购买的商品）</param>
        /// <param name="pMemberType">父节点会员类型（受益者会员类型）</param>
        /// <param name="level">第几级父节点</param>
        /// <returns></returns>
        public static decimal GetPopLevelCommission(MemberType currentMemberType, MemberType pMemberType,int level=1)
        {  
            switch(level)
            {
                case 1:
                    if (currentMemberType == MemberType.Channel)
                    {
                        if (pMemberType == MemberType.Channel)
                            return 35;
                        else if (pMemberType == MemberType.City)
                            return 60;
                    }
                    else if (currentMemberType == MemberType.City)
                    {
                        if (pMemberType == MemberType.Channel)
                            return 80;
                        else if (pMemberType == MemberType.City)
                            return 90;
                    }
                    break;
                case 2:
                    if (currentMemberType == MemberType.Channel)
                    {
                        if (pMemberType == MemberType.Channel)
                            return 20;
                        else if (pMemberType == MemberType.City)
                            return 30;
                    }
                    else if (currentMemberType == MemberType.City)
                    {
                        if (pMemberType == MemberType.Channel)
                            return 40;
                        else if (pMemberType == MemberType.City)
                            return 50;
                    }
                    break;
                case 3:
                    if (currentMemberType == MemberType.Channel)
                    {
                        if (pMemberType == MemberType.Channel)
                            return 25;
                        else if (pMemberType == MemberType.City)
                            return 30;
                    }
                    else if (currentMemberType == MemberType.City)
                    {
                        if (pMemberType == MemberType.Channel)
                            return 50;
                        else if (pMemberType == MemberType.City)
                            return 50;
                    }
                    break;
                case 4:
                    if (currentMemberType == MemberType.Channel)
                    {
                        if (pMemberType == MemberType.Channel)
                            return 0;
                        else if (pMemberType == MemberType.City)
                            return 30;
                    }
                    else if (currentMemberType == MemberType.City)
                    {
                        if (pMemberType == MemberType.Channel)
                            return 0;
                        else if (pMemberType == MemberType.City)
                            return 30;
                    }
                    break;

            }          
            
            return 0;

        
        }
        
      

       
    }
}