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
    class StockReceiptInfoDP : DataProvier
    {
        private static StockReceiptInfoDP instance;

        public static StockReceiptInfoDP Instance
        {
            get { if (instance == null) instance = new StockReceiptInfoDP(); return StockReceiptInfoDP.instance; }
            private set { StockReceiptInfoDP.instance = value; }
        }
        public bool AddIntoDB(StockReceiptInfo stockReceiptInfo)
        {
            try
            {
                OpenConnection();
                string queryString = "insert into StockReceiptInfo(IdStockReceipt, IdGoods, Quantity, ImportPrice) " +
                    "values(@IdStockReceipt, @IdGoods, @Quantity, @ImportPrice )";
                SqlCommand command = new SqlCommand(queryString, connect);
                command.Parameters.AddWithValue("@IdStockReceipt", stockReceiptInfo.IdStockReceipt.ToString());
                command.Parameters.AddWithValue("@IdGoods", stockReceiptInfo.IdGoods.ToString());
                command.Parameters.AddWithValue("@Quantity", stockReceiptInfo.Quantity.ToString());
                command.Parameters.AddWithValue("@ImportPrice", stockReceiptInfo.ImportPrice.ToString());

                int rs = command.ExecuteNonQuery();
                if (rs != 1)
                {
                    throw new Exception();
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
        public bool DeleteFromDB(string idGoods)
        {
            try
            {
                OpenConnection();
                string queryString = "delete from StockReceiptInfo where IdGoods=" + idGoods;
                SqlCommand command = new SqlCommand(queryString, connect);
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
        public bool DeleteByIdStock(string idGoods, string idStockReceipt)
        {
            try
            {
                OpenConnection();
                string queryString = string.Format("delete from StockReceiptInfo where idGoods = {0} and IdStockReceipt = {1}", idGoods, idStockReceipt);
                SqlCommand command = new SqlCommand(queryString, connect);
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
        public bool DeleteByIdStockReceipt(string idStockReceipt)
        {
            try
            {
                OpenConnection();
                string queryString = "delete from StockReceiptInfo where IdStockReceipt = " + idStockReceipt;
                SqlCommand command = new SqlCommand(queryString, connect);
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
        public bool UpdateOnDB(StockReceiptInfo stockReceiptInfo)
        {
            try
            {
                OpenConnection();
                string queryString = "update StockReceiptInfo set quantity=@quantity, importPrice=@importPrice " +
                    "where idGoods=@idGoods and idStockReceipt=@idStockReceipt";
                SqlCommand command = new SqlCommand(queryString, connect);
                command.Parameters.AddWithValue("@idGoods", stockReceiptInfo.IdGoods.ToString());
                command.Parameters.AddWithValue("@idStockReceipt", stockReceiptInfo.IdStockReceipt.ToString());
                command.Parameters.AddWithValue("@quantity", stockReceiptInfo.Quantity.ToString());
                command.Parameters.AddWithValue("@importPrice", stockReceiptInfo.ImportPrice.ToString());
                command.ExecuteNonQuery();
                return true;
            }
            catch
            {
                MessageBox.Show("Thực hiện thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
        public List<string> QueryIdStockReceipt(string idGoods)
        {
            List<string> res = new List<string>();
            try
            {
                OpenConnection();
                string queryString = "select idStockReceipt from StockReceiptInfo where idGoods=" + idGoods;
                SqlCommand command = new SqlCommand(queryString, connect);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res.Add(rdr["idStockReceipt"].ToString());
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
        public long CalculateTotalMoney(string idStockReceipt)
        {
            long res = 0;
            try
            {
                OpenConnection();
                string queryString = string.Format("select sum(importPrice * quantity) as total from StockReceiptInfo " +
                    "where idStockReceipt = {0} group by idStockReceipt", idStockReceipt);
                SqlCommand command = new SqlCommand(queryString, connect);

                SqlDataReader rdr = command.ExecuteReader();
                rdr.Read();
                res = long.Parse(rdr["total"].ToString());
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
        public StockReceiptInfo GetStockReceiptInfoById(string idStockReceipt)
        {
            try
            {
                OpenConnection();
                string queryString = "select * from StockReceiptInfo where idStockReceipt = " + idStockReceipt;
                SqlCommand command = new SqlCommand(queryString, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    StockReceiptInfo stockReceiptInfo = new StockReceiptInfo(int.Parse(dataTable.Rows[0].ItemArray[0].ToString()),
                            int.Parse(dataTable.Rows[0].ItemArray[1].ToString()), int.Parse(dataTable.Rows[0].ItemArray[2].ToString()),
                            long.Parse(dataTable.Rows[0].ItemArray[3].ToString()));
                    return stockReceiptInfo;

                }
                return null;

            }
            catch
            {
                return new StockReceiptInfo();
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
