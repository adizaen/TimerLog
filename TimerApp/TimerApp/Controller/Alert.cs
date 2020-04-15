using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Media;

namespace TimerApp.Controller
{
    public class Alert
    {
        private SoundPlayer player;

        // Untuk play sound ketika tombol alert diaktifkan
        public void AlertSound(int jam, int menit, int detik, int _jam, int _menit, int _detik)
        {
            if (_jam == jam && _menit == menit && _detik == detik)
            {
                player = new SoundPlayer();

                string location = Directory.GetCurrentDirectory() + "\\Media\\Music.wav";
                player.SoundLocation = location;
                player.PlayLooping();
            }
        }

        // Untuk stop sound ketika tombol stop ditekan
        public void StopSound()
        {
            player = new SoundPlayer();

            player.Stop();
        }
    }
}
