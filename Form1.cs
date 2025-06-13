using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace DesktopMascot_Share
{

    #region ----------------- enum うさまるのモード ----------------------------
    public enum Usamaru_Mode
    {
        Stop,
        Patoka,
        Walk,
        HiyokoGyu,
        PopCone,
        Cry,
        Sleep,
        Cheerleader
    }
    #endregion ----------------- enum うさまるのモード 末尾 ----------------------------

    public partial class Form1 : Form
    {

        #region ----------------- フィールド ----------------------------
        //private SoundPlayer Sound_Item;
        private ClipboardHistoryManager clipboardHistory;
        // ドラッグ用
        private bool mouseDown = false;
        private Point mouseOffset;

        //移動するフラグ
        private Usamaru_Mode usamaru_mode = Usamaru_Mode.Stop;

        // パトカーモード用の変数
        private int direction = 1; // 1=右, -1=左
        private int speedX = 5; // パトカーは速めに
        private int centerY;
        private double waveCounter = 0;
        private int waveAmplitude = 10; // パトカーは浮き沈みを小さめに

        private int Animation_Frame = 1; // アニメーション用のフレームカウンタ-
        private int Return_Anime_Counter = 0;

        private int physicalTimerCounter = 0; // 体力減少用のカウンター
        private int foodTimerCounter = 0; // 満腹度減少用のカウンター
        private int sleepTimerCounter = 0;//睡眠カウンター

        private Usamaru_Mode Before_Mode;//履歴モード
        #endregion ----------------- フィールド 末尾 ----------------------------

        #region ----------------- 初期化 ----------------------------
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.BackColor = Color.Magenta;
            this.TransparencyKey = Color.Magenta;
            this.StartPosition = FormStartPosition.Manual;

            // 初期位置
            this.Location = new Point(100, 300);
            centerY = this.Top;

            // PictureBox1に画像を設定
            pictureBox1.Image = Properties.Resources.main;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            // タイマー設定
            timer1.Interval = 50; // パトカーは速めに更新
            timer1.Enabled = true;

            // 体力タイマーの設定
            timer_Physical.Interval = 1000; // 1秒間隔
            timer_Physical.Enabled = true;

            // イベント登録
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;

            //clipboardHistory = new ClipboardHistoryManager();

            this.contextMenuStrip1.Items.Add("ファイルのパスを作成", null, Get_File_Paths);
            //this.contextMenuStrip1.Items.Add("コピー履歴を見る", null, ShowClipboardHistory);

            progressBar_Food.Style = ProgressBarStyle.Continuous;
            progressBar_Physical.Style = ProgressBarStyle.Continuous;
            // 非同期でデータを取得
            _ = InitializeFirebaseDataAsync();
        }

        private async Task InitializeFirebaseDataAsync()
        {
            try
            {
                await LoadData();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in InitializeFirebaseDataAsync: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"データの初期化中にエラーが発生しました: {ex.Message}", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion ----------------- 初期化 末尾 ----------------------------

        #region ----------------- 便利機能 ----------------------------
        private async Task LoadData()
        {
            try
            {

                // 設定の処理
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
                MessageBox.Show($"データの読み込み中にエラーが発生しました: {ex.Message}", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Get_File_Paths(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "ファイル一覧を作成するフォルダを選択してください";
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string selectedPath = folderDialog.SelectedPath;
                        var fileList = new List<string>();

                        // ヘッダー情報を追加
                        fileList.Add($"フォルダ一覧: {selectedPath}");
                        fileList.Add($"作成日時: {DateTime.Now:yyyy/MM/dd HH:mm:ss}");
                        fileList.Add(new string('-', 80));
                        fileList.Add("");

                        // ディレクトリとファイルを再帰的に取得
                        await Task.Run(() => {
                            ProcessDirectory(selectedPath, fileList, 0);
                        });

                        // 保存ダイアログを表示
                        using (SaveFileDialog saveDialog = new SaveFileDialog())
                        {
                            saveDialog.Filter = "テキストファイル (*.txt)|*.txt";
                            saveDialog.Title = "ファイル一覧を保存";
                            saveDialog.FileName = $"ファイル一覧_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                            saveDialog.DefaultExt = "txt";
                            saveDialog.AddExtension = true;

                            if (saveDialog.ShowDialog() == DialogResult.OK)
                            {
                                // ファイルに保存（非同期処理を使用）
                                await Task.Run(() => {
                                    File.WriteAllLines(saveDialog.FileName, fileList, Encoding.UTF8);
                                });
                                MessageBox.Show("ファイル一覧を保存しました。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"エラーが発生しました：{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ProcessDirectory(string path, List<string> fileList, int level)
        {
            string currentIndent = new string(' ', level * 2);
            try
            {
                // 現在のディレクトリ内のファイルを取得
                var files = Directory.GetFiles(path);
                foreach (var file in files.OrderBy(f => f))
                {
                    var fileInfo = new FileInfo(file);
                    string fileSize = FormatFileSize(fileInfo.Length);
                    string fileDate = fileInfo.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss");
                    fileList.Add($"{currentIndent}📄 {Path.GetFileName(file)} ({fileSize}) - {fileDate}");
                }

                // サブディレクトリを取得して処理
                var directories = Directory.GetDirectories(path);
                foreach (var dir in directories.OrderBy(d => d))
                {
                    var dirInfo = new DirectoryInfo(dir);
                    string dirDate = dirInfo.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss");
                    fileList.Add($"{currentIndent}📁 {Path.GetFileName(dir)} - {dirDate}");
                    ProcessDirectory(dir, fileList, level + 1);
                }
            }
            catch (UnauthorizedAccessException)
            {
                fileList.Add($"{currentIndent}⚠️ アクセスが拒否されたフォルダ: {Path.GetFileName(path)}");
            }
            catch (Exception ex)
            {
                fileList.Add($"{currentIndent}⚠️ エラー: {Path.GetFileName(path)} - {ex.Message}");
            }
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double size = bytes;

            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }

            return $"{size:0.##} {sizes[order]}";
        }

        private void ShowClipboardHistory(object sender, EventArgs e)
        {
            ClipboardHistoryForm historyForm = new ClipboardHistoryForm(clipboardHistory);
            historyForm.Show();
        }
        #endregion ----------------- 便利機能 末尾 ----------------------------

        #region ----------------- マウス ----------------------------
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            mouseOffset = new Point(e.X, e.Y);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Left = Cursor.Position.X - mouseOffset.X;
                this.Top = Cursor.Position.Y - mouseOffset.Y;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        #endregion ------------------- マウス末尾 ---------------------------------

        #region ----------------- Formロード CLOSE ----------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            // アプリケーションの設定を読み込む
            Properties.Settings.Default.Reload();

            try
            {
                progressBar_Food.Value = Properties.Settings.Default.Food_Level;
                progressBar_Physical.Value = Properties.Settings.Default.Physical_Strength;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine($"ロードイベントでエラーキャッチ");
            }
        }

        private void Form1_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.Food_Level = progressBar_Food.Value;
                Properties.Settings.Default.Physical_Strength = progressBar_Physical.Value;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine($"閉じるイベントでエラーキャッチ");
            }

            // アプリケーションの設定を保存する
            Properties.Settings.Default.Save();
        }
        #endregion ----------------- フォームイベント末尾 ----------------------------


        #region ----------------- タイマー処理 ----------------------------
            #region ---- 各モードアクション用タイマー -----
            private void timer1_Tick(object sender, EventArgs e)
            {
                switch (usamaru_mode)
                {
                    case Usamaru_Mode.Patoka:// 自動移動
                        PatoCar_Action();
                        break;

                    case Usamaru_Mode.HiyokoGyu:
                        // ひよこぎゅーアニメーション
                        HiyokoGyu_Action();
                        break;

                    case Usamaru_Mode.PopCone:
                        //  ポップコーンアニメーション
                        PopCone_Action();
                        break;

                    case Usamaru_Mode.Cry:
                        //  Cryアニメーション
                        Cry_Action();
                        break;

                    case Usamaru_Mode.Sleep:
                        //  睡眠
                        Sleep_Action();
                        break;

                }

            }
            #endregion ---- 各モード アクション用タイマー 末尾 -----

            #region ---- 減少系タイマー 末尾 -----
            private void timer_Physical_Tick(object sender, EventArgs e)
            {
                #region ---- 体力減少 -----
                if (progressBar_Physical.Value > 0 && (usamaru_mode != Usamaru_Mode.Sleep))
                {
                    if (physicalTimerCounter >= 60)
                    {

                        if ((progressBar_Physical.Value - 1) >= 0)
                        {
                            //System.Diagnostics.Debug.WriteLine($"体力減少");
                            progressBar_Physical.Value--;
                        }
                        physicalTimerCounter = 0;
                    }
                    else
                    {
                        physicalTimerCounter++;
                    }
                }
                else if ((usamaru_mode != Usamaru_Mode.Sleep) && (usamaru_mode != Usamaru_Mode.PopCone))
                {
                    //System.Diagnostics.Debug.WriteLine($"睡眠モードに変換");
                    Before_Mode = usamaru_mode;//履歴を保存
                    usamaru_mode = Usamaru_Mode.Sleep;

                    pictureBox1.Image = Properties.Resources.Sleep_1;
                    Animation_Frame = 5;
                    // タイマー設定
                    timer1.Interval = 200; // 更新速度変更
                    sleepTimerCounter = 0;
                }
                #endregion ---- 体力減少 末尾 -----

                #region ---- 食事減少 ----
                if (progressBar_Food.Value > 0 && usamaru_mode != Usamaru_Mode.PopCone)
                {
                    if (foodTimerCounter >= 60)
                    {
                        if ((progressBar_Food.Value - 2) > 0 && (usamaru_mode == Usamaru_Mode.Patoka))
                        {
                            progressBar_Food.Value -= 2;
                        }
                        else if ((progressBar_Food.Value - 1) >= 0)
                        {
                            progressBar_Food.Value--;
                        }
                        foodTimerCounter = 0;
                    }
                    else
                    {
                        foodTimerCounter++;
                    }
                }
                else if (progressBar_Food.Value <= 0 && (usamaru_mode != Usamaru_Mode.Cry) && (usamaru_mode != Usamaru_Mode.Sleep))
                {
                    Cry_Change();
                }
                #endregion ---- 食事減少 末尾 -----
            }
            #endregion ---- 減少系タイマー 末尾 -----

        #endregion ----------------- タイマー処理末尾 ----------------------------

        #region ----------------- クリックイベント ----------------------------
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            switch (usamaru_mode)
            {
                case Usamaru_Mode.Patoka:
                    SoundList.Sound_Patoka();
                    break;

                case Usamaru_Mode.Stop:
                    Stop_Image_Change();
                    break;

                case Usamaru_Mode.PopCone:
                    if (Animation_Frame == 18)
                    {
                        Eat_Sleep_After_Change();
                    }
                    break;

                case Usamaru_Mode.Sleep:
                    SoundList.Sound_Sleep();
                    break;
            }
        }
        #endregion ----------------- クリックイベント末尾 ----------------------------

        #region --------------------- モードチェンジ関数 ---------------------------
        private void Stop_Image_Change() {
            Image[] Pic_List = new Image[] {
                Properties.Resources.main,
                Properties.Resources.main2,
                Properties.Resources.main3
            };

            Random rnd = new Random();      // Randomオブジェクトを作成
            int num = rnd.Next(0, 3);        // 0から50までの値をランダムに取得
            pictureBox1.Image = Pic_List[num];
            usamaru_mode = Usamaru_Mode.Stop;
        }

        private void Patoka_Change() {
            Before_Mode = Usamaru_Mode.Patoka;//履歴を保存
            usamaru_mode = Usamaru_Mode.Patoka;

            pictureBox1.Image = Properties.Resources.Patoka_R; // 右向きで開始
            direction = 1; // 右向きに設定
                           // パトカーモード開始時の位置を保存
            centerY = this.Top;
            // タイマー設定
            timer1.Interval = 50; // パトカーは速めに更新
        }
        private void Hiyoko_Change() {
            Before_Mode = usamaru_mode;//履歴を保存
            usamaru_mode = Usamaru_Mode.HiyokoGyu;

            pictureBox1.Image = Properties.Resources.HiyokoGyu_1;
            Animation_Frame = 1;
            // タイマー設定
            timer1.Interval = 200; // 更新速度変更
        }
        public void PopCone_Change() {
            Before_Mode = usamaru_mode;//履歴を保存
            usamaru_mode = Usamaru_Mode.PopCone;

            pictureBox1.Image = Properties.Resources.PopCone_1;
            Animation_Frame = 1;
            Return_Anime_Counter = 0;
            // タイマー設定
            timer1.Interval = 200; // 更新速度変更
        }
        public void Cry_Change() {
            if(usamaru_mode != Usamaru_Mode.Cry)
            {
                Before_Mode = usamaru_mode;//履歴を保存
            }
            usamaru_mode = Usamaru_Mode.Cry;

            pictureBox1.Image = Properties.Resources.Cry_1;
            Animation_Frame = 1;
            // タイマー設定
            timer1.Interval = 200; // 更新速度変更
        }

        private void Eat_Sleep_After_Change() {

            switch (Before_Mode) {
                case Usamaru_Mode.Patoka:
                    Patoka_Change();
                    break;

                default:
                    Stop_Image_Change();
                    break;
            }
        }
        #endregion ------------------ モードチェンジ関数 末尾------------------------------------------

        #region ----------------- 右クリックイベント ----------------------------
        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void パトカーToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if ((progressBar_Food.Value > 0) && (usamaru_mode != Usamaru_Mode.Sleep))
            {
                Patoka_Change();
            }
            else if (progressBar_Food.Value <= 0)
            {
                MessageBox.Show("うさまるはお腹が空いているようです。\nご飯をあげてください。");
            }
            else if (usamaru_mode == Usamaru_Mode.Sleep)
            {
                MessageBox.Show("うさまるは疲れて寝ているようです。");
            }
        }

        private void ストップToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((progressBar_Food.Value > 0) && (usamaru_mode != Usamaru_Mode.Sleep))
            {
                Stop_Image_Change();
            }
            else if (progressBar_Food.Value <= 0)
            {
                MessageBox.Show("うさまるはお腹が空いているようです。\nご飯をあげてください。");
            }
            else if (usamaru_mode == Usamaru_Mode.Sleep)
            {
                MessageBox.Show("うさまるは疲れて寝ているようです。");
            }
        }

        private void ひよこToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((progressBar_Food.Value > 0) && (usamaru_mode != Usamaru_Mode.Sleep))
            {
                ItemForm itemForm = new ItemForm(Item_Mode.Hiyoko);
                itemForm.Show();
            }
            else if (progressBar_Food.Value <= 0)
            {
                MessageBox.Show("うさまるはお腹が空いているようです。\nご飯をあげてください。");
            }
            else if (usamaru_mode == Usamaru_Mode.Sleep)
            {
                MessageBox.Show("うさまるは疲れて寝ているようです。");
            }
        }

        private void ポップコーンToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (usamaru_mode != Usamaru_Mode.Sleep)
            {
                ItemForm itemForm = new ItemForm(Item_Mode.PopCone);
                itemForm.Show();
            }
            else if (usamaru_mode == Usamaru_Mode.PopCone)
            {

            }
            else if (usamaru_mode == Usamaru_Mode.Sleep)
            {
                MessageBox.Show("うさまるは疲れて寝ているようです。");
            }
        }

        private void 体力の表示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar_Food.Visible = !progressBar_Food.Visible;
            progressBar_Physical.Visible = !progressBar_Physical.Visible;
        }
        private void 応援ToolStripMenuItem_Click(object sender, EventArgs e) {
            pictureBox1.Image = Properties.Resources.Cheerleader; //
            usamaru_mode = Usamaru_Mode.Cheerleader;
            centerY = this.Top;
            // タイマー設定
            timer1.Interval = 200; //
        }
        #endregion ----------------- 右クリック末尾 ----------------------------


        #region ----------------- アイテムをあげた後の処理 ----------------------------
        public void ChangeToHiyokoGyuMode() {
            if (progressBar_Food.Value > 0) {
                Hiyoko_Change();
            } else {
                MessageBox.Show("うさまるはお腹が空いているようです。\nご飯をあげてください。");
            }
        }
        public void ChangeToPopConeMode() {
            if (Usamaru_Mode.Sleep == usamaru_mode) {
                MessageBox.Show("うさまるは疲れて寝ているようです。");
            } else {
                PopCone_Change();
            }
        }
        #endregion -----------------  ----------------------------

        #region ---- 各モードアクション -----
        private void PatoCar_Action() {
            if (!mouseDown) {
                this.Left += speedX * direction;

                // 画面端で反転
                var screenBounds = Screen.PrimaryScreen.WorkingArea;
                if (this.Right >= screenBounds.Right || this.Left <= screenBounds.Left) {
                    direction *= -1;
                    // 画像を切り替え
                    pictureBox1.Image = (direction == 1) ? Properties.Resources.Patoka_R : Properties.Resources.Patoka_L;
                }

                // 浮き沈み
                waveCounter += 0.2;
                int waveOffset = (int)(Math.Sin(waveCounter) * waveAmplitude);
                this.Top = centerY + waveOffset;
            } else {
                // ドラッグ中は中心Yを更新
                centerY = this.Top;
            }
        }
        private void HiyokoGyu_Action() {
            if (!mouseDown) {
                if (Animation_Frame != 20) {
                    // フレームを更新
                    Animation_Frame = (Animation_Frame + 1);

                    // 画像を切り替え
                    string resourceName = $"HiyokoGyu_{Animation_Frame}";
                    pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject(resourceName);
                }
            }
        }
        private void PopCone_Action() {
            if (!mouseDown) {
                if (Animation_Frame != 18) {

                    if (Return_Anime_Counter >= 5 && Animation_Frame >= 17) {
                        // フレームを更新
                        Animation_Frame++;
                    } else if (Animation_Frame >= 17) {
                        Return_Anime_Counter++;
                        Animation_Frame = 10;
                    } else {
                        // フレームを更新
                        if ((progressBar_Food.Value + 1) < 100) {
                            progressBar_Food.Value++;
                        }
                        Animation_Frame++;
                    }

                    // 画像を切り替え
                    string resourceName = $"PopCone_{Animation_Frame}";
                    pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject(resourceName);
                } else {
                    //if((progressBar1.Value + 10) >= 100) {
                    //   progressBar1.Value = 100;
                    //} else {
                    //    progressBar1.Value += 10;
                    //}
                }
            }
        }
        private void Cry_Action() {
            if (!mouseDown) {
                if (Animation_Frame != 12) {
                    // フレームを更新
                    Animation_Frame++;

                    // 画像を切り替え
                    string resourceName = $"Cry_{Animation_Frame}";
                    pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject(resourceName);
                }
            }
        }
        private void Sleep_Action() {
            if (!mouseDown) {
                if (progressBar_Physical.Value < 100) {

                    if (Animation_Frame < 17) {
                        // フレームを更新
                        Animation_Frame++;
                        // 画像を切り替え
                        string resourceName = $"Sleep_{Animation_Frame}";
                        pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject(resourceName);
                    }

                    if (sleepTimerCounter < 10) {
                        sleepTimerCounter++;
                        progressBar_Physical.Value++;
                    } else {
                        sleepTimerCounter = 0;
                    }

                } else {
                    Eat_Sleep_After_Change();
                }
            }
        }
        #endregion ---- 各モード アクション 末尾 -----
    }
}