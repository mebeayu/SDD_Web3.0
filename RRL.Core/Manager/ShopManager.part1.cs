// TASK #28 lcl 20180614 Insert

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Dynamic;
using System.IO;
using System.Text;

using RRL.Config;
using RRL.DB;

namespace RRL.Core.Manager
{
    public partial class ShopManager
    {
        PublicManager publicMgr = null;

        /// <summary>
        /// 获取用于分享的商品url
        /// </summary>
        /// <param name="username">手机号码</param>
        /// <param name="goodsId">商品ID</param>
        /// <returns></returns>
        public string GetGoodsShareUrl(string username, int goodsId)
        {
            /* 
            * 用于分享的商品url格式：https://e-shop.rrlsz.com.cn/#/goodsdetail/15164/null?reference=13428711799
            * 15164：商品ID；
            * 13428711799：分享的用户的手机号码
            */
            StringBuilder sbGoodsShareUrl = new StringBuilder();
            sbGoodsShareUrl.Append(RRLConfig.LiveSiteUrl); // 即使username或者goodsId无效，也可以生成3.0首页的二维码

            if (!string.IsNullOrWhiteSpace(username) && goodsId > 0)
            {
                // 如果用户身份和商品ID是有效的，则拼接成完整的商品分享地址
                sbGoodsShareUrl.Append($@"/#/goodsdetail/{goodsId}/null?reference={username}");
            }

            return sbGoodsShareUrl.ToString();
        }

        /// <summary>
        /// 生成商品分享的图片
        /// </summary>
        /// <param name="goodsData">商品数据</param>
        /// <returns></returns>
        public Image GenerateGoodsShareImage(dynamic goodsData)
        {
            if (publicMgr == null)
            {
                publicMgr = new PublicManager();
            }

            string strBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // 底图图片完整名称
            string strBaseFullname = Path.Combine(strBaseDirectory, @"Assets\GoodsShare\Img\share_img_bg.png");
            // 价格区域背景图片完整名称
            string strPriceBgFullname = Path.Combine(strBaseDirectory, @"Assets\GoodsShare\Img\price_img_bg.png");
            // 定义图片对象
            Image imgBase = null, imgPriceBg = null;
            // 定义绘图图面
            Graphics gBase = null;

            try
            {
                imgBase = new Bitmap(strBaseFullname);
                imgPriceBg = new Bitmap(strPriceBgFullname);
                // 绘制文本区域内容
                //DrawTextArea(imgTextArea, stepData);

                gBase = Graphics.FromImage(imgBase);
                // 以下Graphics的三个属性，用来设置图片为高质量
                gBase.CompositingQuality = CompositingQuality.HighQuality;
                gBase.SmoothingMode = SmoothingMode.HighQuality;
                gBase.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // 绘制商品主图
                DrawMainPictureForShare(gBase, imgBase, goodsData.pictureId);
                // 绘制商品说明
                DrawNameForShare(gBase, goodsData);
                // 绘制商品价格
                DrawPriceForShare(gBase, imgBase, imgPriceBg, goodsData);

                // 将二维码图片绘制到底图上
                // 生成二维码原始图
                using (var qrImg = publicMgr.GenerateQRCodeImage(goodsData.qrCodeContent))
                {
                    // 等比例缩小二维码图像
                    using (Image cutImg = publicMgr.CutToFixedSizeByEqualProportion(qrImg, 220, 220))
                    {
                        // 加边框
                        using (Graphics g = Graphics.FromImage(cutImg))
                        {
                            using (Brush brush = new SolidBrush(Color.FromArgb(228, 229, 233)))
                            {
                                using (Pen pen = new Pen(brush, 4f))
                                {
                                    pen.DashStyle = DashStyle.Solid;
                                    g.DrawRectangle(pen, 0, 0, cutImg.Width, cutImg.Height);
                                }
                            }
                        }
                        // 绘制二维码
                        gBase.DrawImage(cutImg, 60f, imgBase.Height - cutImg.Height - 220f);
                    }
                }

            }
            finally
            {
                if (imgPriceBg != null)
                {
                    imgPriceBg.Dispose();
                }
                if (gBase != null)
                {
                    gBase.Dispose();
                }
            }

            return imgBase;
        }

        /// <summary>
        /// 绘制分享图片中的主图
        /// </summary>
        /// <param name="gBase">底图的图面</param>
        /// <param name="imgBase">底图图像</param>
        /// <param name="pictureId">主图ID</param>
        private void DrawMainPictureForShare(Graphics gBase, Image imgBase, int pictureId)
        {
            byte[] imgByte = publicMgr.getPicture(pictureId);
            if (imgByte.Length > 0)
            {
                MemoryStream msMainImg = new MemoryStream(imgByte);
                using (Image mainImg = new Bitmap(msMainImg))
                {
                    // 主图宽度 = 底图宽度 - 左右两侧留白宽度
                    float fWidth = imgBase.Width - 140 * 2;
                    // 主图高度按照宽度等比缩放
                    float fHeight = mainImg.Height * (fWidth / mainImg.Width);
                    // 等比例缩放图像
                    using (Image cutImg = publicMgr.CutToFixedSizeByEqualProportion(mainImg, Convert.ToInt32(fWidth), Convert.ToInt32(fHeight)))
                    {
                        // 绘制主图
                        gBase.DrawImage(cutImg, (imgBase.Width - cutImg.Width) / 2f, 180f);
                    }
                }
            }
        }

        /// <summary>
        /// 绘制分享图片中的商品说明
        /// </summary>
        /// <param name="gBase">底图的图面</param>
        /// <param name="goodsData">商品数据</param>
        private void DrawNameForShare(Graphics gBase, dynamic goodsData)
        {
            // 定义商品名称、商品简介的字体
            Font fontName = new Font("Arial", 20, FontStyle.Bold), fontPropaganda = new Font("Arial", 16);
            // 定义画笔
            Brush brush = new SolidBrush(Color.Black);
            // 定义字体尺寸变量
            SizeF sizeF = new SizeF();

            // 绘制商品名称
            // 商品名称区域允许的最大宽度
            float fMaxWidthForName = 430f;
            string strName = goodsData.name;
            if (!string.IsNullOrWhiteSpace(strName))
            {
                // 根据指定的字体，计算得出的最大中文字数
                int intMaxNumberForWord = 0;
                // 应用该字体的一个中文字的尺寸
                sizeF = gBase.MeasureString(strName.Substring(0, 1), fontName);
                // 在规定的显示区域宽度内，最多能显示多少该字体的中文字
                intMaxNumberForWord = Convert.ToInt32(fMaxWidthForName / sizeF.Width);
                if (strName.Length > intMaxNumberForWord)
                {
                    strName = $@"{strName.Substring(0, intMaxNumberForWord)}...";
                }
            }
            gBase.DrawString(strName, fontName, brush, new RectangleF(70f, 700f, fMaxWidthForName, 50f));

            // 绘制商品简介
            // 商品简介区域允许的最大宽度
            float fMaxWidthForPropaganda = 380f;
            float fRowTotal = 2f; // 定义可以显示多少行
            float fHeightTotal = 50f; // 显示区域总的高度
            string strPropaganda = goodsData.propaganda;

            if (!string.IsNullOrWhiteSpace(strPropaganda))
            {
                //StringBuilder sbPropaganda = new StringBuilder();
                //// 允许的最大宽度（把多行看作是1行来计算总宽度，以此作为控制显示字数的上限）
                //float fMaxWidth = fMaxWidthForPropaganda * fRowTotal;
                //float fCurrentWidth = 0f;
                //char[] arr = strPropaganda.ToCharArray();
                //for (int i = 0; i < arr.Length; i++)
                //{
                //    fCurrentWidth += gBase.MeasureString(arr[i].ToString(), fontPropaganda).Width;
                //    if (fCurrentWidth >= fMaxWidth)
                //    {
                //        // 如果超出上限，则不再追加，跳出循环
                //        sbPropaganda.Append("..");
                //        break;
                //    }
                //    else
                //    {
                //        sbPropaganda.Append(arr[i]);
                //    }
                //}
                if (strPropaganda.Length > 30)
                {
                    strPropaganda = $@"{strPropaganda.Substring(0, 30)}...";
                }

                // 计算区域总高度
                fHeightTotal = sizeF.Height * fRowTotal;

            }
            gBase.DrawString(strPropaganda, fontPropaganda, brush, new RectangleF(70f, 750f, fMaxWidthForPropaganda, fHeightTotal));
        }

        /// <summary>
        /// 绘制分享图片中的商品价格
        /// </summary>
        /// <param name="gBase">底图的图面</param>
        /// <param name="imgPriceBg">底图</param>
        /// <param name="imgPriceBg">价格区域背景图</param>
        /// <param name="goodsData">商品数据</param>
        private void DrawPriceForShare(Graphics gBase, Image imgBase, Image imgPriceBg, dynamic goodsData)
        {
            // 定义商品单价、商品现金+金豆混合交易价的字体
            Font fontPrice = new Font("微软雅黑", 24, FontStyle.Bold), fontRMBSymbol = new Font("微软雅黑", 20, FontStyle.Bold),
                 fontCashAndBeansPrice = new Font("微软雅黑", 14);
            // 定义画笔
            Brush brush = new SolidBrush(Color.White);

            using (Graphics gPriceBg = Graphics.FromImage(imgPriceBg))
            {
                gPriceBg.CompositingQuality = CompositingQuality.HighQuality;
                gPriceBg.SmoothingMode = SmoothingMode.HighQuality;
                gPriceBg.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // 绘制商品单价
                string strRMBSymbol = "￥";
                SizeF sizeF_PriceSymbol = gBase.MeasureString(strRMBSymbol, fontRMBSymbol); // 获取币制符号应用指定字体后的尺寸
                SizeF sizeF_Price = gBase.MeasureString(goodsData.price, fontPrice); // 获取商品单价应用指定字体后的尺寸
                using (Image imgPrice = new Bitmap(Convert.ToInt32(sizeF_PriceSymbol.Width + sizeF_Price.Width), Convert.ToInt32(sizeF_Price.Height)))
                {
                    using (Graphics gPrice = Graphics.FromImage(imgPrice))
                    {
                        gPrice.CompositingQuality = CompositingQuality.HighQuality;
                        gPrice.SmoothingMode = SmoothingMode.HighQuality;
                        gPrice.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        // 绘制币制符号
                        gPrice.DrawString(strRMBSymbol, fontRMBSymbol, brush, 0f, 6f);
                        // 绘制商品单价(紧接着币制符号后绘制)
                        gPrice.DrawString(goodsData.price, fontPrice, brush, sizeF_PriceSymbol.Width, 0f);

                        // 将商品单价图绘制到价格区域背景图上
                        gPriceBg.DrawImage(imgPrice, (imgPriceBg.Width - imgPrice.Width) / 2f, 12f);
                    }
                }

                // 绘制商品现金+金豆混合交易价
                string strCashAndBeansPriceText = $@"(￥{goodsData.cash_price}+{goodsData.beans_price}金豆)";
                SizeF sizeF_CashAndBeansPrice = gBase.MeasureString(strCashAndBeansPriceText, fontCashAndBeansPrice);
                gPriceBg.DrawString(strCashAndBeansPriceText, fontCashAndBeansPrice, brush, (imgPriceBg.Width - sizeF_CashAndBeansPrice.Width) / 2f, sizeF_Price.Height + 16f);

                // 将价格区域绘制到底图上
                gBase.DrawImage(imgPriceBg, imgBase.Width - imgPriceBg.Width - 70f, 700f);
            }
        }
    }
}