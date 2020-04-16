using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TimerApp.Model.Entity;
using TimerApp.Controller;
using TimerApp.View;

namespace TimerApp.View
{
    public partial class FrmCountdown : Form
    {
        private System.Timers.Timer _timer;
        private int _jam, _menit, _detik, keterangan;
        private Alert alert;

        public FrmCountdown()
        {
            InitializeComponent();
            ViewAwal();
        }

        private void ViewAwal()
        {
            _jam = 0;
            _menit = 0;
            _detik = 0;
            SetTimer(_jam, _menit, _detik);

            dtStartAt.Text = "00:00:00";
            dtAlert.Text = "00:00:00";
            cbAlert.Checked = false;
            TextTombol(1); // start
            TextTombol(2); // stop

            this.Text = "Countdown";
        }

        // inisiasi keterangan
        // 1 -> start
        // 2 -> stop
        // 3 -> resume
        // 4 -> reset
        private void TextTombol(int number)
        {
            if (number == 1)
                btnStart.Text = "Start";
            else if (number == 2)
                btnStop.Text = "Stop";
            else if (number == 3)
                btnStart.Text = "Resume";
            else
                btnStop.Text = "Reset";
        }

        // Fungsi untuk set waktu pada label timer
        private void SetTimer(int Jam, int Menit, int Detik)
        {
            lblTimer.Text = string.Format("{0}:{1}:{2}", Jam.ToString().PadLeft(2, '0'),
                    Menit.ToString().PadLeft(2, '0'), Detik.ToString().PadLeft(2, '0'));
        }

        private void OnTimeEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            var jam = int.Parse(dtAlert.Value.Hour.ToString());
            var menit = int.Parse(dtAlert.Value.Minute.ToString());
            var detik = int.Parse(dtAlert.Value.Second.ToString());

            Invoke(new Action(() =>
            {
                if (_detik == 0)
                {
                    _detik = 60;
                    if (_menit != 0)
                        _menit -= 1;
                }

                if (_menit != 0 && _jam != 0)
                {
                    if (_menit == 0)
                    {
                        _menit = 59;
                        if (_jam != 0)
                            _jam -= 1;
                    }
                }

                _detik -= 1;

                if (_jam == 0 && _menit == 0 & _detik == 0)
                {
                    alert = new Alert();
                    alert.AlertSound(jam, menit, detik, _jam, _menit, _detik);

                    _timer.Stop();
                    ViewAwal();
                }

                SetTimer(_jam, _menit, _detik);
            }));

            if (cbAlert.Checked == true && _jam == jam && _menit == menit && _detik == detik)
            {
                alert = new Alert();
                alert.AlertSound(jam, menit, detik, _jam, _menit, _detik);
            }
        }

        private void FrmCountdown_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void FrmCountdown_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();

            toolTip1.SetToolTip(this.btnSwitch, "Switch to stopwatch mode");
            toolTip1.SetToolTip(this.btnStart, "Start timer");
            toolTip1.SetToolTip(this.btnStop, "Stop timer");
            toolTip1.SetToolTip(this.btnSet, "Custom start timer");
            toolTip1.SetToolTip(this.btnStop, "Stop alarm sound");

            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += OnTimeEvent;
        }

        private void FrmCountdown_Shown(object sender, EventArgs e)
        {
            lblTimer.Focus();
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            FrmTimer fTimer = new FrmTimer();
            fTimer.Show();
            this.Hide();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_jam != 0 || _menit != 0 || _detik != 0)
            {
                _timer.Start();
                this.Text = "Countdown - Running";

                TextTombol(1); // Start
                TextTombol(2); // Stop
                keterangan = 0;
            }
            else
                MessageBox.Show("Set timer terlebih dahulu!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_jam != 0 || _menit != 0 || _detik != 0)
            {
                _timer.Stop();
                this.Text = "Countdown - Stopped";

                TextTombol(3); // Resume
                TextTombol(4); // Reset
                keterangan += 1;

                if (keterangan == 2)
                    ViewAwal();
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            _jam = int.Parse(dtStartAt.Value.Hour.ToString());
            _menit = int.Parse(dtStartAt.Value.Minute.ToString());
            _detik = int.Parse(dtStartAt.Value.Second.ToString());

            SetTimer(_jam, _menit, _detik);
        }

        private void btnStopAlert_Click(object sender, EventArgs e)
        {
            alert = new Alert();
            alert.StopSound();
        }
    }
}
