using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using QlySanBong.Model;
using System.Data.SqlClient;
using System.Windows;

namespace QlySanBong.data_provier
{
    class FootballFieldDP : DataProvier
    {
        private static FootballFieldDP instance;
        public static FootballFieldDP Instance
        {
            get
            {
                if (instance == null)
                    instance = new FootballFieldDP();
                return FootballFieldDP.instance;
            }
            private set
            {
                FootballFieldDP.instance = value;
            }
        }
        FootballFieldDP() { }
        public List<FootballField> ConvertDBToList()
        {
            DataTable dataTable = new DataTable();
            List<FootballField> footballFields = new List<FootballField>();
            try
            {
                OpenConnection();
                string queryString = @"Select * from FootballField Where IsDeleted=0";
                SqlCommand command = new SqlCommand(queryString, connect);
                command.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    int idField = int.Parse(dataTable.Rows[i].ItemArray[0].ToString());
                    string name = dataTable.Rows[i].ItemArray[1].ToString();
                    int type = int.Parse(dataTable.Rows[i].ItemArray[2].ToString());
                    int status = int.Parse(dataTable.Rows[i].ItemArray[3].ToString());
                    string note = dataTable.Rows[i].ItemArray[4].ToString();
                    int isDeleted = int.Parse(dataTable.Rows[i].ItemArray[5].ToString());
                    FootballField footballField = new FootballField(idField, name, type, status, note, isDeleted);
                    footballFields.Add(footballField);
                }
            }
            catch { }
            finally
            {
                CloseConnection();
            }
            return footballFields;
        }
        public bool AddIntoDB(FootballField footballField)
        {
            try
            {
                OpenConnection();
                string query = "insert into FootballField(IdField, Name, Type, Status, Note, IsDeleted) values (@IdField, @Name, @Type, @Status, @Note, @Isdeleted)";
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@IdField", footballField.IdField.ToString());
                command.Parameters.AddWithValue("@Name", footballField.Name);
                command.Parameters.AddWithValue("@Type", footballField.Type.ToString());
                command.Parameters.AddWithValue("@Status", footballField.Status.ToString());
                command.Parameters.AddWithValue("@Note", footballField.Note);
                command.Parameters.AddWithValue("@IsDeleted", footballField.IsDeleted);
                int rs = command.ExecuteNonQuery();
                if (rs != 1)
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
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
        public bool DeleteField(string idField)
        {
            try
            {
                OpenConnection();
                string query = @"Update FootballField Set IsDeleted = 1 Where IdField = " + idField;
                SqlCommand command = new SqlCommand(query, connect);
                int rs = command.ExecuteNonQuery();
                if (rs == 1)
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
        public bool UpdateField(FootballField footballField)
        {
            try
            {
                OpenConnection();
                string query = @"update FootballField set IdField = @idField, Name = @name, Type = @type, Status = @status, IsDeleted = @isDeleted
                                where IdField = " + footballField.IdField.ToString();
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@idField", footballField.IdField.ToString());
                command.Parameters.AddWithValue("@name", footballField.Name);
                command.Parameters.AddWithValue("@type", footballField.Type.ToString());
                command.Parameters.AddWithValue("@status", footballField.Status.ToString());
                command.Parameters.AddWithValue("@isDeleted", footballField.IsDeleted);
                int rs = command.ExecuteNonQuery();
                if (rs == 1)
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
        public List<string> GetFieldType()
        {
            try
            {
                OpenConnection();
                string query = @"select distinct(Type) from FootballField where IsDeleted=0 order by type ASC";
                SqlCommand cmd = new SqlCommand(query, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                List<string> listTmp = new List<string>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    listTmp.Add(dataTable.Rows[i].ItemArray[0].ToString());
                }
                return listTmp;
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
        public bool isExistFieldName(string fieldName)
        {
            try
            {
                OpenConnection();
                string query = @"select * from FootballField where IsDeleted=0 and Name = '" + fieldName + "'";
                SqlCommand cmd = new SqlCommand(query, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                if (dataTable.Rows.Count == 0)
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
        public FootballField GetFootballFieldById(string idField)
        {
            try
            {
                OpenConnection();
                string queryString = "select * from FootballField where IdField = " + idField;

                SqlCommand command = new SqlCommand(queryString, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                FootballField res = new FootballField(int.Parse(idField), dataTable.Rows[0].ItemArray[1].ToString(),
                    int.Parse(dataTable.Rows[0].ItemArray[2].ToString()), int.Parse(dataTable.Rows[0].ItemArray[3].ToString()),
                    dataTable.Rows[0].ItemArray[4].ToString(), int.Parse(dataTable.Rows[0].ItemArray[5].ToString()));
                return res;
            }
            catch
            {
                return new FootballField();
            }
            finally
            {
                CloseConnection();
            }
        }
        public List<FootballField> GetGoodFields()
        {
            List<FootballField> footballFields = new List<FootballField>();
            try
            {
                OpenConnection();
                string query = @"select * from FootballField where isDeleted=0 and status=1 order by idField ASC";
                SqlCommand cmd = new SqlCommand(query, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    FootballField footballField = new FootballField(int.Parse(dataTable.Rows[i].ItemArray[0].ToString()),
                                                  dataTable.Rows[i].ItemArray[1].ToString(), int.Parse(dataTable.Rows[i].ItemArray[2].ToString()),
                                                  int.Parse(dataTable.Rows[i].ItemArray[3].ToString()), dataTable.Rows[i].ItemArray[4].ToString(),
                                                  int.Parse(dataTable.Rows[i].ItemArray[5].ToString()));
                    footballFields.Add(footballField);
                }
            }
            catch
            {

            }
            finally
            {
                CloseConnection();
            }
            return footballFields;
        }
        public List<FootballField> GetEmptyField(string type, string day, string startTime, string endTime)
        {
            List<FootballField> footballFields = new List<FootballField>();
            try
            {
                OpenConnection();
                string query = @"Select IdField,Name from FootballField
                                 Where FootballField.Type=@type
                                 Except
                                 Select FieldInfo.IdField,FootballField.Name from FieldInfo
                                 Join FootballField on FieldInfo.IdField=FootballField.IdField
                                 Where convert(varchar(10), StartingTime, 103)=@day and convert(varchar(5), StartingTime, 108)=@startTime and convert(varchar(5), EndingTime, 108) =@endTime and FootballField.Type=@type and FootballField.IsDeleted=0";
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@day", day);
                command.Parameters.AddWithValue("@startTime", startTime);
                command.Parameters.AddWithValue("@endTime", endTime);
                command.Parameters.AddWithValue("@type", type);
                command.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    FootballField footballField = new FootballField(int.Parse(dataTable.Rows[i].ItemArray[0].ToString()), dataTable.Rows[i].ItemArray[1].ToString(),
                                                                    int.Parse(type), 0, " ", 0);
                    footballFields.Add(footballField);
                }
            }
            catch
            {

            }
            finally
            {
                CloseConnection();
            }
            return footballFields;
        }

        public int GetMaxIdFootballFiled()
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = "select max(IdField) from FootballField ";

                SqlCommand command = new SqlCommand(queryString, connect);
                command.ExecuteNonQuery();
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






