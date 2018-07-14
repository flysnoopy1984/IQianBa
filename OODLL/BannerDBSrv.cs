using IQBCore.DataBase;
using IQBCore.Model;
using IQBCore.OO.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OODBSrv
{

    public class BannerDBSrv: BaseDBSrv
    {

        public NResult<EBanner> GetLatestData(int showNum)
        {
            NResult<EBanner> result = base.GetLatestData<EBanner>(showNum);
            return result;
        }

        public int Insert(EBanner obj)
        {
            try
            {
                using (OOContent db = new OOContent())
                {
                    
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }
    }
}
