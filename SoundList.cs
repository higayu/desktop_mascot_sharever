using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace DesktopMascot_Share {
    public static class SoundList {

        public static void Sound_Patoka() {
            SoundPlayer[] Sound_ItemList = new SoundPlayer[] {
                new SoundPlayer(Properties.Resources.Car_Horn),
                new SoundPlayer(Properties.Resources.Police_Car_Siren1),
                new SoundPlayer(Properties.Resources.Police_Car_Siren2)
            };

            Random rnd = new Random();      // Randomオブジェクトを作成
            int num = rnd.Next(0, 3);        // 0から50までの値をランダムに取得
            Sound_ItemList[num].Play();

            // 音声
            //Sound_Item = new SoundPlayer(Properties.Resources.click);
            //Sound_Item.Play();
        }

        public static void Sound_Sleep() {
            // 音声
            SoundPlayer Sound_Item = new SoundPlayer(Properties.Resources.Sleep_Sound);
            Sound_Item.Play();
        }
    }
}
