namespace WinFormReleaseVer
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_r = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_version = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_path = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_r
            // 
            this.btn_r.Location = new System.Drawing.Point(227, 179);
            this.btn_r.Name = "btn_r";
            this.btn_r.Size = new System.Drawing.Size(118, 58);
            this.btn_r.TabIndex = 0;
            this.btn_r.Text = "发布";
            this.btn_r.UseVisualStyleBackColor = true;
            this.btn_r.Click += new System.EventHandler(this.btn_r_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(166, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "版本号";
            // 
            // txt_version
            // 
            this.txt_version.Location = new System.Drawing.Point(226, 125);
            this.txt_version.Name = "txt_version";
            this.txt_version.Size = new System.Drawing.Size(154, 25);
            this.txt_version.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(166, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "路径:";
            // 
            // txt_path
            // 
            this.txt_path.Location = new System.Drawing.Point(227, 33);
            this.txt_path.Name = "txt_path";
            this.txt_path.Size = new System.Drawing.Size(154, 25);
            this.txt_path.TabIndex = 2;
            this.txt_path.Text = "c:\\SDOUDOU";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 360);
            this.Controls.Add(this.txt_path);
            this.Controls.Add(this.txt_version);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_r);
            this.Name = "Form1";
            this.Text = "SDD3.0版本发布替换服务";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_r;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_version;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_path;
    }
}

