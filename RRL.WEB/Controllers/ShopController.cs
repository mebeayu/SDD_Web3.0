using System;
using System.Data;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Web.Mvc;

using RRL.Core.Manager;
using RRL.Core.Models;
using RRL.Core.Utility;

namespace RRL.WEB.Controllers
{
    // TASK #28 lcl 20180613 Insert
    public class ShopController : Controller
    {
        // TASK #28 lcl 20180613 Insert
        /// <summary>
        /// 商品分享的二维码
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="goodsId">商品ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GoodsShareQRCode(string token, int goodsId)
        {
            MemoryStream ms = new MemoryStream();

            UserAuth user = new UserAuth();
            if (!string.IsNullOrWhiteSpace(token))
            {
                TokenObject tokenObject = TokenObject.InitTokenObjFromString(token);
                // 获取用户信息
                user.Load(tokenObject.id);
            }
            string strQRCodeContent = new ShopManager().GetGoodsShareUrl(user.username, goodsId);

            PublicManager publicMgr = new PublicManager();
            // 生成二维码原始图
            using (var qrImg = publicMgr.GenerateQRCodeImage(strQRCodeContent.ToString()))
            {
                // 等比例缩小二维码图像
                using (var cutImg = publicMgr.CutToFixedSizeByEqualProportion(qrImg, 140, 140))
                {
                    cutImg.Save(ms, ImageFormat.Jpeg);
                }
            }

            return File(ms.ToArray(), "image/jpeg");
        }

        // TASK #28 lcl 20180613 Insert
        /// <summary>
        /// 商品的分享图片
        /// </summary>
        /// <param name="token"></param>
        /// <param name="goodsId">商品ID</param>
        /// <returns></returns>
        public ActionResult GoodsShareImage(string token, int goodsId, int goodsPictureId)
        {
            MemoryStream ms = new MemoryStream();
            ShopManager ShopMgr = new ShopManager();

            UserAuth user = new UserAuth();
            if (!string.IsNullOrWhiteSpace(token))
            {
                TokenObject tokenObject = TokenObject.InitTokenObjFromString(token);
                // 获取用户信息
                user.Load(tokenObject.id);
            }

            // 创建商品数据对象
            dynamic goodsData = new ExpandoObject();
            goodsData.qrCodeContent = ShopMgr.GetGoodsShareUrl(user.username, goodsId); // 二维码内容
            goodsData.pictureId = goodsPictureId; // 获取商品主图

            // 获取商品明细数据
            DataSet ds = ShopMgr.GetGoodsDetail(goodsId);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    goodsData.name = dr["name"]; // 商品名称
                    goodsData.propaganda = dr["propaganda"]; // 商品简介
                    goodsData.price = Convert.ToDecimal(dr["price"]).ToString("#.00"); // 商品单价
                    goodsData.cash_price = Convert.ToDecimal(dr["cash_price"]).ToString("#.##"); // 现金+金豆交易中的现金
                    goodsData.beans_price = Convert.ToDecimal(dr["beans_price"]).ToString("#.##"); // 现金+金豆交易中的金豆
                }
            }

            using (var shareImage = ShopMgr.GenerateGoodsShareImage(goodsData))
            {
                shareImage.Save(ms, ImageFormat.Jpeg);
            }

            return File(ms.ToArray(), "image/jpeg");
        }
    }
}