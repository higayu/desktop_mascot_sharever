
namespace DesktopMascot_Share {
    partial class Form1 {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.終了ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.アクションToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.パトカーToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ストップToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ひよこToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ポップコーンToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.体力の表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.progressBar_Food = new System.Windows.Forms.ProgressBar();
            this.timer_Physical = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.progressBar_Physical = new System.Windows.Forms.ProgressBar();
            this.応援ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Yu Gothic UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.終了ToolStripMenuItem,
            this.アクションToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(223, 132);
            // 
            // 終了ToolStripMenuItem
            // 
            this.終了ToolStripMenuItem.Name = "終了ToolStripMenuItem";
            this.終了ToolStripMenuItem.Size = new System.Drawing.Size(222, 50);
            this.終了ToolStripMenuItem.Text = "終了";
            this.終了ToolStripMenuItem.Click += new System.EventHandler(this.終了ToolStripMenuItem_Click);
            // 
            // アクションToolStripMenuItem
            // 
            this.アクションToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.パトカーToolStripMenuItem,
            this.ストップToolStripMenuItem,
            this.ひよこToolStripMenuItem,
            this.ポップコーンToolStripMenuItem,
            this.応援ToolStripMenuItem,
            this.体力の表示ToolStripMenuItem});
            this.アクションToolStripMenuItem.Name = "アクションToolStripMenuItem";
            this.アクションToolStripMenuItem.Size = new System.Drawing.Size(222, 50);
            this.アクションToolStripMenuItem.Text = "アクション";
            // 
            // パトカーToolStripMenuItem
            // 
            this.パトカーToolStripMenuItem.Name = "パトカーToolStripMenuItem";
            this.パトカーToolStripMenuItem.Size = new System.Drawing.Size(393, 50);
            this.パトカーToolStripMenuItem.Text = "パトカー";
            this.パトカーToolStripMenuItem.Click += new System.EventHandler(this.パトカーToolStripMenuItem_Click);
            // 
            // ストップToolStripMenuItem
            // 
            this.ストップToolStripMenuItem.Name = "ストップToolStripMenuItem";
            this.ストップToolStripMenuItem.Size = new System.Drawing.Size(393, 50);
            this.ストップToolStripMenuItem.Text = "ストップ";
            this.ストップToolStripMenuItem.Click += new System.EventHandler(this.ストップToolStripMenuItem_Click);
            // 
            // ひよこToolStripMenuItem
            // 
            this.ひよこToolStripMenuItem.Name = "ひよこToolStripMenuItem";
            this.ひよこToolStripMenuItem.Size = new System.Drawing.Size(393, 50);
            this.ひよこToolStripMenuItem.Text = "ひよこ";
            this.ひよこToolStripMenuItem.Click += new System.EventHandler(this.ひよこToolStripMenuItem_Click);
            // 
            // ポップコーンToolStripMenuItem
            // 
            this.ポップコーンToolStripMenuItem.Name = "ポップコーンToolStripMenuItem";
            this.ポップコーンToolStripMenuItem.Size = new System.Drawing.Size(393, 50);
            this.ポップコーンToolStripMenuItem.Text = "ポップコーン";
            this.ポップコーンToolStripMenuItem.Click += new System.EventHandler(this.ポップコーンToolStripMenuItem_Click);
            // 
            // 体力の表示ToolStripMenuItem
            // 
            this.体力の表示ToolStripMenuItem.Name = "体力の表示ToolStripMenuItem";
            this.体力の表示ToolStripMenuItem.Size = new System.Drawing.Size(393, 50);
            this.体力の表示ToolStripMenuItem.Text = "体力の表示ON/OFF";
            this.体力の表示ToolStripMenuItem.Click += new System.EventHandler(this.体力の表示ToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 200);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            // 
            // progressBar_Food
            // 
            this.progressBar_Food.BackColor = System.Drawing.Color.White;
            this.progressBar_Food.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar_Food.ForeColor = System.Drawing.Color.LimeGreen;
            this.progressBar_Food.Location = new System.Drawing.Point(0, 0);
            this.progressBar_Food.Name = "progressBar_Food";
            this.progressBar_Food.Size = new System.Drawing.Size(94, 25);
            this.progressBar_Food.TabIndex = 2;
            this.progressBar_Food.Visible = false;
            // 
            // timer_Physical
            // 
            this.timer_Physical.Interval = 1000;
            this.timer_Physical.Tick += new System.EventHandler(this.timer_Physical_Tick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.progressBar_Food);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.progressBar_Physical);
            this.splitContainer1.Size = new System.Drawing.Size(200, 25);
            this.splitContainer1.SplitterDistance = 94;
            this.splitContainer1.TabIndex = 3;
            // 
            // progressBar_Physical
            // 
            this.progressBar_Physical.BackColor = System.Drawing.Color.White;
            this.progressBar_Physical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar_Physical.Location = new System.Drawing.Point(0, 0);
            this.progressBar_Physical.Name = "progressBar_Physical";
            this.progressBar_Physical.Size = new System.Drawing.Size(102, 25);
            this.progressBar_Physical.TabIndex = 3;
            this.progressBar_Physical.Visible = false;
            // 
            // 応援ToolStripMenuItem
            // 
            this.応援ToolStripMenuItem.Name = "応援ToolStripMenuItem";
            this.応援ToolStripMenuItem.Size = new System.Drawing.Size(393, 50);
            this.応援ToolStripMenuItem.Text = "応援";
            this.応援ToolStripMenuItem.Click += new System.EventHandler(this.応援ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(200, 225);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.splitContainer1);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 終了ToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem アクションToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem パトカーToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ストップToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ひよこToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ポップコーンToolStripMenuItem;
        private System.Windows.Forms.ProgressBar progressBar_Food;
        private System.Windows.Forms.ToolStripMenuItem 体力の表示ToolStripMenuItem;
        private System.Windows.Forms.Timer timer_Physical;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ProgressBar progressBar_Physical;
        private System.Windows.Forms.ToolStripMenuItem 応援ToolStripMenuItem;
    }
}

