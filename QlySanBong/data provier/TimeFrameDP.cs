﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using QlySanBong.Model;

namespace QlySanBong.data_provier
{
    class TimeFrameDP : DataProvier
    {
        private static TimeFrameDP instance;
        public static TimeFrameDP Instance
        {
            get
            {
                if (instance == null)
                    return new TimeFrameDP();
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        public List<TimeFrame> ConvertDBToList()
        {
            List<TimeFrame> timeFrames = new List<TimeFrame>();
            DataTable dt;
            try
            {
                OpenConnection();
                dt = LoadData("TimeFrame");
            }
            catch
            {
                CloseConnection();
                dt = LoadData("TimeFrame");
            }
            DataView dv = dt.DefaultView;
            dv.Sort = "startTime ASC";
            dt = dv.ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                long price = -1;
                if (dt.Rows[i].ItemArray[4].ToString() != "")
                {
                    price = long.Parse(dt.Rows[i].ItemArray[4].ToString());
                }
                TimeFrame tmp = new TimeFrame(int.Parse(dt.Rows[i].ItemArray[0].ToString()), dt.Rows[i].ItemArray[1].ToString(),
                    dt.Rows[i].ItemArray[2].ToString(), int.Parse(dt.Rows[i].ItemArray[3].ToString()), price);
                timeFrames.Add(tmp);
            }
            return timeFrames;
        }
        public bool AddTimeFrame(TimeFrame time)
        {
            try
            {
                OpenConnection();
                string query = @"insert into TimeFrame(Id, StartTime, EndTime, FieldType, Price) values(@id, @startTime, @endTime, @fieldType, @price)";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@id", time.Id.ToString());
                cmd.Parameters.AddWithValue("@startTime", time.StartTime);
                cmd.Parameters.AddWithValue("@endTime", time.EndTime);
                cmd.Parameters.AddWithValue("@fieldType", time.FieldType);
                cmd.Parameters.AddWithValue("@price", time.Price);
                int rs = cmd.ExecuteNonQuery();
                if (rs == 1)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        //Xóa tất cả những khung giờ có loại sân được truyền vào
        public bool DeleteFieldType(string fieldType)
        {
            try
            {
                OpenConnection();
                string query = @"delete from TimeFrame where FieldType = " + fieldType;
                SqlCommand cmd = new SqlCommand(query, connect);
                int rs = cmd.ExecuteNonQuery();
                if (rs < 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return true;
            }
            finally
            {
                CloseConnection();
            }
        }
        public bool ClearData()
        {
            try
            {
                OpenConnection();
                string query = @"delete from TimeFrame";
                SqlCommand cmd = new SqlCommand(query, connect);
                int rs = cmd.ExecuteNonQuery();
                if (rs >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
        public int GetIdMax()
        {
            try
            {
                OpenConnection();
                string query = @"select max(Id) from TimeFrame";
                SqlCommand cmd = new SqlCommand(query, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows[0].ItemArray[0].ToString() == "")
                {
                    return 0;
                }
                else
                {
                    return int.Parse(dt.Rows[0].ItemArray[0].ToString());
                }
            }
            catch
            {
                return 0;
            }
            finally
            {
                CloseConnection();
            }
        }
        public bool CheckPriceIsNull()
        {
            try
            {
                OpenConnection();
                string query = @"select id from TimeFrame where price = -1";
                SqlCommand cmd = new SqlCommand(query, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
        public List<TimeFrame> GetTimeFrame()
        {
            try
            {
                OpenConnection();
                string query = @"select StartTime, EndTime from TimeFrame group by StartTime, EndTime";
                SqlCommand cmd = new SqlCommand(query, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                List<TimeFrame> timeFrames = new List<TimeFrame>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TimeFrame tmp = new TimeFrame(-1, dt.Rows[i].ItemArray[0].ToString(),
                        dt.Rows[i].ItemArray[1].ToString(), -1, -1);
                    timeFrames.Add(tmp);
                }
                return timeFrames;
            }
            catch
            {
                return new List<TimeFrame>();
            }
            finally
            {
                CloseConnection();
            }
        }
        public string GetPriceOfTimeFrame(string startingTime, string endingTime, string fieldType)
        {
            DataTable dt = new DataTable();
            try
            {
                OpenConnection();
                string query = @"Select Distinct TimeFrame.price 
                                From TimeFrame
                                Join FootballField on TimeFrame.FieldType = FootballField.Type
                                Where EndTime = @endingTime and StartTime = @startingTime and Type = @fieldType";
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@startingTime", startingTime);
                command.Parameters.AddWithValue("@endingTime", endingTime);
                command.Parameters.AddWithValue("@fieldType", fieldType);
                int rs = command.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                return dt.Rows[0].ItemArray[0].ToString();
            }
            catch
            {
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }
        public List<TimeFrame> GetEmptyTime(string idField, string day)
        {
            List<TimeFrame> timeFrames = new List<TimeFrame>();
            try
            {
                OpenConnection();
                string query = @"Select distinct TimeFrame.StartTime,TimeFrame.EndTime from TimeFrame
                                 Except
                                 Select convert(varchar(5), StartingTime, 108) as StartTime,convert(varchar(5), EndingTime, 108) as EndTime from FieldInfo
                                 where convert(varchar(10), StartingTime, 103)=@day and IdField =@idField";
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@idField", idField);
                command.Parameters.AddWithValue("@day", day);
                command.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TimeFrame timeFrame = new TimeFrame(0, dt.Rows[i].ItemArray[0].ToString(), dt.Rows[i].ItemArray[1].ToString(), 5, 0);
                    timeFrames.Add(timeFrame);
                }
            }
            catch
            {

            }
            finally
            {
                CloseConnection();
            }
            return timeFrames;
        }
    }
}
