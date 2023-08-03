using QlySanBong.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;

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
        public bool IsExistUserName(string username)
        {
            try
            {
                OpenConnection();
                string queryString = "select * from Account where username = '" + username + "'";
                SqlCommand command = new SqlCommand(queryString, conn);
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
    }
}
