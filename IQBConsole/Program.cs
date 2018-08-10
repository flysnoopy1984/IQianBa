using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace IQBConsole
{
    class Program
    {
        static PPBatchJob job = new PPBatchJob();
       
        static int _OrderDiffMin = 90;
        static int _interval = 60;

        static void Main(string[] args)
        {
          
            try
            {
                //Console.Write("请输入需要删除的订单间隔时间：");
                //_OrderDiffMin = Convert.ToInt32(Console.ReadLine());

                //while(true)
                //{
                //    job.Run(_OrderDiffMin);
                //    Thread.Sleep(_interval * 1000 * 60);
                //}
                //  job.ChangeReceiveQR();
                // job.UpdateAgentRate();
                //  job.Test();
                job.WXNTPayTellAdmin("o3nwE0qI_cOkirmh_qbGGG-5G6B0");
                Console.Read();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
            finally
            {
              
            }
            
        }



       

      
    }
}
