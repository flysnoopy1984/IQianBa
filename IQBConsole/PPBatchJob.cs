using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace IQBConsole
{
    public class PPBatchJob
    {

        int _OrderDiffMin = 60;

        public PPBatchJob()
        {
            
          
        }
        public void DeleteWaitingOrder()
        {
            using (IQBContent db = new IQBContent())
            {
                string sql = @"select count(*) as delCount from O2OOrder as o
where datediff(MINUTE,o.CreateDateTime,getdate()) >{0}";
                sql = string.Format(sql, _OrderDiffMin);
                int delNum = db.Database.SqlQuery<int>(sql).FirstOrDefault();
                if(delNum>0)
                {
                    Console.WriteLine(string.Format("【{0}】：开始删除等待上传且超时{1}分钟的订单",DateTime.Now.ToString(), delNum));
                    sql = @"delete from O2OOrder where datediff(MINUTE,O2OOrder.CreateDateTime,getdate()) >90";
                    int i = db.Database.ExecuteSqlCommand(sql);
                    Console.WriteLine(string.Format("【{0}】：完成删除{1}", DateTime.Now.ToString(), i));
                }
                else
                {
                    Console.WriteLine(string.Format("【{0}】：暂时没有数据删除", DateTime.Now.ToString()));
                }
            }
        }
        public void Run(int OrderDiffMin)
        {
            _OrderDiffMin = OrderDiffMin;
            DeleteWaitingOrder();
        }

       

      
    }
}
