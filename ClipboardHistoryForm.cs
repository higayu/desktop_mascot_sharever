using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopMascot_Share {
    public partial class ClipboardHistoryForm : Form {
        private ClipboardHistoryManager manager;
        private ListBox listBox;
        private TextBox searchBox;

        public ClipboardHistoryForm(ClipboardHistoryManager manager) {
            InitializeComponent();
            this.manager = manager;

            this.Text = "コピー履歴";
            this.Size = new System.Drawing.Size(400, 350);

            // 検索ボックス
            searchBox = new TextBox() { Dock = DockStyle.Top };
            searchBox.TextChanged += SearchBox_TextChanged;

            // リストボックス
            listBox = new ListBox() {
                Dock = DockStyle.Fill,
                Font = new Font("MS UI Gothic", 14)
            };
            listBox.Items.AddRange(manager.History.ToArray());
            listBox.DoubleClick += (s, e) => {
                if (listBox.SelectedItem is ClipboardEntry entry) {
                    Clipboard.SetText(entry.Text);
                    MessageBox.Show("再コピーしました！", "コピー", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            // ボタンパネル
            var buttonPanel = new FlowLayoutPanel() { Dock = DockStyle.Bottom, Height = 40 };
            var clearButton = new Button() { Text = "履歴をクリア", Width = 100 };
            clearButton.Click += ClearButton_Click;

            var closeButton = new Button() { Text = "閉じる", Width = 100 };
            closeButton.Click += (s, e) => this.Close();

            buttonPanel.Controls.Add(clearButton);
            buttonPanel.Controls.Add(closeButton);

            // コントロール追加
            this.Controls.Add(listBox);
            this.Controls.Add(searchBox);
            this.Controls.Add(buttonPanel);
        }

        private void ClearButton_Click(object sender, EventArgs e) {
            manager.ClearHistory();
            RefreshList();
        }

        private void SearchBox_TextChanged(object sender, EventArgs e) {
            string keyword = searchBox.Text.ToLower();

            var filtered = manager.History
                .Where(x => x.Text.ToLower().Contains(keyword))
                .ToArray();

            listBox.BeginUpdate();
            listBox.Items.Clear();
            listBox.Items.AddRange(filtered);
            listBox.EndUpdate();
        }

        private void RefreshList() {
            listBox.BeginUpdate();
            listBox.Items.Clear();
            listBox.Items.AddRange(manager.History.ToArray());
            listBox.EndUpdate();
        }

        private void ClipboardHistoryForm_Load(object sender, EventArgs e) {
        
        }
    }
}
