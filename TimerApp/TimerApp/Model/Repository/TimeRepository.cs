﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using TimerApp.Model.Context;
using TimerApp.Model.Entity;

namespace TimerApp.Model.Repository
{
    public class TimeRepository
    {
        private OleDbConnection _conn;

        public TimeRepository(DbContext context)
        {
            _conn = context.Conn;
        }

        public int Create(Time time)
        {
            int result = 0;

            string sql = @"insert into tb_time (log, tanggal, jam, menit, detik) values (@log, @tanggal, @jam, @menit, @detik)";

            using (OleDbCommand cmd=new OleDbCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@log", time.NamaLog);
                cmd.Parameters.AddWithValue("@tanggal", time.Tanggal);
                cmd.Parameters.AddWithValue("@jam", time.Jam);
                cmd.Parameters.AddWithValue("@menit", time.Menit);
                cmd.Parameters.AddWithValue("@detik", time.Detik);

                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("Create error: {0}", ex.Message);
                }
            }

            return result;
        }

        public int Delete(Time time)
        {
            int result = 0;

            string sql = @"delete from tb_time where log_id = @log_id";

            using (OleDbCommand cmd=new OleDbCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@log_id", time.LogId);

                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("Delete error: {0}", ex.Message);
                }
            }

            return result;
        }

        public List<Time> ReadByDate(DateTime date)
        {
            List<Time> list = new List<Time>();

            try
            {
               string sql = @"select*from tb_time where tanggal = @tanggal";

                using (OleDbCommand cmd = new OleDbCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@tanggal", date);

                    using (OleDbDataReader dtr = cmd.ExecuteReader())
                    {
                        while (dtr.Read())
                        {
                            var time = new Time();

                            time.LogId = int.Parse(dtr["log_id"].ToString());
                            time.NamaLog = dtr["log"].ToString();
                            time.Tanggal = DateTime.Parse(dtr["tanggal"].ToString());
                            time.Jam = int.Parse(dtr["jam"].ToString());
                            time.Menit = int.Parse(dtr["menit"].ToString());
                            time.Detik = int.Parse(dtr["detik"].ToString());

                            list.Add(time);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("ReadByDate error: {0}", ex.Message);
            }

            return list;
        }

        public Time ReadByID(int id)
        {
            var time = new Time();

            try
            {
                string sql = @"select jam, menit, detik from tb_time where log_id = @log_id";

                using (OleDbCommand cmd = new OleDbCommand(sql, _conn))
                {
                    cmd.Parameters.AddWithValue("@log_id", id);

                    using (OleDbDataReader dtr = cmd.ExecuteReader())
                    {
                        while (dtr.Read())
                        {
                            time.Jam = int.Parse(dtr["jam"].ToString());
                            time.Menit = int.Parse(dtr["menit"].ToString());
                            time.Detik = int.Parse(dtr["detik"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("ReadByID error: {0}", ex.Message);
            }

            return time;
        }
    }
}
