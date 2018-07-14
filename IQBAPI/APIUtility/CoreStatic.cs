using IQBCore.DataBase;
using IQBCore.OO.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBAPI.APIUtility
{
    public class CoreStatic
    {
        private static CoreStatic _Instance;

        public static CoreStatic Instance
        {
            get
            {
                if(_Instance == null)
                {
                    _Instance = new CoreStatic();
                }
                return _Instance;
            }
        }

        private ESysConfig _SysConfig;

        public ESysConfig Sys
        {
            get
            {
                if (_SysConfig == null)
                {
                    using (OOContent db = new OOContent())
                    {
                        _SysConfig = db.DBSysConfig.FirstOrDefault();
                        if(_SysConfig == null)
                        {
                            _SysConfig = new ESysConfig();
                            _SysConfig.ADRewardRate = 50;
                            _SysConfig.CurCurrencyCode = "OOB";
                            _SysConfig.Code = "OOAPISys";
                            _SysConfig.IntroRate = 2;
                            _SysConfig.L1RewardRate = 1;
                            _SysConfig.L2RewardRate = 1;
                            _SysConfig.L3RewardRate = 1;
                            db.DBSysConfig.Add(_SysConfig);
                            db.SaveChanges();

                        }
                    }
                }
                return _SysConfig;
            }
        }

        public void ResetSys()
        {
            _SysConfig = null;
        }
    }
}