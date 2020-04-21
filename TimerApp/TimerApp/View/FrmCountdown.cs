using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TimerApp.Controller;

namespace TimerApp.View
{
    public partial class FrmCountdown : Form
    {
        private DateTime _stopTime = new DateTime();
        private int _jam, _menit, _detik, jam, menit, detik, keterangan;

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

        private void btnSet_Click(object sender, EventArgs e)
        {
            _jam = int.Parse(dtStartAt.Value.Hour.ToString());
            _menit = int.Parse(dtStartAt.Value.Minute.ToString());
            _detik = int.Parse(dtStartAt.Value.Second.ToString());

            SetTimer(_jam, _menit, _detik);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (lblTimer.Text != "00:00:00")
            {
                timer1.Enabled = true;
                timer1.Start();
                TimeSpan durasi = TimeSpan.Parse(lblTimer.Text);
                _stopTime = DateTime.Now.Add(durasi);

                keterangan = 0;
                TextTombol(1);
                TextTombol(2);
                this.Text = "Countdown - Running";
            }
            else
                MessageBox.Show("Set timer terlebih dahulu!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void dtAlert_ValueChanged(object sender, EventArgs e)
        {
            if (cbAlert.Checked == true)
            {
                jam = int.Parse(dtAlert.Value.Hour.ToString());
                menit = int.Parse(dtAlert.Value.Minute.ToString());
                detik = int.Parse(dtAlert.Value.Second.ToString());
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (lblTimer.Text != "00:00:00")
            {
                timer1.Enabled = false;
                timer1.Stop();
                keterangan += 1;
                TextTombol(3);
                TextTombol(4);
                this.Text = "Countdown - Stopped";

                if (keterangan == 2)
                    ViewAwal();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan sisa = _stopTime.Subtract(DateTime.Now);

            _jam = sisa.Hours;
            _menit = sisa.Minutes;
            _detik = sisa.Seconds;

            SetTimer(_jam, _menit, _detik);

            if (cbAlert.Checked == true && _jam == jam && _menit == menit && _detik == detik)
            {
                alert = new Alert();
                alert.AlertSound(jam, menit, detik, _jam, _menit, _detik);
            }

            if (sisa.TotalSeconds <= 0)
            {
                sisa = TimeSpan.Zero;
                timer1.Enabled = false;
                timer1.Stop();
                ViewAwal();
            }         
        }

        private void btnStopAlert_Click(object sender, EventArgs e)
        {
            alert = new Alert();
            alert.StopSound();
        }
    }
}
