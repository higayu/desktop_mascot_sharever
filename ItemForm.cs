using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopMascot_Share {

    public enum Item_Mode {
        Hiyoko,
        PopCone,
        HotDog
    }

    public partial class ItemForm : Form {

        // ドラッグ用
        private bool mouseDown = false;
        private Point mouseOffset;

        private int centerY;
        Item_Mode item_Mode;


        public ItemForm(Item_Mode item_) {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.BackColor = Color.Magenta;
            this.TransparencyKey = Color.Magenta;

            // 初期位置
            this.Location = new Point(100, 300);
            centerY = this.Top;

            // イベント登録
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;

            // PictureBox1に画像を設定
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            item_Mode = item_;
            switch (item_Mode) {
                case Item_Mode.Hiyoko:
                    pictureBox1.Image = Properties.Resources.Hiyoko;

                    break;

                case Item_Mode.HotDog:
                    pictureBox1.Image = Properties.Resources.Hiyoko;
                    break;

                case Item_Mode.PopCone:
                    pictureBox1.Image = Properties.Resources.PopCone;
                    break;
            }

            // タイマー設定
            timer1.Interval = 100;
            timer1.Tick += timer1_Tick;
            timer1.Start();

            // ドラッグアンドドロップの設定
            this.AllowDrop = false;  // このフォームはドロップを受け付けない
            //this.pictureBox1.MouseDown += PictureBox1_MouseDown;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                mouseDown = true;
                mouseOffset = new Point(e.X, e.Y);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e) {
            if (mouseDown) {
                this.Left = Cursor.Position.X - mouseOffset.X;
                this.Top = Cursor.Position.Y - mouseOffset.Y;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e) {
            mouseDown = false;

            // Form1との位置関係をチェック
            Form mainForm = Application.OpenForms["Form1"];
            if (mainForm != null) {
                // このフォームの領域
                Rectangle thisBounds = new Rectangle(this.Location, this.Size);
                // Form1の領域
                Rectangle mainBounds = new Rectangle(mainForm.Location, mainForm.Size);

                // 領域が重なっているかチェック
                if (thisBounds.IntersectsWith(mainBounds)) {
                    // Form1のusamaru_modeを変更
                    Form1 form1 = (Form1)mainForm;
                    if(this.item_Mode == Item_Mode.Hiyoko) {
                        form1.ChangeToHiyokoGyuMode();
                    }else if (this.item_Mode == Item_Mode.PopCone) {
                        form1.ChangeToPopConeMode();
                    }

                    // このフォームを閉じる
                    this.Close();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e) {

        }


    }
}
