namespace Wnmp
{
    partial class Frm_Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Main));
            this.UiStart = new System.Windows.Forms.Button();
            this.UiLogBox = new System.Windows.Forms.RichTextBox();
            this.UiReload = new System.Windows.Forms.Button();
            this.UiQuit = new System.Windows.Forms.Button();
            this.BtnProcesses = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // UiStart
            // 
            this.UiStart.Location = new System.Drawing.Point(231, 391);
            this.UiStart.Name = "UiStart";
            this.UiStart.Size = new System.Drawing.Size(107, 47);
            this.UiStart.TabIndex = 0;
            this.UiStart.Text = "启  动";
            this.UiStart.UseVisualStyleBackColor = true;
            this.UiStart.Click += new System.EventHandler(this.UiStart_Click);
            // 
            // UiLogBox
            // 
            this.UiLogBox.Location = new System.Drawing.Point(-1, 3);
            this.UiLogBox.Name = "UiLogBox";
            this.UiLogBox.Size = new System.Drawing.Size(800, 369);
            this.UiLogBox.TabIndex = 1;
            this.UiLogBox.Text = "";
            // 
            // UiReload
            // 
            this.UiReload.Location = new System.Drawing.Point(344, 391);
            this.UiReload.Name = "UiReload";
            this.UiReload.Size = new System.Drawing.Size(107, 47);
            this.UiReload.TabIndex = 2;
            this.UiReload.Text = "重 启";
            this.UiReload.UseVisualStyleBackColor = true;
            this.UiReload.Click += new System.EventHandler(this.UiReload_Click);
            // 
            // UiQuit
            // 
            this.UiQuit.Location = new System.Drawing.Point(457, 391);
            this.UiQuit.Name = "UiQuit";
            this.UiQuit.Size = new System.Drawing.Size(107, 47);
            this.UiQuit.TabIndex = 3;
            this.UiQuit.Text = "退 出";
            this.UiQuit.UseVisualStyleBackColor = true;
            this.UiQuit.Click += new System.EventHandler(this.UiQuit_Click);
            // 
            // BtnProcesses
            // 
            this.BtnProcesses.Location = new System.Drawing.Point(684, 391);
            this.BtnProcesses.Name = "BtnProcesses";
            this.BtnProcesses.Size = new System.Drawing.Size(107, 47);
            this.BtnProcesses.TabIndex = 5;
            this.BtnProcesses.Text = "显示服务进程";
            this.BtnProcesses.UseVisualStyleBackColor = true;
            this.BtnProcesses.Click += new System.EventHandler(this.BtnProcesses_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "准确的来说现在还是Wnp，MySql还未集成";
            this.notifyIcon1.BalloonTipTitle = "Wnmp";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Wnmp";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            // 
            // Frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnProcesses);
            this.Controls.Add(this.UiQuit);
            this.Controls.Add(this.UiReload);
            this.Controls.Add(this.UiLogBox);
            this.Controls.Add(this.UiStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Frm_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wnmp";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Frm_Main_FormClosed);
            this.SizeChanged += new System.EventHandler(this.Frm_Main_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button UiStart;
        private System.Windows.Forms.RichTextBox UiLogBox;
        private System.Windows.Forms.Button UiReload;
        private System.Windows.Forms.Button UiQuit;
        private System.Windows.Forms.Button BtnProcesses;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}

