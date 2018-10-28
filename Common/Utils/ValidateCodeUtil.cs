using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Common.Utils
{
    /// <summary>
    /// Powered by Rocby.com
    /// </summary>
    public class ValidateCodeUtilGroup
    {
        public string Name { get; set; }
        public Color TheColor { get; set; }
    }

    /// <summary>
    /// Powered by Rocby.com
    /// </summary>
    public class ValidateCodeUtil
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Color BackgroundColor { get; set; }
        public int Length { get; set; }
        private string SessionName = "ValidateCode";
        public float FontSize { get; set; }
        public int CutWidth { get; set; }

        List<ValidateCodeUtilGroup> DicGroup = new List<ValidateCodeUtilGroup>();

        public ValidateCodeUtil()
        {
            DicGroup.Add(new ValidateCodeUtilGroup() { Name = "蓝色", TheColor = Color.Blue });
            DicGroup.Add(new ValidateCodeUtilGroup() { Name = "红色", TheColor = Color.Red });
            DicGroup.Add(new ValidateCodeUtilGroup() { Name = "绿色", TheColor = Color.Green });
        }

        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="VcodeNum"></param>
        /// <returns></returns>
        string RndNum(int VcodeNum)
        {
            string Vchar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] VcArray = Vchar.Split(new Char[] { ',' });
            string VNum = string.Empty;
            Random rand = new Random();
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                int t = rand.Next(Vchar.Split(',').Length);
                VNum += VcArray[t];
            }
            return VNum;
        }

        /// <summary>
        /// 随机颜色
        /// </summary>
        /// <returns></returns>
        /*Color GetRandomColor(int seed)
        {
            int sIndex = new Random(seed).Next(DicGroup.Count);
            return DicGroup[sIndex].TheColor;
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            //  对于C#的随机数，没什么好说的
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            //  为了在白色背景上显示，尽量生成深色
            int int_Red = RandomNum_First.Next(256);
            int int_Green = RandomNum_Sencond.Next(256);
            int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return Color.FromArgb(int_Red, int_Green, int_Blue);
        }*/

        /// <summary>
        /// 创建图片
        /// </summary>
        /// <param name="len"></param>
        /// <param name="name"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public Byte[] CreateImage(ImageFormat t)
        {
            ValidateCodeUtilGroup selGroup = DicGroup[new Random().Next(DicGroup.Count)];   //获取一个需要用户进行输入的颜色组
            string str_ValidateCode = RndNum(this.Length);  //获取相对数量的随机字符

            Bitmap theBitmap = new Bitmap(this.Width, this.Height); //图高20px
            Graphics g = Graphics.FromImage(theBitmap);
            g.Clear(this.BackgroundColor);    //背景颜色设置
            //theGraphics.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, int_ImageWidth - 1, 19); //灰色边框
            Font theFont = new Font(FontFamily.GenericSansSerif , this.FontSize);    //10pt的字体
            string sessionStr = string.Empty;
            Random newRandom = new Random();
            for (int int_index = 0; int_index < str_ValidateCode.Length; int_index++)
            {

                //g.TranslateTransform(this.Width / 2, this.Height / 2);
                //g.RotateTransform(int_index % 2 == 0 ? 5 : -5); //旋转


                string str_char = str_ValidateCode.Substring(int_index, 1);

                var forRandom = new Random((int)DateTime.Now.Ticks + int_index);
                var randomGroup = DicGroup[forRandom.Next(DicGroup.Count)];
                if (randomGroup.TheColor == selGroup.TheColor) { sessionStr += str_char; }   //组织Session字符
                Brush newBrush = new SolidBrush(randomGroup.TheColor);
                Point thePos = new Point(int_index * this.CutWidth + 1 + newRandom.Next(3), 1 + newRandom.Next(3));
                g.DrawString(str_char, theFont, newBrush, thePos);
            }
            if (string.IsNullOrWhiteSpace(sessionStr))
            {
                return CreateImage(t);
            }
            //存到Session
            HttpContext.Current.Session[this.SessionName] = sessionStr;
            //
            g.DrawString("请输入" + selGroup.Name + "的字符", new Font(FontFamily.GenericSansSerif, 9), new SolidBrush(selGroup.TheColor), new Point(5, this.Height - 18));
            //  将生成的图片发回客户端
            MemoryStream ms = new MemoryStream();
            theBitmap.Save(ms, t);
            byte[] arr = ms.ToArray();
            g.Dispose();
            theBitmap.Dispose();
            return arr;
        }

        /// <summary>
        /// 判断验证码正确性
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool CheckCode(string code)
        {
            string ses = Convert.ToString(HttpContext.Current.Session[SessionName]);
            if (string.IsNullOrWhiteSpace(ses) || string.IsNullOrWhiteSpace(code)) { return false; }
            return ses.ToLower() == code.ToLower();
        }
    }
}