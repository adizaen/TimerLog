using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimerApp.Model.Context;
using System.Windows.Forms;
using TimerApp.Model.Entity;
using TimerApp.Model.Repository;

namespace TimerApp.Controller
{
    public class TimeController
    {
        private TimeRepository _repository;

        public int Create(Time time)
        {
            int result = 0;

            if (string.IsNullOrEmpty(time.NamaLog))
            {
                MessageBox.Show("Nama log harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }

            if (string.IsNullOrEmpty(time.Jam.ToString()))
            {
                MessageBox.Show("Jam harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }

            if (string.IsNullOrEmpty(time.Menit.ToString()))
            {
                MessageBox.Show("Menit harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }

            if (string.IsNullOrEmpty(time.Detik.ToString()))
            {
                MessageBox.Show("Detik harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }

            using (DbContext context = new DbContext())
            {
                _repository = new TimeRepository(context);
                result = _repository.Create(time);
            }

            return result;
        }

        public int Delete(Time time)
        {
            int result = 0;

            if (string.IsNullOrEmpty(time.LogId.ToString()))
            {
                MessageBox.Show("Log ID harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }

            using (DbContext context = new DbContext())
            {
                _repository = new TimeRepository(context);
                result = _repository.Delete(time);
            }

            return result;
        }

        public List<Time> ReadAll()
        {
            List<Time> list = new List<Time>();

            using (DbContext context = new DbContext())
            {
                _repository = new TimeRepository(context);
                list = _repository.ReadAll();
            }

            return list;
        }
    }
}
