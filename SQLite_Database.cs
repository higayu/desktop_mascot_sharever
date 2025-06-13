using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace DesktopMascot_Share {
    public class SQLite_Database {
        private readonly string connectionString = "Data Source=desktopmascot.db";

        public SQLite_Database() {
            using (var connection = new SQLiteConnection(connectionString)) {
                connection.Open();

                // ① コピー/タスク記録テーブル
                string sqlDesktopMemo = @"
            CREATE TABLE IF NOT EXISTS desktopmemo (
                data_id INTEGER PRIMARY KEY AUTOINCREMENT,
                txt_val TEXT NOT NULL,
                item_type VARCHAR(50),
                count_sum BIGINT NOT NULL DEFAULT 1,
                update_at DATETIME NOT NULL,
                item_state INTEGER NULL,
                item_group VARCHAR(100) NULL
            );";
                using (var command = new SQLiteCommand(sqlDesktopMemo, connection)) {
                    command.ExecuteNonQuery();
                }

                // ② グループ管理テーブル（← ここに追加！）
                string sqlTaskGroups = @"
            CREATE TABLE IF NOT EXISTS taskgroups (
                group_id INTEGER PRIMARY KEY AUTOINCREMENT,
                group_name TEXT NOT NULL UNIQUE,
                color_code TEXT NULL,
                sort_order INTEGER NULL
            );";
                using (var command = new SQLiteCommand(sqlTaskGroups, connection)) {
                    command.ExecuteNonQuery();
                }

                // ③ AI会話履歴テーブル
                string sqlAiConversations = @"
                CREATE TABLE IF NOT EXISTS ai_conversations (
                    conversation_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    title TEXT NULL,
                    question TEXT NOT NULL,
                    answer TEXT NOT NULL,
                    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                );";
                using (var command = new SQLiteCommand(sqlAiConversations, connection)) {
                    command.ExecuteNonQuery();
                }
            }
        }


        public void InsertOrUpdateEntry(string text, string type = "copy", int? state = null, string group = null) {
            using (var connection = new SQLiteConnection(connectionString)) {
                connection.Open();

                var checkCmd = new SQLiteCommand("SELECT data_id, count_sum FROM desktopmemo WHERE txt_val = @text", connection);
                checkCmd.Parameters.AddWithValue("@text", text);
                var reader = checkCmd.ExecuteReader();

                if (reader.Read()) {
                    int id = Convert.ToInt32(reader["data_id"]);
                    long currentCount = Convert.ToInt64(reader["count_sum"]);
                    reader.Close();

                    var updateCmd = new SQLiteCommand(@"
                        UPDATE desktopmemo 
                        SET count_sum = @count, update_at = @time 
                        WHERE data_id = @id", connection);
                    updateCmd.Parameters.AddWithValue("@count", currentCount + 1);
                    updateCmd.Parameters.AddWithValue("@time", DateTime.Now);
                    updateCmd.Parameters.AddWithValue("@id", id);
                    updateCmd.ExecuteNonQuery();
                } else {
                    reader.Close();
                    var insertCmd = new SQLiteCommand(@"
                        INSERT INTO desktopmemo 
                            (txt_val, item_type, count_sum, update_at, item_state, item_group) 
                        VALUES (@text, @type, 1, @time, @state, @group)", connection);

                    insertCmd.Parameters.AddWithValue("@text", text);
                    insertCmd.Parameters.AddWithValue("@type", type);
                    insertCmd.Parameters.AddWithValue("@time", DateTime.Now);
                    insertCmd.Parameters.AddWithValue("@state", state.HasValue ? (object)state.Value : DBNull.Value);
                    insertCmd.Parameters.AddWithValue("@group", group != null ? (object)group : DBNull.Value);
                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        public List<ClipboardEntry> GetAllEntries() {
            var list = new List<ClipboardEntry>();
            using (var connection = new SQLiteConnection(connectionString)) {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT * FROM desktopmemo ORDER BY update_at DESC", connection);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        list.Add(new ClipboardEntry {
                            Id = Convert.ToInt32(reader["data_id"]),
                            Text = reader["txt_val"].ToString(),
                            ItemType = reader["item_type"].ToString(),
                            CountSum = Convert.ToInt64(reader["count_sum"]),
                            UpdateAt = Convert.ToDateTime(reader["update_at"]),
                            ItemState = reader["item_state"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["item_state"]),
                            ItemGroup = reader["item_group"] == DBNull.Value ? null : reader["item_group"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public void Clear() {
            using (var connection = new SQLiteConnection(connectionString)) {
                connection.Open();
                var cmd = new SQLiteCommand("DELETE FROM desktopmemo", connection);
                cmd.ExecuteNonQuery();
            }
        }

        public void AddTask(string text, string group = null) {
            InsertOrUpdateEntry(text, "task", 0, group);
        }

        public List<ClipboardEntry> GetAllTasks(string group = null) {
            var result = new List<ClipboardEntry>();
            using (var conn = new SQLiteConnection(connectionString)) {
                conn.Open();
                string sql = "SELECT * FROM desktopmemo WHERE item_type = 'task'";
                if (!string.IsNullOrEmpty(group))
                    sql += " AND item_group = @group";
                sql += " ORDER BY update_at DESC";

                var cmd = new SQLiteCommand(sql, conn);
                if (!string.IsNullOrEmpty(group))
                    cmd.Parameters.AddWithValue("@group", group);

                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    result.Add(new ClipboardEntry {
                        Id = Convert.ToInt32(reader["data_id"]),
                        Text = reader["txt_val"].ToString(),
                        ItemType = reader["item_type"].ToString(),
                        CountSum = Convert.ToInt64(reader["count_sum"]),
                        UpdateAt = Convert.ToDateTime(reader["update_at"]),
                        ItemState = reader["item_state"] == DBNull.Value ? -1 : Convert.ToInt32(reader["item_state"]),
                        ItemGroup = reader["item_group"] == DBNull.Value ? null : reader["item_group"].ToString()
                    });
                }
            }
            return result;
        }

        public void MarkTaskAsDone(int id) {
            using (var conn = new SQLiteConnection(connectionString)) {
                conn.Open();
                var cmd = new SQLiteCommand("UPDATE desktopmemo SET item_state = 1 WHERE data_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }


        // AI会話履歴の保存
        public void SaveAiConversation(string question, string answer, string title = null) {
            using (var conn = new SQLiteConnection(connectionString)) {
                conn.Open();
                var cmd = new SQLiteCommand(@"
                    INSERT INTO ai_conversations (title, question, answer, created_at) 
                    VALUES (@title, @question, @answer, @created_at)", conn);

                cmd.Parameters.AddWithValue("@title", title != null ? (object)title : DBNull.Value);
                cmd.Parameters.AddWithValue("@question", question);
                cmd.Parameters.AddWithValue("@answer", answer);
                cmd.Parameters.AddWithValue("@created_at", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
        }

        // AI会話履歴の取得
        public List<AiConversation> GetAiConversations() {
            var conversations = new List<AiConversation>();
            using (var conn = new SQLiteConnection(connectionString)) {
                conn.Open();
                var cmd = new SQLiteCommand("SELECT * FROM ai_conversations ORDER BY created_at DESC", conn);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        conversations.Add(new AiConversation {
                            Id = Convert.ToInt32(reader["conversation_id"]),
                            Title = reader["title"] == DBNull.Value ? null : reader["title"].ToString(),
                            Question = reader["question"].ToString(),
                            Answer = reader["answer"].ToString(),
                            CreatedAt = Convert.ToDateTime(reader["created_at"])
                        });
                    }
                }
            }
            return conversations;
        }

        // AI会話履歴の削除
        public void DeleteAiConversation(int conversationId) {
            using (var conn = new SQLiteConnection(connectionString)) {
                conn.Open();
                var cmd = new SQLiteCommand("DELETE FROM ai_conversations WHERE conversation_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", conversationId);
                cmd.ExecuteNonQuery();
            }
        }

        // 1週間以上過去のクリップボードデータを削除
        public void DeleteOldClipboardData() {
            using (var conn = new SQLiteConnection(connectionString)) {
                conn.Open();
                // 1週間前の日時を計算
                var oneWeekAgo = DateTime.Now.AddDays(-7);

                var cmd = new SQLiteCommand("DELETE FROM desktopmemo WHERE update_at < @oneWeekAgo", conn);
                cmd.Parameters.AddWithValue("@oneWeekAgo", oneWeekAgo);
                int deletedCount = cmd.ExecuteNonQuery();

                System.Diagnostics.Debug.WriteLine($"削除された古いクリップボードデータ: {deletedCount}件");
            }
        }
    }

    // AI会話履歴のモデルクラス
    public class AiConversation {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
