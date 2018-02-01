namespace IQBConsole
{
    partial class WebForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.bn_start = new System.Windows.Forms.Button();
            this.bn_close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(1654, 803);
            this.webBrowser1.TabIndex = 0;
            // 
            // bn_start
            // 
            this.bn_start.Location = new System.Drawing.Point(693, 841);
            this.bn_start.Name = "bn_start";
            this.bn_start.Size = new System.Drawing.Size(81, 40);
            this.bn_start.TabIndex = 1;
            this.bn_start.Text = "Start";
            this.bn_start.UseVisualStyleBackColor = true;
            this.bn_start.Click += new System.EventHandler(this.bn_start_Click);
            // 
            // bn_close
            // 
            this.bn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bn_close.Location = new System.Drawing.Point(851, 841);
            this.bn_close.Name = "bn_close";
            this.bn_close.Size = new System.Drawing.Size(79, 40);
            this.bn_close.TabIndex = 2;
            this.bn_close.Text = "Close";
            this.bn_close.UseVisualStyleBackColor = true;
            this.bn_close.Click += new System.EventHandler(this.bn_close_Click);
            // 
            // WebForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bn_close;
            this.ClientSize = new System.Drawing.Size(1654, 926);
            this.Controls.Add(this.bn_close);
            this.Controls.Add(this.bn_start);
            this.Controls.Add(this.webBrowser1);
            this.Name = "WebForm";
            this.Text = "WebForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button bn_start;
        private System.Windows.Forms.Button bn_close;
    }
}