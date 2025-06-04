using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MarkdownSharp;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System.Diagnostics;

namespace DesktopMascot_Share {
    public partial class Data_PreviewForm : Form {
        public Data_PreviewForm(string question, string answer) {
            InitializeComponent();
            this.textBox1.Text = question;
            this.Text = "詳細表示"; // フォームのタイトルを設定
            // テキストボックスを読み取り専用にする (任意)
            this.textBox1.ReadOnly = true;
            InitializeWebView2(answer);
        }

        private async void InitializeWebView2(string answer) {
            try {
                // WebView2の環境オプションを設定
                var env = await CoreWebView2Environment.CreateAsync(null, "UserData", new CoreWebView2EnvironmentOptions());
                await webView2.EnsureCoreWebView2Async(env);

                // WebView2の設定
                webView2.CoreWebView2.Settings.IsScriptEnabled = true;
                webView2.CoreWebView2.Settings.AreDevToolsEnabled = true;
                webView2.CoreWebView2.Settings.IsWebMessageEnabled = true;
                webView2.CoreWebView2.Settings.IsStatusBarEnabled = true;
                webView2.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;
                webView2.CoreWebView2.Settings.IsPinchZoomEnabled = true;
                webView2.CoreWebView2.Settings.IsSwipeNavigationEnabled = true;
                webView2.CoreWebView2.Settings.IsZoomControlEnabled = true;

                // セキュリティ設定
                webView2.CoreWebView2.Settings.IsScriptEnabled = true;
                webView2.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = true;
                webView2.CoreWebView2.Settings.IsPasswordAutosaveEnabled = true;

                // ナビゲーション設定
                webView2.CoreWebView2.NavigationStarting += (sender, e) => {
                    Debug.WriteLine($"NavigationStarting: {e.Uri}");
                    
                    // 許可するURLパターン
                    if (e.Uri.StartsWith("file://") || 
                        e.Uri.StartsWith("data:") || 
                        e.Uri == "about:blank" || 
                        e.Uri == "about:blank#blocked") {
                        e.Cancel = false;
                        return;
                    }

                    // その他のURLは遷移をキャンセル
                    e.Cancel = true;
                    Debug.WriteLine($"Navigation blocked: {e.Uri}");
                };

                // ナビゲーション完了時の処理
                webView2.CoreWebView2.NavigationCompleted += (sender, e) => {
                    if (e.IsSuccess) {
                        Debug.WriteLine($"NavigationCompleted: {webView2.CoreWebView2.Source}");
                    } else {
                        if (e.WebErrorStatus != CoreWebView2WebErrorStatus.OperationCanceled) {
                            Debug.WriteLine($"NavigationFailed: {webView2.CoreWebView2.Source}, Error: {e.WebErrorStatus}");
                        }
                    }
                };

                // エラー処理の設定
                webView2.CoreWebView2.ProcessFailed += (sender, e) => {
                    Debug.WriteLine($"ProcessFailed: {e.ProcessFailedKind}");
                    if (e.ProcessFailedKind != CoreWebView2ProcessFailedKind.BrowserProcessExited) {
                        MessageBox.Show($"WebView2プロセスが失敗しました: {e.ProcessFailedKind}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                // マークダウンの変換と表示
                var md = new Markdown();
                string htmlContent = md.Transform(answer);

                // HTMLの基本構造を追加
                string fullHtml = $@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <style>
        body {{ 
            font-family: Arial, sans-serif; 
            margin: 20px;
            line-height: 1.6;
        }}
        pre {{ 
            background-color: #f5f5f5;
            padding: 10px;
            border-radius: 5px;
            overflow-x: auto;
        }}
        code {{ 
            font-family: Consolas, Monaco, 'Andale Mono', monospace;
        }}
        img {{ 
            max-width: 100%;
            height: auto;
        }}
    </style>
</head>
<body>
{htmlContent}
</body>
</html>";

                webView2.NavigateToString(fullHtml);
            }
            catch (Exception ex) {
                Debug.WriteLine($"WebView2の初期化に失敗しました: {ex.Message}");
                MessageBox.Show($"WebView2の初期化に失敗しました。\nMicrosoft Edge WebView2ランタイムがインストールされているか確認してください。\n\nエラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
