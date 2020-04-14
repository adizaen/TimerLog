using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Media;
using System.Windows.Forms;
using TimerApp.Model.Entity;
using TimerApp.Controller;
using TimerApp.View;

namespace TimerApp
{
    public partial class FrmTimer : Form
    {
        private System.Timers.Timer _timer;
        private int _jam, _menit, _detik;

        private List<Time> listOfTime = new List<Time>();
        private TimeController controller;
        private SoundPlayer player;

        public FrmTimer()
        {
            InitializeComponent();
            controller = new TimeController();
            InisialisasiDataGridView();
            ViewAwal();
        }

        private void InisialisasiDataGridView()
        {
            dgv.Columns.Add("no", "No.");
            dgv.Columns.Add("id", "ID");
            dgv.Columns.Add("nama", "Log");
            dgv.Columns.Add("tgl", "Tanggal");
            dgv.Columns.Add("waktu", "Time");

            dgv.Columns[0].Width = 40;
            dgv.Columns[1].Width = 50;
            dgv.Columns[2].Width = 100;
            dgv.Columns[3].Width = 100;
            dgv.Columns[4].Width = 110;

            dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns[1].Visible = false;
            dgv.Columns[3].Visible = false;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9.75F);
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AllowUserToAddRows = false;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.ReadOnly = true;
        }

        // Fungsi untuk menampilkan tampilan default
        private void ViewAwal()
        {
            _jam = 0;
            _menit = 0;
            _detik = 0;
            SetTimer(_jam, _menit, _detik);

            dtDate.Value = DateTime.Today;
            dtStartAt.Text = "00:00:00";
            dtAlert.Text = "00:00:00";
            cbAlert.Checked = false;

            TampilkanDataGridView(dtDate.Value);
            btnStart.Text = "Start";
            this.Text = "Stopwatch";
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
                if (_jam == 24 && _menit == 0 & _detik == 0)
                {
                    _jam = 0;
                    _menit = 0;
                    _detik = 0;
                }

                SetTimer(_jam, _menit, _detik);
            }));

            if (cbAlert.Checked == true)
            {
                var jam = int.Parse(dtAlert.Value.Hour.ToString());
                var menit = int.Parse(dtAlert.Value.Minute.ToString());
                var detik = int.Parse(dtAlert.Value.Second.ToString());

                if (_jam == jam && _menit == menit && _detik == detik)
                {
                    player = new SoundPlayer();

                    string location = Directory.GetCurrentDirectory() + "\\Media\\Music.wav";
                    player.SoundLocation = location;
                    player.PlayLooping();
                }
            }
        }

        private int CountDataGridView()
        {
            int count = 0;
            count = dgv.Rows.Count + 1;

            return count;
        }

        private void TampilkanDataGridView(DateTime dateTime)
        {
            dgv.Rows.Clear();

            listOfTime = controller.ReadByDate(dateTime);

            if (listOfTime.Count != 0)
            {
                foreach (var time in listOfTime)
                {
                    var noUrut = CountDataGridView();
                    string waktu = time.Jam.ToString("D2") + ":" + time.Menit.ToString("D2") + ":" + time.Detik.ToString("D2");

                    dgv.Rows.Add(noUrut.ToString(), time.LogId, time.NamaLog, time.Tanggal, waktu);
                }

                dgv.CurrentCell = dgv.Rows[CountDataGridView() - 2].Cells[0];
            }
            else if (dtDate.Value < DateTime.Today)
            {
                MessageBox.Show("Tidak ada riwayat log!", "Peringatan", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            else if (dtDate.Value > DateTime.Today)
            {
                string message = "Maaf, belum ada riwayat log! " + "\n" + "Hari ini tanggal " + DateTime.Today.ToLongDateString();
                MessageBox.Show(message, "Peringatan", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        private void FrmTimer_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();

            toolTip1.SetToolTip(this.btnSwitch, "Switch to countdown mode");
            toolTip1.SetToolTip(this.btnStart, "Start timer");
            toolTip1.SetToolTip(this.btnPause, "Pause timer");
            toolTip1.SetToolTip(this.btnReset, "Reset timer to default");
            toolTip1.SetToolTip(this.btnLog, "Record log time");
            toolTip1.SetToolTip(this.btnSet, "Custom start timer");
            toolTip1.SetToolTip(this.btnStop, "Stop alarm sound");
            toolTip1.SetToolTip(this.btnStartAt, "Start timer from selected row");
            toolTip1.SetToolTip(this.btnDelete, "Delete selected row");

            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += OnTimeEvent;
        }

        private void FrmTimer_FormClosing(object sender, FormClosingEventArgs e)
        {
            _timer.Stop();
            Application.DoEvents();
            Application.Exit();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _timer.Start();
            this.Text = "Stopwatch - Running";
            btnStart.Text = "Start";
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            _timer.Stop();
            this.Text = "Stopwatch - Paused";

            btnStart.Text = "Resume";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _timer.Stop();
            ViewAwal();
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            var time = new Time();

            time.NamaLog = "Log " + CountDataGridView().ToString();
            time.Tanggal = DateTime.Parse(DateTime.Now.ToLongDateString());
            time.Jam = _jam;
            time.Menit = _menit;
            time.Detik = _detik;
            controller.Create(time);

            TampilkanDataGridView(DateTime.Today);
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            _jam = int.Parse(dtStartAt.Value.Hour.ToString());
            _menit = int.Parse(dtStartAt.Value.Minute.ToString());
            _detik = int.Parse(dtStartAt.Value.Second.ToString());

            SetTimer(_jam, _menit, _detik);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (this.player.IsLoadCompleted)
                player.Stop();
        }

        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtDate.Value != DateTime.Today)
                btnLog.Enabled = false;
            else
                btnLog.Enabled = true;

            TampilkanDataGridView(dtDate.Value);
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmCountdown fCount = new FrmCountdown();
            fCount.Show();
        }

        private void btnStartAt_Click(object sender, EventArgs e)
        {
            var time = new Time();
            int id = int.Parse(dgv.SelectedCells[1].Value.ToString());
            time = controller.ReadByID(id);

            _jam = time.Jam;
            _menit = time.Menit;
            _detik = time.Detik;

            dtStartAt.Text = _jam.ToString("D2") + ":" + _menit.ToString("D2") + ":" + _detik.ToString("D2");
            btnSet_Click(this, e);
            btnStart_Click(this, e);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                var konfirmasi = MessageBox.Show("Apakah data log ingin dihapus?", "Konfirmasi",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (konfirmasi == DialogResult.Yes)
                {
                    Time time = listOfTime[dgv.CurrentCell.RowIndex];
                    
                    var result = controller.Delete(time);
                    if (result > 0)
                        TampilkanDataGridView(DateTime.Today);
                }
            }
            else
            {
                MessageBox.Show("Data log belum dipilih!", "Peringatan",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}