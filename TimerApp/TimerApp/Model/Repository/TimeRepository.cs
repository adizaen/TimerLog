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

            string sql = @"insert into Tbl_Time (Log, Jam, Menit, Detik) values (@Log, @Jam, @Menit, @Detik)";

            using (OleDbCommand cmd=new OleDbCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@Log", time.NamaLog);
                cmd.Parameters.AddWithValue("@Jam", time.Jam);
                cmd.Parameters.AddWithValue("@Menit", time.Menit);
                cmd.Parameters.AddWithValue("@Detik", time.Detik);

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

            string sql = @"delete from Tbl_Time where Id_Log = @Id_Log";

            using (OleDbCommand cmd=new OleDbCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@Id_Log", time.LogId);

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
               string sql = @"select*from Tbl_Time";

                using (OleDbCommand cmd = new OleDbCommand(sql, _conn))
                {
                    using (OleDbDataReader dtr = cmd.ExecuteReader())
                    {
                        while (dtr.Read())
                        {
                            Time time = new Time();

                            time.LogId = int.Parse(dtr["Id_Log"].ToString());
                            time.NamaLog = dtr["Log"].ToString();
                            time.Jam = int.Parse(dtr["Jam"].ToString());
                            time.Menit = int.Parse(dtr["Menit"].ToString());
                            time.Detik = int.Parse(dtr["Detik"].ToString());

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
