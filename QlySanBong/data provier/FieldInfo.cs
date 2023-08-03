using QlySanBong.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QlySanBong.data_provier
{
    class FieldInfoDP : DataProvier
    {
        private static FieldInfoDP instance;
        public static FieldInfoDP Instance
        {
            get
            {
                if (instance == null)
                    instance = new FieldInfoDP();
                return FieldInfoDP.instance;
            }
            private set
            {
                FieldInfoDP.instance = value;
            }
        }

        private FieldInfoDP()
        {

        }
        public FieldInfo GetFieldInfo(string idFieldInfo)
        {
            try
            {
                OpenConnection();
                string queryString = "select * from FieldInfo where IdFieldInfo = " + idFieldInfo;

                SqlCommand command = new SqlCommand(queryString, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                FieldInfo res = new FieldInfo(int.Parse(idFieldInfo), int.Parse(dataTable.Rows[0].ItemArray[1].ToString()),
                    DateTime.Parse(dataTable.Rows[0].ItemArray[2].ToString()), DateTime.Parse(dataTable.Rows[0].ItemArray[3].ToString()),
                    int.Parse(dataTable.Rows[0].ItemArray[4].ToString()), dataTable.Rows[0].ItemArray[5].ToString(),
                    dataTable.Rows[0].ItemArray[6].ToString(), dataTable.Rows[0].ItemArray[7].ToString(),
                    long.Parse(dataTable.Rows[0].ItemArray[8].ToString()), long.Parse(dataTable.Rows[0].ItemArray[9].ToString()));
                return res;
            }
            catch
            {
                return new FieldInfo();
            }
            finally
            {
                CloseConnection();
            }
        }

        public List<FieldInfo> ConvertDBToList()
        {
            DataTable dt;
            List<FieldInfo> fieldInfos = new List<FieldInfo>();
            try
            {
                dt = LoadData("FieldInfo");
            }
            catch
            {
                dt = null;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                FieldInfo fieldInfo = new FieldInfo(int.Parse(dt.Rows[i].ItemArray[0].ToString()), int.Parse(dt.Rows[i].ItemArray[1].ToString()),
                    DateTime.Parse(dt.Rows[i].ItemArray[2].ToString()), DateTime.Parse(dt.Rows[i].ItemArray[3].ToString()),
                    int.Parse(dt.Rows[i].ItemArray[4].ToString()), dt.Rows[i].ItemArray[5].ToString(), dt.Rows[i].ItemArray[6].ToString(),
                    dt.Rows[i].ItemArray[7].ToString(), long.Parse(dt.Rows[i].ItemArray[8].ToString()), long.Parse(dt.Rows[i].ItemArray[9].ToString()));
                fieldInfos.Add(fieldInfo);
            }
            return fieldInfos;
        }

        //Update idField = null khi xóa Field
        public bool UpdateIdField(string idField)
        {
            try
            {
                OpenConnection();
                string query = @"update FieldInfo set IdField = NULL where IdField = " + idField;
                SqlCommand command = new SqlCommand(query, connect);
                int rs = command.ExecuteNonQuery();
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
        public List<FieldInfo> QueryFieldInfoPerDay(string year, string month, string day)
        {
            List<FieldInfo> res = new List<FieldInfo>();
            try
            {
                CloseConnection();
                OpenConnection();
                string queryString = @"Select IdFieldInfo,IdField, StartingTime,EndingTime,Status 
                                       From FieldInfo
                                       Where year(StartingTime)= @year and month(StartingTime)= @month and day(StartingTime)= @day 
                                       Order by StartingTime,IdField ASC ";
                SqlCommand command = new SqlCommand(queryString, connect);
                command.Parameters.AddWithValue("@year", year);
                command.Parameters.AddWithValue("@month", month);
                command.Parameters.AddWithValue("@day", day);
                command.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FieldInfo fieldInfo = new FieldInfo(int.Parse(dt.Rows[i].ItemArray[0].ToString()), int.Parse(dt.Rows[i].ItemArray[1].ToString()), DateTime.Parse(dt.Rows[i].ItemArray[2].ToString()), DateTime.Parse(dt.Rows[i].ItemArray[3].ToString()), int.Parse(dt.Rows[i].ItemArray[4].ToString()), " ", " ", " ", 0, 0);
                    res.Add(fieldInfo);
                }
                return res;
            }
            catch
            {
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public bool AddIntoDB(FieldInfo fieldInfo)
        {
            try
            {
                OpenConnection();
                string query = "INSERT INTO FieldInfo(IdFieldInfo,IdField,StartingTime,EndingTime,Status,PhoneNumber,CustomerName,Note,Discount,Price) " +
                               "VALUES(@idFieldInfo,@idField,@startingTime,@endingTime,@status,@phoneNumber,@customerName,@note,@discount,@price)";
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@idFieldInfo", fieldInfo.IdFieldInfo);
                command.Parameters.AddWithValue("@idField", fieldInfo.IdField);
                command.Parameters.AddWithValue("@startingTime", fieldInfo.StartingTime);
                command.Parameters.AddWithValue("@endingTime", fieldInfo.EndingTime);
                command.Parameters.AddWithValue("@status", fieldInfo.Status);
                command.Parameters.AddWithValue("@phoneNumber", fieldInfo.PhoneNumber);
                command.Parameters.AddWithValue("@customerName", fieldInfo.CustomerName);
                command.Parameters.AddWithValue("@note", fieldInfo.Note);
                command.Parameters.AddWithValue("@discount", fieldInfo.Discount);
                command.Parameters.AddWithValue("@price", fieldInfo.Price);
                int rs = command.ExecuteNonQuery();
                return true;
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
        public bool UpdateOnDB(FieldInfo fieldInfo)
        {
            try
            {
                OpenConnection();
                string query = @"Update FieldInfo
                                 Set PhoneNumber=@phoneNumber,Status=@status,CustomerName=@customerName,Note=@note,Discount=@discount
                                 Where IdFieldInfo = @idFieldInfo";
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@status", fieldInfo.Status);
                command.Parameters.AddWithValue("@phoneNumber", fieldInfo.PhoneNumber);
                command.Parameters.AddWithValue("@customerName", fieldInfo.CustomerName);
                command.Parameters.AddWithValue("@note", fieldInfo.Note);
                command.Parameters.AddWithValue("@discount", fieldInfo.Discount);
                command.Parameters.AddWithValue("@idFieldInfo", fieldInfo.IdFieldInfo);
                command.ExecuteNonQuery();
                return true;
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
        public bool DeleteFromDB(string idFieldInfo)
        {
            try
            {
                OpenConnection();
                string query = @"Delete from FieldInfo
                                Where IdFieldInfo=" + idFieldInfo;
                SqlCommand command = new SqlCommand(query, connect);
                command.ExecuteNonQuery();
                return true;
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
        public int GetMaxIdFieldInfo()
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = "select max(IdFieldInfo) from FieldInfo";

                SqlCommand command = new SqlCommand(queryString, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                res = int.Parse(dataTable.Rows[0].ItemArray[0].ToString());
            }
            catch
            {

            }
            finally
            {
                CloseConnection();
            }
            return res;
        }
        public List<FieldInfo> GetFieldInfoByIdField(string idField)
        {
            List<FieldInfo> fieldInfos = new List<FieldInfo>();
            try
            {
                OpenConnection();
                string queryString = "select * from FieldInfo where (Status=1 or Status=2) and IdField=" + idField;

                SqlCommand command = new SqlCommand(queryString, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    FieldInfo fieldInfo = new FieldInfo(int.Parse(dataTable.Rows[i].ItemArray[0].ToString()), int.Parse(dataTable.Rows[i].ItemArray[1].ToString()), DateTime.Parse(dataTable.Rows[i].ItemArray[2].ToString()), DateTime.Parse(dataTable.Rows[i].ItemArray[3].ToString()), int.Parse(dataTable.Rows[i].ItemArray[4].ToString()), " ", " ", " ", 0, 0);
                    fieldInfos.Add(fieldInfo);
                }
            }
            catch
            {

            }
            finally
            {
                CloseConnection();
            }
            return fieldInfos;
        }
        public long GetPriceByFieldInfoId(string idFieldInfo)
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = "select price from FieldInfo where IdFieldInfo = " + idFieldInfo;

                SqlCommand command = new SqlCommand(queryString, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                res = int.Parse(dataTable.Rows[0].ItemArray[0].ToString());
            }
            catch
            {

            }
            finally
            {
                CloseConnection();
            }
            return res;
        }
    }
}

