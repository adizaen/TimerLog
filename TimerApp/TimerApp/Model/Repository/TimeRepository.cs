using System;
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

            string sql = @"insert into Time values (@nama_log, @jam, @menit, @detik)";

            using (OleDbCommand cmd=new OleDbCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@nama_log", time.NamaLog);
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

            string sql = @"delete from Time where log_id = @log_id";

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

        public List<Time> ReadAll()
        {
            List<Time> list = new List<Time>();

            try
            {
                string sql = @"select Log, Jam, Menit, Detik from Time order by log asc";

                using (OleDbCommand cmd=new OleDbCommand(sql, _conn))
                {
                    using (OleDbDataReader dtr=cmd.ExecuteReader())
                    {
                        while (dtr.Read())
                        {
                            Time time = new Time();

                            time.NamaLog = dtr["Log"].ToString();
                            time.Jam = int.Parse(dtr["Jam"].ToString());
                            time.Menit = int.Parse(dtr["Menit"].ToString());
                            time.Detik = int.Parse(dtr["detik"].ToString());

                            list.Add(time);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("ReadAll error: {0}", ex.Message);
            }

            return list;
        }
    }
}
