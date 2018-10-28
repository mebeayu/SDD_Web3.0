using System;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Web.Mvc;

using RRL.Core.Manager;
using RRL.Core.Models;
using RRL.Core.Utility;

namespace RRL.WEB.Controllers
{
    // STORY #6 lcl 20180419 Insert
    public class SportController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult Yrecord()
        {
            return View();
        }

        public ActionResult Share()
        {
            return View();
        }

        // STORY #18 lcl 20180518 Insert
        /// <summary>
        /// 运动分享的二维码
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ActionResult SportShareQRCode(string token)
        {
            MemoryStream ms = new MemoryStream();
            // 初始化二维码内容
            string strQRCodeContent = SportsManager.SportShareUrl;

            if (!string.IsNullOrWhiteSpace(token))
            {
                TokenObject tokenObject = TokenObject.InitTokenObjFromString(token);
                UserAuth user = new UserAuth();
                int intResult = user.Load(tokenObject.id);
                if (intResult == MessageCode.SUCCESS)
                {
                    // 是有效的用户，则追加token
                    strQRCodeContent = $@"{strQRCodeContent}?token={token}";
                }
            }

            PublicManager publicMgr = new PublicManager();
            // 生成二维码原始图
            using (var qrImg = publicMgr.GenerateQRCodeImage(strQRCodeContent))
            {
                // 等比例缩小二维码图像
                using (var cutImg = publicMgr.CutToFixedSizeByEqualProportion(qrImg, 140, 140))
                {
                    cutImg.Save(ms, ImageFormat.Jpeg);
                }
            }

            return File(ms.ToArray(), "image/jpeg");
        }

        // STORY #18 lcl 20180518 Insert
        /// <summary>
        /// 用户运动数据分享图片
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ActionResult SportShareImage(string token)
        {
            MemoryStream ms = new MemoryStream();
            SportsManager SportsMgr = new SportsManager();

            string strTextForNoneData = "- -";
            DateTime dtSportDate = DateTime.Now.AddDays(-1).Date; // 默认为当前日期的前一天

            dynamic userStepData = new ExpandoObject();
            userStepData.step = strTextForNoneData; // 总步数
            userStepData.redPackage = strTextForNoneData; // 运动红包数
            userStepData.rankingPercent = strTextForNoneData; // 击败全国用户的百分比
            userStepData.date = dtSportDate.ToString("yyyy-MM-dd"); // 需要获取哪一天的运动数据
            userStepData.qrCodeContent = SportsManager.SportShareUrl; // 初始化二维码内容

            if (!string.IsNullOrWhiteSpace(token))
            {
                TokenObject tokenObject = TokenObject.InitTokenObjFromString(token);
                UserAuth user = new UserAuth();
                int intResult = user.Load(tokenObject.id);
                if (intResult == MessageCode.SUCCESS)
                {
                    // 用户有效
                    // 二维码内容中追加token信息
                    userStepData.qrCodeContent = $@"{userStepData.qrCodeContent}?token={token}";

                    // 获取用户运动数据
                    var stepData = SportsMgr.GetUserSportDiary(user.id, dtSportDate);
                    if (stepData != null)
                    {
                        userStepData.step = stepData.step.ToString();
                        userStepData.redPackage = Math.Floor(stepData.money).ToString();
                        userStepData.rankingPercent = stepData.rankingPercent;
                    }
                }
            }

            using (var shareImage = SportsMgr.GenerateSportShareImage(userStepData))
            {
                shareImage.Save(ms, ImageFormat.Jpeg);
            }

            return File(ms.ToArray(), "image/jpeg");
        }
    }
}