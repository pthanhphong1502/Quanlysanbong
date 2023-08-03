using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;

namespace QlySanBong.data_provier
{
    public class SQLConnection
    {
        private string strConn;
        public SqlConnection connect;
        public SQLConnection()
        {
            try
            {
                strConn = ConfigurationManager.ConnectionStrings["QSB"].ConnectionString;
            }
            catch
            {
                MessageBox.Show("Mất kết nối đến cơ sở dữ liệu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            connect = new SqlConnection(strConn);
        }
        public void OpenConnection()
        {
            try
            {
                if (connect.State != System.Data.ConnectionState.Open)
                {
                    connect.ConnectionString = ConfigurationManager.ConnectionStrings["QSB"].ConnectionString;
                    connect.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mất kết nối đến cơ sở dữ liệu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                throw ex;
            }
        }
        public void CloseConnection()
        {
            connect.Close();
        }
    }
}
