namespace IQBConsole
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
            this.button1 = new System.Windows.Forms.Button();
            this.tb_Result = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.bn_ARData = new System.Windows.Forms.Button();
            this.bn_catchHJB = new System.Windows.Forms.Button();
            this.bn_webForm = new System.Windows.Forms.Button();
            this.bn_MemberList = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(34, 38);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(171, 34);
            this.button1.TabIndex = 0;
            this.button1.Text = "爬虫(IQB/ABC)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tb_Result
            // 
            this.tb_Result.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tb_Result.Location = new System.Drawing.Point(0, 397);
            this.tb_Result.Margin = new System.Windows.Forms.Padding(4);
            this.tb_Result.Name = "tb_Result";
            this.tb_Result.Size = new System.Drawing.Size(1366, 384);
            this.tb_Result.TabIndex = 1;
            this.tb_Result.Text = "";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(34, 110);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(218, 34);
            this.button2.TabIndex = 2;
            this.button2.Text = "Create APtrans Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // bn_ARData
            // 
            this.bn_ARData.Location = new System.Drawing.Point(34, 153);
            this.bn_ARData.Margin = new System.Windows.Forms.Padding(4);
            this.bn_ARData.Name = "bn_ARData";
            this.bn_ARData.Size = new System.Drawing.Size(218, 34);
            this.bn_ARData.TabIndex = 2;
            this.bn_ARData.Text = "Create ARtrans Data";
            this.bn_ARData.UseVisualStyleBackColor = true;
            this.bn_ARData.Click += new System.EventHandler(this.bn_ARData_Click);
            // 
            // bn_catchHJB
            // 
            this.bn_catchHJB.Location = new System.Drawing.Point(231, 38);
            this.bn_catchHJB.Margin = new System.Windows.Forms.Padding(4);
            this.bn_catchHJB.Name = "bn_catchHJB";
            this.bn_catchHJB.Size = new System.Drawing.Size(171, 34);
            this.bn_catchHJB.TabIndex = 0;
            this.bn_catchHJB.Text = "爬虫(汇聚宝)";
            this.bn_catchHJB.UseVisualStyleBackColor = true;
            this.bn_catchHJB.Click += new System.EventHandler(this.bn_catchHJB_Click);
            // 
            // bn_webForm
            // 
            this.bn_webForm.Location = new System.Drawing.Point(1149, 38);
            this.bn_webForm.Name = "bn_webForm";
            this.bn_webForm.Size = new System.Drawing.Size(90, 43);
            this.bn_webForm.TabIndex = 3;
            this.bn_webForm.Text = "webForm";
            this.bn_webForm.UseVisualStyleBackColor = true;
            this.bn_webForm.Click += new System.EventHandler(this.bn_webForm_Click);
            // 
            // bn_MemberList
            // 
            this.bn_MemberList.Location = new System.Drawing.Point(34, 217);
            this.bn_MemberList.Name = "bn_MemberList";
            this.bn_MemberList.Size = new System.Drawing.Size(218, 37);
            this.bn_MemberList.TabIndex = 4;
            this.bn_MemberList.Text = "微信会员列表";
            this.bn_MemberList.UseVisualStyleBackColor = true;
            this.bn_MemberList.Click += new System.EventHandler(this.bn_MemberList_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 781);
            this.Controls.Add(this.bn_MemberList);
            this.Controls.Add(this.bn_webForm);
            this.Controls.Add(this.bn_ARData);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tb_Result);
            this.Controls.Add(this.bn_catchHJB);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox tb_Result;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button bn_ARData;
        private System.Windows.Forms.Button bn_catchHJB;
        private System.Windows.Forms.Button bn_webForm;
        private System.Windows.Forms.Button bn_MemberList;
    }
}

