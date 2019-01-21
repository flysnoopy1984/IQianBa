using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace IQBCore.Common.Helper
{
    public class StringHelper
    {
        public static string GetRnd(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;

            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }

            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }

            return s;
        }

        public static string urlconvertor(string WebRoot, string imagesurl1)
        {

            string imagesurl2 = imagesurl1.Replace(WebRoot, ""); //转换成相对路径
            imagesurl2 = imagesurl2.Replace(@"\", @"/");
            return "/"+imagesurl2;
        }

        public static string GenerateOrderNo()
        {
            return "YJO" + DateTime.Now.ToString("yyyyMMddhhmmss") + GetRnd(2,true,true,false,false,"");
        }

        public static string GenerateO2ONo ()
        {
            return "O2O" + DateTime.Now.ToString("yyyyMMddhhmmss") + GetRnd(2, true, true, false, false, "");
        }

        public static string GenerateTransferNo(TransferTarget target)
        {
            string pix = "YJTO";
            switch(target)
            {
                case TransferTarget.Agent:
                    pix = "YJTOA";
                    break;
                case TransferTarget.ParentAgent:
                    pix = "YJTOPA";
                    break;
                case TransferTarget.User:
                    pix = "YJTOU";
                    break;
            }
            return pix + DateTime.Now.ToString("yyyyMMddhhmmss") + GetRnd(2, true, true, false, false, "");
        }

        public static string GenerateSubAccountTransNo()
        {
            return "YJSub" + DateTime.Now.ToString("yyyyMMddhhmm") + GetRnd(2, true, true, false, false, "");
        }

        public static string GenerateVerifyCode()
        {
            return GetRnd(6, true, false, false, false, "");
        }

        public static string GenerateReceiveNo()
        {
            return DateTime.Now.ToString("MMdd") + GetRnd(6, true, false, false, false, "");
        }

        public static string GetSSOToken()
        {
            DateTime dt = DateTime.Now;
            string sd = dt.ToString("yyyyMMddhhmmss");
            string r3 = GetRnd(3, false, true, true, false, "");

            return sd + r3;
        }

        public static bool IsPhoneNo(string str_handset)
        {
            return Regex.IsMatch(str_handset, "^(((13[0-9])|(15([0-3]|[5-9]))|(18[0-9])|(17[0-9])|(14[0-9]))\\d{8})$");
        }

        #region OO
        /// <summary>
        /// 手机后3位+3位随机数
        /// </summary>
        /// <returns></returns>
        public static string GenerateUserInviteCode(string phone)
        {

            string code = phone.Substring(4);
            code += GetRnd(3, true, false, false, false, "");
            return code;
        }
        public static string GenerateOONo()
        {
            return "OO" + DateTime.Now.ToString("yyyyMMddhhmmss") + GetRnd(2, true, true, false, false, "");
        }

        #endregion

        #region Game
        public static string GenerateRoomCode()
        {
            return DateTime.Now.ToString("MMdd") + GetRnd(4, true, false, false, false, "");
        }
        #endregion


    }
}