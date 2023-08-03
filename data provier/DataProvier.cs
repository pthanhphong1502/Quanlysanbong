﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QlySanBong.data_provier
{
     public class DataProvier :SQLConnection
    {
        public DataTable LoadData(string tableName)
        {
            DataTable dt = new DataTable();
            CloseConnection();
            try
            {
                OpenConnection();
                string sql = "SELECT * FROM " + tableName;
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                CloseConnection();
                return dt;
            }
            catch
            {
                MessageBox.Show("Mất kết nối đến cơ sở dữ liệu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                App.Current.Shutdown();

            }
            return dt;
        }
    }
}
