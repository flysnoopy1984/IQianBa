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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(23, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "爬虫(IQB/ABC)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tb_Result
            // 
            this.tb_Result.Location = new System.Drawing.Point(724, 515);
            this.tb_Result.Name = "tb_Result";
            this.tb_Result.Size = new System.Drawing.Size(431, 147);
            this.tb_Result.TabIndex = 1;
            this.tb_Result.Text = "";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(23, 73);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(145, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Create APtrans Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // bn_ARData
            // 
            this.bn_ARData.Location = new System.Drawing.Point(23, 102);
            this.bn_ARData.Name = "bn_ARData";
            this.bn_ARData.Size = new System.Drawing.Size(145, 23);
            this.bn_ARData.TabIndex = 2;
            this.bn_ARData.Text = "Create ARtrans Data";
            this.bn_ARData.UseVisualStyleBackColor = true;
            this.bn_ARData.Click += new System.EventHandler(this.bn_ARData_Click);
            // 
            // bn_catchHJB
            // 
            this.bn_catchHJB.Location = new System.Drawing.Point(154, 25);
            this.bn_catchHJB.Name = "bn_catchHJB";
            this.bn_catchHJB.Size = new System.Drawing.Size(114, 23);
            this.bn_catchHJB.TabIndex = 0;
            this.bn_catchHJB.Text = "爬虫(汇聚宝)";
            this.bn_catchHJB.UseVisualStyleBackColor = true;
            this.bn_catchHJB.Click += new System.EventHandler(this.bn_catchHJB_Click);
            // 
            // bn_webForm
            // 
            this.bn_webForm.Location = new System.Drawing.Point(766, 25);
            this.bn_webForm.Margin = new System.Windows.Forms.Padding(2);
            this.bn_webForm.Name = "bn_webForm";
            this.bn_webForm.Size = new System.Drawing.Size(60, 29);
            this.bn_webForm.TabIndex = 3;
            this.bn_webForm.Text = "webForm";
            this.bn_webForm.UseVisualStyleBackColor = true;
            this.bn_webForm.Click += new System.EventHandler(this.bn_webForm_Click);
            // 
            // bn_MemberList
            // 
            this.bn_MemberList.Location = new System.Drawing.Point(23, 145);
            this.bn_MemberList.Margin = new System.Windows.Forms.Padding(2);
            this.bn_MemberList.Name = "bn_MemberList";
            this.bn_MemberList.Size = new System.Drawing.Size(145, 25);
            this.bn_MemberList.TabIndex = 4;
            this.bn_MemberList.Text = "微信会员列表";
            this.bn_MemberList.UseVisualStyleBackColor = true;
            this.bn_MemberList.Click += new System.EventHandler(this.bn_MemberList_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(23, 224);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(360, 607);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(644, 25);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "TestQR";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(522, 25);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 881);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.bn_MemberList);
            this.Controls.Add(this.bn_webForm);
            this.Controls.Add(this.bn_ARData);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tb_Result);
            this.Controls.Add(this.bn_catchHJB);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

