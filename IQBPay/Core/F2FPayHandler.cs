
using Com.Alipay.Business;
using Com.Alipay.Domain;
using Com.Alipay.Model;
using IQBCore.Common.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace IQBPay.Core
{
    public class F2FPayHandler
    {
        public AlipayTradePrecreateContentBuilder BuildPrecreateContent(string sellerid,string TotalAmt)
        {
            //线上联调时，请输入真实的外部订单号。
            string out_trade_no = StringHelper.GenerateOrderNo();
           

            AlipayTradePrecreateContentBuilder builder = new AlipayTradePrecreateContentBuilder();
            //收款账号
            builder.seller_id = sellerid;

           
            //订单编号
            builder.out_trade_no = out_trade_no;
            //订单总金额
            builder.total_amount = TotalAmt;
            //参与优惠计算的金额
            //builder.discountable_amount = "";
            //不参与优惠计算的金额
            //builder.undiscountable_amount = "";
            //订单名称
            builder.subject = "爱钱吧币";
            //自定义超时时间
          //  builder.timeout_express = "5m";
            //订单描述
            builder.body = "至尊宝";
            //门店编号，很重要的参数，可以用作之后的营销
          //  builder.store_id = "test store id";
            //操作员编号，很重要的参数，可以用作之后的营销
           // builder.operator_id = "test";

            //传入商品信息详情
            List<GoodsInfo> gList = new List<GoodsInfo>();
            GoodsInfo goods = new GoodsInfo();
            goods.goods_id = "goods id";
            goods.goods_name = "goods name";
            
            goods.price = "0.01";
            goods.quantity = "1";
            gList.Add(goods);
            builder.goods_detail = gList;

            //系统商接入可以填此参数用作返佣
            ExtendParams exParam = new ExtendParams();
            exParam.sys_service_provider_id = AliPayConfig.pid;
            
            builder.extend_params = exParam;

            return builder;

        }

        public string CreateQR(AlipayF2FPrecreateResult precreateResult)
        {
            Bitmap bt;
            string filePath;
            string enCodeString = precreateResult.response.QrCode;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            qrCodeEncoder.QRCodeScale = 3;
            qrCodeEncoder.QRCodeVersion = 8;
            bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
            string filename = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString()
             + ".jpg";
            filePath = ConfigurationManager.AppSettings["QRPath"] + filename;
            bt.Save(filePath);

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
            Bitmap myBitmap = new Bitmap(Image.FromFile(filePath));
            QRCodeDecoder decoder = new QRCodeDecoder();
            string decodedString = decoder.decode(new QRCodeBitmapImage(myBitmap));
            return decodedString;
        }
    }
}