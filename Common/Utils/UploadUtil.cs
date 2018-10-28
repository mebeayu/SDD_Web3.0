using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;

namespace Common.Utils
{
    /// <summary>
    /// 上传反馈
    /// Powered by Rocby.com
    /// </summary>
    public class UploadUtilResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 保存后的虚拟路径
        /// </summary>
        public string ServerFilePath { get; set; }
    }

    /// <summary>
    /// 上传工具
    /// </summary>
    public class UploadUtil
    {
        /// <summary>
        /// 允许上传的文件后缀（注，需是大写形式）（如“.zip”或“.txt”）
        /// </summary>
        public string[] Exts { get; set; }
        /// <summary>
        /// 最小长度
        /// </summary>
        public long MinLen { get; set; }
        /// <summary>
        /// 最大长度
        /// </summary>
        public long MaxLen { get; set; }
        /// <summary>
        /// 保存到服务器的文件夹的虚拟路径
        /// </summary>
        public string SaveServerPath { get; set; }

        public bool CheckExt(string[] exts, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) { return false; }
            string ext = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(ext)) { return false; }
            foreach (var e in exts)
            {
                if (fileName.EndsWith(e, System.StringComparison.OrdinalIgnoreCase)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// 检验上传文件到服务器
        /// </summary>
        /// <param name="file"></param>
        /// <param name="randomFileName">如果为true，则只需要传入目录路径</param>
        /// <returns></returns>
        public UploadUtilResult CheckUploadHttpPostedFile(HttpPostedFileBase file, bool randomFileName)
        {
            UploadUtilResult result = new UploadUtilResult();
            if (file == null) { result.Message = "文件没有设置"; }
            else
            {
                //后缀判断
                if (!CheckExt(this.Exts, file.FileName)) { result.Message = "文件后缀不符合规范（" + string.Join(", ", this.Exts) + "）"; }
                else
                {
                    //大小判断
                    if (file.ContentLength < this.MinLen || file.ContentLength > this.MaxLen) { result.Message = "文件长度不符合规范（" + FormatUtil.FormatLength(this.MinLen) + " - " + FormatUtil.FormatLength(this.MaxLen) + "）"; }
                    else
                    {

                            result.Message = "文件可以成功";
                            result.IsSuccess = true;
                            result.ServerFilePath = "";
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 上传文件到服务器
        /// </summary>
        /// <param name="file"></param>
        /// <param name="randomFileName">如果为true，则只需要传入目录路径</param>
        /// <returns></returns>
        public UploadUtilResult UploadHttpPostedFile(HttpPostedFileBase file, bool randomFileName)
        {
            UploadUtilResult result = new UploadUtilResult();
            if (file == null) { result.Message = "文件没有设置"; }
            else
            {
                //后缀判断
                if (!CheckExt(this.Exts, file.FileName)) { result.Message = "文件后缀不符合规范（" + string.Join(", ", this.Exts) + "）"; }
                else
                {
                    //大小判断
                    if (file.ContentLength < this.MinLen || file.ContentLength > this.MaxLen) { result.Message = "文件长度不符合规范（" + FormatUtil.FormatLength(this.MinLen) + " - " + FormatUtil.FormatLength(this.MaxLen) + "）"; }
                    else
                    {
                        //目录存在性判断
                        string folderDirPath = Path.GetDirectoryName(this.SaveServerPath);
                        if (!Directory.Exists(HttpContext.Current.Server.MapPath(folderDirPath))) { result.Message = "储存目录不存在（" + folderDirPath + "）"; }
                        else
                        {
                            string fileSavePath = this.SaveServerPath;
                            if (randomFileName)
                            {
                                if (!fileSavePath.EndsWith("/") && fileSavePath.EndsWith("\\")) { fileSavePath += "/"; }
                                fileSavePath += Guid.NewGuid() + Path.GetExtension(file.FileName);
                            }
                            /*
                            // cs
                            Image image = Image.FromFile(fileSavePath);//@"c:\1.jpg"
                            int smallWidth = 150;
                            int smallHeight = 100;
                            if (image.Width > image.Height)
                            {
                                smallHeight = smallWidth * image.Height / image.Width;
                            }
                            else
                            {
                                smallWidth = smallHeight * image.Width / image.Height;
                            }

                            //缩略图
                            Bitmap bitmap = new Bitmap(smallWidth, smallHeight);
                            Graphics g = Graphics.FromImage(bitmap);
                            g.DrawImage(image, 0, 0, smallWidth, smallHeight);
                            //保存图片 第二个参数根据自己的图片格式选择
                            bitmap.Save(HttpContext.Current.Server.MapPath(fileSavePath), System.Drawing.Imaging.ImageFormat.Jpeg);
                            g.Dispose();
                            bitmap.Dispose();
                            image.Dispose();
                            //cs
                            */

                            //保存
                            file.SaveAs(HttpContext.Current.Server.MapPath(fileSavePath));
                            result.Message = "文件上传成功";
                            result.IsSuccess = true;
                            result.ServerFilePath = fileSavePath;
                        }
                    }
                }
            }
            return result;
        }
    }
}