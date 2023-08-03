using QlySanBong.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QlySanBong.data_provier
{
    class GoodsDP : DataProvier
    {
        private static GoodsDP instance;

        public static GoodsDP Instance
        {
            get { if (instance == null) instance = new GoodsDP(); return GoodsDP.instance; }
            private set { GoodsDP.instance = value; }
        }
        private GoodsDP()
        {

        }
        public List<Goods> ConvertDBToList()
        {
            List<Goods> goodsList = new List<Goods>();
            try
            {
                OpenConnection();
                string queryString = "select * from Goods where IsDeleted = 0";

                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Goods acc = new Goods(int.Parse(dt.Rows[i].ItemArray[0].ToString()), dt.Rows[i].ItemArray[1].ToString(),
                        dt.Rows[i].ItemArray[2].ToString(), long.Parse(dt.Rows[i].ItemArray[3].ToString()),
                        Convert.FromBase64String(dt.Rows[i].ItemArray[4].ToString()), int.Parse(dt.Rows[i].ItemArray[5].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[6].ToString()));
                    goodsList.Add(acc);
                }
                return goodsList;
            }
            catch
            {
                return goodsList;
            }
            finally
            {
                CloseConnection();
            }
        }
        public DataTable LoadDatatable()
        {
            try
            {
                OpenConnection();
                string queryString = "select * from Goods where IsDeleted = 0";

                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch
            {
                return new DataTable();
            }
            finally
            {
                CloseConnection();
            }
        }
        public bool AddIntoDB(Goods goods)
        {
            try
            {
                OpenConnection();
                string queryString = "insert into Goods(IdGoods, Name, Unit, UnitPrice, ImageFile, Quantity, IsDeleted) " +
                    "values(@idGoods, @name, @unit, @unitPrice, @imageFile, @quantity, @isDeleted)";
                SqlCommand command = new SqlCommand(queryString, conn);
                command.Parameters.AddWithValue("@idGoods", goods.IdGoods.ToString());
                command.Parameters.AddWithValue("@name", goods.Name);
                command.Parameters.AddWithValue("@unit", goods.Unit);
                command.Parameters.AddWithValue("@unitPrice", goods.UnitPrice.ToString());
                command.Parameters.AddWithValue("@imageFile", Convert.ToBase64String(goods.ImageFile));
                command.Parameters.AddWithValue("@quantity", goods.Quantity.ToString());
                command.Parameters.AddWithValue("@isDeleted", goods.IsDeleted.ToString());

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
        public bool UpdateOnDB(Goods goods)
        {
            try
            {
                OpenConnection();
                string queryString = "update Goods set Name=@name, Unit=@unit, UnitPrice=@unitPrice, ImageFile=@imageFile " +
                    "where IdGoods =" + goods.IdGoods.ToString();
                SqlCommand command = new SqlCommand(queryString, conn);
                command.Parameters.AddWithValue("@name", goods.Name);
                command.Parameters.AddWithValue("@unit", goods.Unit);
                command.Parameters.AddWithValue("@unitPrice", goods.UnitPrice.ToString());
                command.Parameters.AddWithValue("@imageFile", Convert.ToBase64String(goods.ImageFile));

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
        public bool UpdateQuantity(int idGoods, int quantity)
        {
            try
            {
                OpenConnection();
                string queryString = "update Goods set Quantity=@quantity where IdGoods = " + idGoods.ToString();
                SqlCommand command = new SqlCommand(queryString, conn);
                command.Parameters.AddWithValue("@quantity", quantity.ToString());
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
        public bool ImportToDB(Goods goods)
        {
            try
            {
                OpenConnection();
                string queryString = "update Goods set Quantity = Quantity + @quantity where IdGoods=" + goods.IdGoods.ToString();
                SqlCommand command = new SqlCommand(queryString, conn);
                command.Parameters.AddWithValue("@quantity", goods.Quantity.ToString());
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
        public bool DeleteFromDB(string idGoods)
        {
            try
            {
                OpenConnection();
                string queryString = "update Goods set IsDeleted = 1 where IdGoods = " + idGoods;
                SqlCommand command = new SqlCommand(queryString, conn);
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
        public Goods GetGoods(string idGoods) // lấy thông tin hàng hóa khi biết id 
        {
            try
            {
                OpenConnection();
                string queryString = "select * from Goods where IdGoods = " + idGoods;

                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                Goods res = new Goods(int.Parse(idGoods), dataTable.Rows[0].ItemArray[1].ToString(),
                    dataTable.Rows[0].ItemArray[2].ToString(), long.Parse(dataTable.Rows[0].ItemArray[3].ToString()),
                    Convert.FromBase64String(dataTable.Rows[0].ItemArray[4].ToString()), int.Parse(dataTable.Rows[0].ItemArray[5].ToString()),
                    int.Parse(dataTable.Rows[0].ItemArray[6].ToString()));

                return res;
            }
            catch
            {
                return new Goods();
            }
            finally
            {
                CloseConnection();
            }
        }
        public int GetMaxId()
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = "select max(IdGoods) as id from Goods";
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataReader rdr = command.ExecuteReader();
                rdr.Read();
                res = int.Parse(rdr["id"].ToString());
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
        public bool IsExistGoodsName(string goodsName)
        {
            try
            {
                OpenConnection();
                string query = @"select * from Goods where IsDeleted = 0 and Name = '" + goodsName + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count == 0)
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
    }
}

