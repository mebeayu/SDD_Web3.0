using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormReleaseVer
{
    public partial class Form1 : Form
    {
        private static string INDEX_R_PATH = @"Views\ViewRoute\Index.cshtml";
        private static string VUEROUTER_R_PATH = @"Assets\Dist\Scripts\vue-router.js";
        private static string APPJS_R_PATH = @"Assets\Dist\Scripts\app.js";
        private static string APPCSS_R_PATH = @"Assets\Dist\Styles\app.css";

        private string APP_PATH = "";
        private string version = "";
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            initUI();
        }

        private void initUI()
        {
            this.version=this.txt_version.Text = DateTime.Now.ToString("yyyyMMddHHmm");
            this.APP_PATH = this.txt_path.Text.ToString().Trim();
        }

        private void btn_r_Click(object sender, EventArgs e)
        {
            initUI();
            string vue_path_source = Path.Combine(APP_PATH, VUEROUTER_R_PATH);
            string vue_path_dist = Path.Combine(APP_PATH, VUEROUTER_R_PATH.Replace("vue-router.js", string.Format("vue-router-{0}.js", version)));
            File.Copy(vue_path_source, vue_path_dist,true);

            string appjs_path_source = Path.Combine(APP_PATH, APPJS_R_PATH);
            string appjs_path_dist = Path.Combine(APP_PATH, APPJS_R_PATH.Replace("app.js", string.Format("app-{0}.js", version)));
            File.Copy(appjs_path_source, appjs_path_dist, true);


            string appcss_path_source = Path.Combine(APP_PATH, APPCSS_R_PATH);
            string appcss_path_dist = Path.Combine(APP_PATH, APPCSS_R_PATH.Replace("app.css", string.Format("app-{0}.css", version)));
            File.Copy(appcss_path_source, appcss_path_dist, true);

            string index_path = Path.Combine(APP_PATH, INDEX_R_PATH);
            var allIndexContent=File.ReadAllText(index_path, Encoding.UTF8);
            //allIndexContent=allIndexContent.Replace("vue-router.js", string.Format("vue-router-{0}.js", version));
            //allIndexContent = allIndexContent.Replace("app.js", string.Format("app-{0}.js", version));
            //allIndexContent = allIndexContent.Replace("app.css", string.Format("app-{0}.css", version));
            allIndexContent=Regex.Replace(allIndexContent, @"vue-router(-)?\d*.js", string.Format("vue-router-{0}.js", version));
            allIndexContent = Regex.Replace(allIndexContent, @"app(-)?\d*.js", string.Format("app-{0}.js", version));
            allIndexContent = Regex.Replace(allIndexContent, @"app(-)?\d*.css", string.Format("app-{0}.css", version));
            File.WriteAllText(index_path, allIndexContent);


            MessageBox.Show("发布完成!");

        }
    }
}
