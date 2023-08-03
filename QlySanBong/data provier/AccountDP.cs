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
    class AccountDP: DataProvier
    {
        private static AccountDP instance;

        public static AccountDP Instance
        {
            get { if (instance == null) instance = new AccountDP(); return AccountDP.instance; }
            private set { AccountDP.instance = value; }
        }
        public List<Account> ConvertDBToList()
        {
            DataTable dt;
            List<Account> accounts = new List<Account>();
            try
            {
                dt = LoadData("Account");
            }
            catch
            {
                CloseConnection();
                dt = LoadData("Account");
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Account acc = new Account(int.Parse(dt.Rows[i].ItemArray[0].ToString()), dt.Rows[i].ItemArray[1].ToString(), dt.Rows[i].ItemArray[2].ToString(), int.Parse(dt.Rows[i].ItemArray[3].ToString()));
                accounts.Add(acc);
            }
            return accounts;
        }

        public bool DeleteAcount(string idAcount)
        {
            try
            {
                OpenConnection();
                string query ="Delete from Acount where IdAcount = "+ idAcount;
                SqlCommand command = new SqlCommand(query, connect);
                if (command.ExecuteNonQuery() > 0)
                    return true;
                else return false;
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

        public bool UpdateType(Account account)
        {
            try
            {
                OpenConnection();
                string query = "update Account set type=@type where IdAccount = " + account.IdAccount;
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@type  ", account.Type);
                if (command.ExecuteNonQuery() > 0)
                    return true;
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

        public bool UpdatePasswordByUsername(string username, string password)
        {
            try
            {
                OpenConnection();
                string query = "update Account set password=@password where username = @username";
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
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

        public bool IsExistIdUserName(int Idusername)
        {
            try
            {
                OpenConnection();
                string queryString = "select * from Account where IdAccount = '" + Idusername + "'";
                SqlCommand command = new SqlCommand(queryString, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
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

        public bool IsExistUserName(string username)
        {
            try
            {
                OpenConnection();
                string queryString = "select * from Account where username = '" + username + "'";
                SqlCommand command = new SqlCommand(queryString, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
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

        public int SetNewID()
        {
            try
            {
                OpenConnection();
                string queryString = "select max(idAccount) from Account";
                SqlCommand command = new SqlCommand(queryString, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return int.Parse(dataTable.Rows[0].ItemArray[0].ToString()) + 1;
                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                return -1;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool AddAccount(Account account)
        {
            try
            {
                OpenConnection();
                string query = "insert into Account(IdAccount,Username,Password,Type) Values(@IdAccount,@Username,@Password,@Type)";
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@IdAccount", account.IdAccount);
                command.Parameters.AddWithValue("@Username", account.Username);
                command.Parameters.AddWithValue("@Password", account.Password);
                command.Parameters.AddWithValue("@Type", account.Type);
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
    }
}
