namespace IQBConsole
{
    partial class SocketClient
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
            this.bn_Connect = new System.Windows.Forms.Button();
            this.bn_Send = new System.Windows.Forms.Button();
            this.tb_Word = new System.Windows.Forms.TextBox();
            this.bn_WSConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bn_Connect
            // 
            this.bn_Connect.Location = new System.Drawing.Point(1129, 50);
            this.bn_Connect.Name = "bn_Connect";
            this.bn_Connect.Size = new System.Drawing.Size(75, 23);
            this.bn_Connect.TabIndex = 0;
            this.bn_Connect.Text = "Connect";
            this.bn_Connect.UseVisualStyleBackColor = true;
            this.bn_Connect.Click += new System.EventHandler(this.bn_Connect_Click);
            // 
            // bn_Send
            // 
            this.bn_Send.Location = new System.Drawing.Point(1129, 611);
            this.bn_Send.Name = "bn_Send";
            this.bn_Send.Size = new System.Drawing.Size(75, 23);
            this.bn_Send.TabIndex = 1;
            this.bn_Send.Text = "Send";
            this.bn_Send.UseVisualStyleBackColor = true;
            this.bn_Send.Click += new System.EventHandler(this.bn_Send_Click);
            // 
            // tb_Word
            // 
            this.tb_Word.Location = new System.Drawing.Point(13, 200);
            this.tb_Word.Multiline = true;
            this.tb_Word.Name = "tb_Word";
            this.tb_Word.Size = new System.Drawing.Size(1076, 434);
            this.tb_Word.TabIndex = 2;
            // 
            // bn_WSConnect
            // 
            this.bn_WSConnect.Location = new System.Drawing.Point(1129, 109);
            this.bn_WSConnect.Name = "bn_WSConnect";
            this.bn_WSConnect.Size = new System.Drawing.Size(75, 23);
            this.bn_WSConnect.TabIndex = 3;
            this.bn_WSConnect.Text = "WS Connect";
            this.bn_WSConnect.UseVisualStyleBackColor = true;
            this.bn_WSConnect.Click += new System.EventHandler(this.bn_WSConnect_Click);
            // 
            // SocketClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1251, 736);
            this.Controls.Add(this.bn_WSConnect);
            this.Controls.Add(this.tb_Word);
            this.Controls.Add(this.bn_Send);
            this.Controls.Add(this.bn_Connect);
            this.Name = "SocketClient";
            this.Text = "SocketClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bn_Connect;
        private System.Windows.Forms.Button bn_Send;
        private System.Windows.Forms.TextBox tb_Word;
        private System.Windows.Forms.Button bn_WSConnect;
    }
}