using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopMascot_Share {
    public class ClipboardEntry {
        public int Id { get; set; }
        public string Text { get; set; }
        public string ItemType { get; set; }
        public long CountSum { get; set; }
        public DateTime UpdateAt { get; set; }
        public int? ItemState { get; set; }  // ← NULL許容
        public string ItemGroup { get; set; }  // ← VARCHAR & NULL許容に変更

        public override string ToString() {
            return $"{UpdateAt:yyyy/MM/dd HH:mm:ss} - {Text} (x{CountSum})";
        }
    }

}
