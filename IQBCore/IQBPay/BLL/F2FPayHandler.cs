﻿
using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Com.Alipay.Business;
using Com.Alipay.Domain;
using Com.Alipay.Model;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBPay.Models.Sys;
using IQBCore.IQBPay.Models.Tool;
using IQBCore.IQBPay.Models.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace IQBCore.IQBPay.BLL
{
    public class F2FPayHandler
    {
        private string _OrderNo;
        public string OrderNo
        {
            get { return _OrderNo; }
        }

        private string _SellerId;
        public string SellerId
        {
            get { return _SellerId; }
        }

        public AlipayTradePrecreateResponse BuildNew(EAliPayApplication app,EStoreInfo store,EUserInfo AgentUI,string TotalAmt,bool needNotifyUrl=true,bool needControl = true)
        {
            string NotifyUrl = ConfigurationManager.AppSettings["Main_SiteUrl"] + "AliPay/PayNotify";
            _OrderNo = StringHelper.GenerateOrderNo();

            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
            app.Merchant_Private_Key, "json", "1.0", "RSA2", app.Merchant_Public_key, "GBK", false);

            AlipayTradePrecreateRequest request = new AlipayTradePrecreateRequest();
            AlipayTradePrecreateModel model = new AlipayTradePrecreateModel();

            model.SellerId = store.AliPayAccount;
            NLogHelper.InfoTxt("当面付OrderNo:" + _OrderNo);
            model.OutTradeNo = _OrderNo;
            model.TotalAmount = TotalAmt;
            model.Subject =  "#"+AgentUI.Name+ " 收银台";
            model.Body = app.AppName + "-商品";
            model.ExtendParams = new Aop.Api.Domain.ExtendParams();
            model.ExtendParams.SysServiceProviderId = app.AppId;
            if(needControl)
                model.DisablePayChannels = "balance,moneyFund,creditCard,creditCardExpress,creditCardCartoon,debitCardExpress,point,bankPay";

            request.SetBizModel(model);
            if(needNotifyUrl)
                request.SetNotifyUrl(NotifyUrl);

            AlipayTradePrecreateResponse response = aliyapClient.Execute(request, null, store.AliPayAuthToke);
            return response;
        }

        public AlipayTradePrecreateResponse BuildPartyPay(EAliPayApplication app, EStoreInfo store, EUserInfo AgentUI, string TotalAmt, bool needNotifyUrl = true)
        {
            string NotifyUrl = ConfigurationManager.AppSettings["Main_SiteUrl"] + "AliPay/PayNotify";
            _OrderNo = StringHelper.GenerateOrderNo();

            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
          app.Merchant_Private_Key, "json", "1.0", "RSA2", app.Merchant_Public_key, "GBK", false);

            AlipayTradePrecreateRequest request = new AlipayTradePrecreateRequest();
            AlipayTradePrecreateModel model = new AlipayTradePrecreateModel();

            model.SellerId = store.AliPayAccount;
            model.OutTradeNo = _OrderNo;
            model.TotalAmount = TotalAmt;
            model.Subject = "#" + AgentUI.Name + " 收银台";
            model.Body = app.AppName + "-商品";
            model.ExtendParams.HbFqNum = "3";
            model.ExtendParams.HbFqSellerPercent = "0";
            

            request.SetBizModel(model);
            if (needNotifyUrl)
                request.SetNotifyUrl(NotifyUrl);

            AlipayTradePrecreateResponse response = aliyapClient.Execute(request, null, store.AliPayAuthToke);
            return response;
        }

        public AlipayTradePrecreateContentBuilder BuildPrecreateContent(EAliPayApplication app,EUserInfo AgentUi, string sellerid,string TotalAmt)
        {
            //线上联调时，请输入真实的外部订单号。
            _OrderNo = StringHelper.GenerateOrderNo();
            _SellerId = sellerid;

             AlipayTradePrecreateContentBuilder builder = new AlipayTradePrecreateContentBuilder();
            //收款账号
            builder.seller_id = _SellerId;

      //      builder.store_id = _SellerId;

            //订单编号
            builder.out_trade_no = OrderNo;
            //订单总金额
            builder.total_amount = TotalAmt;
            
            //参与优惠计算的金额
            //builder.discountable_amount = "";
            //不参与优惠计算的金额
            //builder.undiscountable_amount = "";
            //订单名称
            builder.subject = "#"+ AgentUi.Name+"收银台";
            //自定义超时时间
          //  builder.timeout_express = "5m";
            //订单描述
            builder.body = "#" + app.AppName + "-商品";
            //门店编号，很重要的参数，可以用作之后的营销
            // builder.store_id = "test store id";
            //操作员编号，很重要的参数，可以用作之后的营销
            // builder.operator_id = "test";

            //传入商品信息详情
            //List<GoodsInfo> gList = new List<GoodsInfo>();
            //GoodsInfo goods = new GoodsInfo();
            //goods.goods_id = "爱钱吧商品";
            //goods.goods_name = "爱钱吧商品";
            //goods.price = TotalAmt;
            //goods.quantity = "1";
            //gList.Add(goods);
            //List<GoodsInfo> gList = AliPayManager.GetGoodsList(TotalAmt);
            //builder.goods_detail = gList;

            //系统商接入可以填此参数用作返佣
            //ExtendParams exParam = new ExtendParams();
            //exParam.sys_service_provider_id = app.AppId;

            //builder.extend_params = exParam;



            return builder;

        }

        public AlipayTradePrecreateContentBuilder BuildPrecreateContent_ForR(EAliPayApplication app, string sellerid, string TotalAmt)
        {
            //线上联调时，请输入真实的外部订单号。
            _OrderNo = StringHelper.GenerateOrderNo();
            _SellerId = sellerid;

            AlipayTradePrecreateContentBuilder builder = new AlipayTradePrecreateContentBuilder();
            //收款账号
            builder.seller_id = _SellerId;
           
            //订单编号
            builder.out_trade_no = OrderNo;
            //订单总金额
            builder.total_amount = TotalAmt;
            //参与优惠计算的金额
            //builder.discountable_amount = "";
            //不参与优惠计算的金额
            //builder.undiscountable_amount = "";
            //订单名称
            builder.subject = "#" + app.AppName + "-"  + "收银";
            //自定义超时时间
            //  builder.timeout_express = "5m";
            //订单描述
           
            //门店编号，很重要的参数，可以用作之后的营销
            // builder.store_id = "test store id";
            //操作员编号，很重要的参数，可以用作之后的营销
            // builder.operator_id = "test";

            //传入商品信息详情
            //List<GoodsInfo> gList = new List<GoodsInfo>();
            //GoodsInfo goods = new GoodsInfo();
            //goods.goods_id = "爱钱吧商品";
            //goods.goods_name = "爱钱吧商品";
            //goods.price = TotalAmt;
            //goods.quantity = "1";
            //gList.Add(goods);
            //List<GoodsInfo> gList = AliPayManager.GetGoodsList(TotalAmt);
            //builder.goods_detail = gList;

            builder.body = "#玉杰商品";

            //系统商接入可以填此参数用作返佣
            //ExtendParams exParam = new ExtendParams();
            //exParam.sys_service_provider_id = app.AppId;

            //builder.extend_params = exParam;



            return builder;

        }

        public string CreateQR_ForR(AlipayF2FPrecreateResult precreateResult, ETool_QR qr)
        {
            Bitmap bt;
            string filePath, virtualPath;
            string enCodeString = precreateResult.response.QrCode;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            qrCodeEncoder.QRCodeScale = 3;
            qrCodeEncoder.QRCodeVersion = 8;
            bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
            string filename = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString()
             + ".jpg";
 
            filePath = "/Content/QR/Tools/" + filename;

            Bitmap bkImg = new Bitmap(160, 180);


            Graphics g = Graphics.FromImage(bkImg);

            //获取水印位置设置
            ArrayList loca = new ArrayList();
            int x = 0;
            int y = 0;
            x = 5;
            y= 6;
         //   x = bkImg.Width / 2 - waterimg.Width / 2 - 90;
         //   y = MainImg.Height / 2 - 260;
            loca.Add(x);
            loca.Add(y);


            g.DrawImage(bt, new Rectangle(int.Parse(loca[0].ToString()), int.Parse(loca[1].ToString()), 149, 149));

            string s = "输入金额："+qr.InputAmt;
            FontFamily ff = new FontFamily("黑体");
            Font f = new Font(ff,10);
            SolidBrush b = new SolidBrush(Color.Black);

            g.DrawString(s, f, b, 5, 155);
          //  Bitmap finImg = ImgHelper.ImageWatermark(bkImg, bt);

            virtualPath = filePath;



            filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            bkImg.Save(filePath);

            bkImg.Dispose();
            bt.Dispose();
           return virtualPath;

           
        }

        public string CreateF2FQR(string QRUrl, bool ForTool = false)
        {
            Bitmap bt;
            string filePath, virtualPath;
            string enCodeString = QRUrl;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            qrCodeEncoder.QRCodeScale = 3;
            qrCodeEncoder.QRCodeVersion = 8;
            bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
            string filename = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString()
             + ".jpg";

            filePath = ConfigurationManager.AppSettings["QR_F2F_FP"] + filename;
            if (ForTool)
            {
                filePath = "/Content/QR/Tools/" + filename;
            }

            virtualPath = filePath;

          

            filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            bt.Save(filePath);

            if (ForTool)
                return virtualPath;

            // string ds = DeQR(filePath);
            return filePath;

        }

        public string CreateQR(AlipayF2FPrecreateResult precreateResult,bool ForTool = false)
        {
            Bitmap bt;
            string filePath,virtualPath;
            string enCodeString = precreateResult.response.QrCode;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            qrCodeEncoder.QRCodeScale = 3;
            qrCodeEncoder.QRCodeVersion = 8;
            bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
            string filename = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString()
             + ".jpg";   
            
            filePath = ConfigurationManager.AppSettings["QR_F2F_FP"] + filename;
            if (ForTool)
            {
                filePath = "/Content/QR/Tools/" + filename;
            }
            virtualPath = filePath;

            filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            bt.Save(filePath);

            if (ForTool)
                return virtualPath;

           // string ds = DeQR(filePath);
            return filePath;

        }

        public string CreateQR(string ImageUrl)
        {
            Bitmap bt;
            string filePath;
            string enCodeString = ImageUrl;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            qrCodeEncoder.QRCodeScale = 3;
            qrCodeEncoder.QRCodeVersion = 0;
            bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
            string filename = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString()
             + ".jpg";
            filePath = ConfigurationManager.AppSettings["QRPath"] + filename;
            bt.Save(filePath);

            // string ds = DeQR(filePath);
            return filePath;

        }

        public string DeQR(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                return null;
            Bitmap myBitmap = new Bitmap(System.Drawing.Image.FromFile(filePath));
            QRCodeDecoder decoder = new QRCodeDecoder();
            string decodedString = decoder.decode(new QRCodeBitmapImage(myBitmap));
            return decodedString;
        }


    }
}