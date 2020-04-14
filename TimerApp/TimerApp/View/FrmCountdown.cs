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
        public FrmCountdown()
        {
            InitializeComponent();
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            FrmTimer fTimer = new FrmTimer();
            fTimer.Show();
            this.Hide();
        }

        private void FrmCountdown_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
