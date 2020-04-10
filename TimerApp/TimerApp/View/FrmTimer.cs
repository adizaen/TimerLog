using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TimerApp
{
    public partial class FrmTimer : Form
    {
        private System.Timers.Timer _timer;
        private int _jam, _menit, _detik;

        public FrmTimer()
        {
            InitializeComponent();
            ViewAwal();
        }

        // Fungsi untuk menampilkan tampilan default
        private void ViewAwal()
        {
            _timer.Stop();
            _jam = 0;
            _menit = 0;
            _detik = 0;
            lblTimer.Text = "00:00:00";
            SetTimer(_jam, _menit, _detik);
            this.Text = "Timer";
        }

        // Fungsi untuk set waktu pada label timer
        private void SetTimer(int Jam, int Menit, int Detik)
        {
            lblTimer.Text = string.Format("{0}:{1}:{2}", Jam.ToString().PadLeft(2, '0'),
                    Menit.ToString().PadLeft(2, '0'), Detik.ToString().PadLeft(2, '0'));
        }
       
        private void OnTimeEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                _detik += 1;

                if (_detik == 60)
                {
                    _detik = 0;
                    _menit += 1;
                }
                if (_menit == 60)
                {
                    _menit = 0;
                    _jam += 1;
                }

                SetTimer(_jam, _menit, _detik);
            }));
        }

        private void FrmTimer_Load(object sender, EventArgs e)
        {
            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += OnTimeEvent;
            lblTimer.Focus();
        }

        private void FrmTimer_FormClosing(object sender, FormClosingEventArgs e)
        {
            _timer.Stop();
            Application.DoEvents();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _timer.Start();
            this.Text = "Timer - Running";
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            _timer.Stop();
            this.Text = "Timer - Paused";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            btnPause_Click(this, e);
            ViewAwal();
        }

        private void btnLog_Click(object sender, EventArgs e)
        {

        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            var startAt = dtStartAt.Value.ToString("HH:MM:ss");

            _jam = int.Parse(dtStartAt.Value.Hour.ToString());
            _menit = int.Parse(dtStartAt.Value.Minute.ToString());
            _detik = int.Parse(dtStartAt.Value.Second.ToString());

            SetTimer(_jam, _menit, _detik);
        }

        private void btnStartAt_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }
    }
}