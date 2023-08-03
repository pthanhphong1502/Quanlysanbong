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
    class BillDP: DataProvier
    {
        private static BillDP instance;

        public static BillDP Instance
        {
            get { if (instance == null) instance = new BillDP(); return BillDP.instance; }
            private set { BillDP.instance = value; }
        }


        public bool AddIntoDB(BillInfo billInfo)
        {
            try
            {
                OpenConnection();
                string queryString = "Insert into BillInfo(IdBill,IdGoods,Name,ImportDay,NameGoods,PriceGoods,Quanity,Total) Values(@IdBill, @IdGoods,@Name,@ImportDay,@NameGoods,@PriceGoods,@Quantity,@Total)";
                SqlCommand command = new SqlCommand(queryString, connect);
                command.Parameters.AddWithValue("@IdBill", billInfo.IdBill);
                command.Parameters.AddWithValue("@IdGoods", billInfo.IdGoods);
                command.Parameters.AddWithValue("@Name", billInfo.Username);
                command.Parameters.AddWithValue("@ImportDay", billInfo.ImportDay);
                command.Parameters.AddWithValue("@NameGoods", billInfo.NameGoods);
                command.Parameters.AddWithValue("@PriceGoods", billInfo.Pricegoods);
                command.Parameters.AddWithValue("@Quantity", billInfo.Quantity);
                command.Parameters.AddWithValue("@Total", billInfo.Total);
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
                MessageBox.Show("Đã tồn tại mặt hàng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public int GetMaxIdBill()
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = @"Select max(idBill) From Bill";
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
