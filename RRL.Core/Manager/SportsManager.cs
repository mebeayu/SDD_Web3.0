/*
 * STORY #18 lcl 20180515 Insert
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Dynamic;
using System.IO;
using System.Linq;

using RRL.Config;
using RRL.DB;

namespace RRL.Core.Manager
{
    public partial class SportsManager
    {
        /// <summary>
        /// 运动分享二维码中的url
        /// </summary>
        public readonly static string SportShareUrl = $@"{RRLConfig.LiveSiteUrl}/ProductForGame";

        /// <summary>
        /// 获取用户的竞猜数据
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="guessDate">参与竞猜的日期</param>
        /// <returns></returns>
        public List<dynamic> GetUserGuessData(int uid, DateTime guessDate)
        {
            string strSql = @"select * from rrl_run_guess(nolock) where ([uid] = @uid) and (guess_date = @guess_date)";

            return new SqlDataBase().Select<dynamic>(strSql, new { uid = uid, guess_date = guessDate.Date });
        }

        /// <summary>
        /// 获取用户的运动数据
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="runDate">运动步数记录的日期</param>
        /// <returns></returns>
        public List<dynamic> GetUserSportData(int uid, DateTime runDate)
        {
            string strSql = @"select *
                                from (
                                    select [uid] ,step ,isnull(award_money, 0) as award_money
	                                      ,row_number() over(order by step desc) as ranking
                                      from rrl_run_step(nolock)
                                     where (run_date = @run_date)
                                ) as t
                               where ([uid] = @uid)";

            return new SqlDataBase().Select<dynamic>(strSql, new { uid = uid, run_date = runDate.Date });
        }

        /// <summary>
        /// 获取用户运动日记数据
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="runDate">运动步数记录的日期</param>
        /// <returns></returns>
        public dynamic GetUserSportDiary(int uid, DateTime runDate)
        {
            var runData = GetUserSportData(uid, runDate);
            if (runData != null && runData.Count > 0)
            {
                var userRunData = runData.First();
                // 定义可跨DLL的动态对象
                dynamic result = new ExpandoObject();
                // 运动时间
                result.date = runDate.ToString("yyyy-MM-dd");
                // 运动步数
                result.step = userRunData.step;
                // 奖励红包数额
                result.money = userRunData.award_money;
                // 用户排名百分比
                decimal dRankingPercent = CalculateRankingPercent(int.Parse(userRunData.ranking.ToString()));
                result.rankingPercent = $@"{ Math.Floor(dRankingPercent * 100m)}%";
                // 名次
                result.ranking = userRunData.ranking;

                return result;
            }

            return null;
        }

        public decimal CalculateRankingPercent(int ranking)
        {
            if (ranking > 100 || ranking < 1)
            {
                // 100名以外的用户，或者排名值无效的（0和负数的异常情况的处理），显示为88%
                return 0.88m;
            }

            if (ranking == 1)
            {
                // 第一名，显示为100%
                return 1m;
            }

            // 第2~100名，显示为98%
            return 0.98m;
        }

        /// <summary>
        /// 获取用户被点赞的数据
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="likeDate">被点赞的日期</param>
        /// <returns></returns>
        public List<dynamic> GetUserLikeData(int uid, DateTime likeDate)
        {
            string strSql = @"select * from rrl_run_like(nolock) where (to_uid = @uid) and (like_date = @like_date)";

            return new SqlDataBase().Select<dynamic>(strSql, new { uid = uid, like_date = likeDate.Date });
        }

        public Image GenerateSportShareImage(dynamic stepData)
        {
            string strBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // 基础图片完整名称
            string strBaseFullname = Path.Combine(strBaseDirectory, @"Assets\Sport\imgs\share_img_bg.png");
            // 文本区域图片完整名称
            string strTextFullname = Path.Combine(strBaseDirectory, @"Assets\Sport\imgs\share_img_text_area.png");
            // 定义图片对象
            Image imgBase = null, imgTextArea = null;
            // 定义绘图图面
            Graphics gBase = null;

            try
            {
                imgBase = new Bitmap(strBaseFullname);
                imgTextArea = new Bitmap(strTextFullname);
                // 绘制文本区域内容
                DrawTextArea(imgTextArea, stepData);

                gBase = Graphics.FromImage(imgBase);
                // 定义文本区域在基础图片上的位置和大小
                Rectangle recTextArea = new Rectangle()
                {
                    X = (imgBase.Width - imgTextArea.Width) / 2,
                    Y = 540,
                    Width = imgTextArea.Width,
                    Height = imgTextArea.Height
                };
                // 在基础图片上绘制文本区域
                gBase.DrawImage(imgTextArea, recTextArea);

            }
            finally
            {
                if (imgTextArea != null)
                {
                    imgTextArea.Dispose();
                }
                if (gBase != null)
                {
                    gBase.Dispose();
                }
            }

            return imgBase;
        }

        /// <summary>
        /// 绘制文本区域内容 
        /// </summary>
        /// <param name="imgTextArea">文本区域的底图</param>
        /// <param name="stepData">运动数据</param>
        private void DrawTextArea(Image imgTextArea, dynamic stepData)
        {
            PublicManager publicMgr = new PublicManager();
            // 定义绘图图面
            Graphics gTextArea = null;
            // 定义运动日记的标题、内容说明文字、运动数据的字体
            Font dateFont = new Font("Arial", 24, FontStyle.Bold), sportDataFont = new Font("Arial", 36);
            // 定义画笔
            Brush brush = new SolidBrush(Color.White);
            // 定义字体尺寸变量
            SizeF sizeF = new SizeF();
            // 定义纵向偏移量。由于字体样式的不同，造成纵向定位不同
            float fVerticalOffset = 0f;

            try
            {
                gTextArea = Graphics.FromImage(imgTextArea);
                // 以下Graphics的三个属性，用来设置图片为高质量
                gTextArea.CompositingQuality = CompositingQuality.HighQuality;
                gTextArea.SmoothingMode = SmoothingMode.HighQuality;
                gTextArea.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // 绘制日期
                sizeF = gTextArea.MeasureString(stepData.date, dateFont);
                gTextArea.DrawString(stepData.date, dateFont, brush, (imgTextArea.Width - sizeF.Width) / 2f, 100f);
                // 绘制步数
                sizeF = gTextArea.MeasureString(stepData.step, sportDataFont);
                gTextArea.DrawString(stepData.step, sportDataFont, brush, (imgTextArea.Width - sizeF.Width) / 2f, 176f + fVerticalOffset);
                // 绘制红包数
                sizeF = gTextArea.MeasureString(stepData.redPackage, sportDataFont);
                gTextArea.DrawString(stepData.redPackage, sportDataFont, brush, (imgTextArea.Width - sizeF.Width) / 2f, 236f + fVerticalOffset);
                // 绘制击败全国用户百分比
                sizeF = gTextArea.MeasureString(stepData.rankingPercent, sportDataFont);
                gTextArea.DrawString(stepData.rankingPercent, sportDataFont, brush, (imgTextArea.Width - sizeF.Width) / 2f + 30f, 298f + fVerticalOffset);
                // 生成二维码原始图
                using (var qrImg = publicMgr.GenerateQRCodeImage(stepData.qrCodeContent))
                {
                    // 等比例缩小二维码图像
                    using (var cutImg = publicMgr.CutToFixedSizeByEqualProportion(qrImg, 140, 140))
                    {
                        // 绘制二维码
                        gTextArea.DrawImage(cutImg, (imgTextArea.Width - cutImg.Width) / 2f, 396f);
                    }
                }

                gTextArea.Flush();
            }
            finally
            {
                if (gTextArea != null)
                {
                    gTextArea.Dispose();
                }
            }
        }
    }
}
