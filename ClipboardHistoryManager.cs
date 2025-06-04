using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DesktopMascot_Share {
    public class ClipboardHistoryManager {
        private SQLite_Database db;

        // ✅ このプロパティを追加！
        public SQLite_Database Database => db;
        private string lastCopied;
        private Timer clipboardTimer;

        public IReadOnlyList<ClipboardEntry> History => db.GetAllEntries();

        public ClipboardHistoryManager() {
            db = new SQLite_Database();
            lastCopied = string.Empty;

            clipboardTimer = new Timer();
            clipboardTimer.Interval = 1000;
            clipboardTimer.Tick += CheckClipboard;
            clipboardTimer.Start();
        }

        private void CheckClipboard(object sender, EventArgs e) {
            try {
                if (Clipboard.ContainsText()) {
                    string currentText = Clipboard.GetText();
                    if (!string.IsNullOrWhiteSpace(currentText) && currentText != lastCopied) {
                        db.InsertOrUpdateEntry(currentText, "copy"); // ← ここで "copy" を指定
                        lastCopied = currentText;
                    }
                }
            } catch {
                // アクセス例外は無視
            }
        }

        public void ClearHistory() {
            db.Clear();
            lastCopied = string.Empty;
        }
    }


}
