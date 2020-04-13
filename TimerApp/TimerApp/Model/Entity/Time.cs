using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimerApp.Model.Entity
{
    public class Time
    {
        public int LogId { get; set; }
        public string NamaLog { get; set; }
        public DateTime Tanggal { get; set; }
        public int Jam { get; set; }
        public int Menit { get; set; }
        public int Detik { get; set; }
    }
}
