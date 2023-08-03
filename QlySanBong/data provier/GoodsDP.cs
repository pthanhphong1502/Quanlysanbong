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
     class GoodsDP: DataProvier
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
        public List<Goods> ConverDBtoList()
        {
            List<Goods> goodsList = new List<Goods>();
            try
            {
                OpenConnection();
                string queryString = "select * from Goods where IsDeleted = 0";

                SqlCommand command = new SqlCommand(queryString, connect);
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

        public bool DeleteFromDB(string idGoods)
        {
            try
            {
                OpenConnection();
                string queryString = "update Goods set IsDeleted = 1 where idGoods = " + idGoods;
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

        public int GetMaxId()
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = "select max(idGoods) as id from Goods";
                SqlCommand command = new SqlCommand(queryString, connect);

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
        public bool UpdateOnDB(Goods goods)
        {
            try
            {
                OpenConnection();
                string queryString = "update Goods set Name=@Name, Unit=@Unit, UnitPrice=@UnitPrice, ImageFile=@ImageFile " +
                    "where idGoods =" + goods.IdGoods.ToString();
                SqlCommand command = new SqlCommand(queryString, connect);
                command.Parameters.AddWithValue("@Name", goods.Name);
                command.Parameters.AddWithValue("@Unit", goods.Unit);
                command.Parameters.AddWithValue("@UnitPrice", goods.UnitPrice.ToString());
                command.Parameters.AddWithValue("@ImageFile", Convert.ToBase64String(goods.ImageFile));
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

                SqlCommand command = new SqlCommand(queryString, connect);
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
        public bool AddIntoDB(Goods goods)
        {

            try
            {
                OpenConnection();
                string queryString = "insert into Goods(IdGoods, Name, Quantity, UnitPrice, Unit, ImageFile, IsDeleted) " +
                    "values(@IdGoods, @Name, @Quantity, @UnitPrice, @Unit, @ImageFile,  @IsDeleted)";
                SqlCommand command = new SqlCommand(queryString, connect);
                command.Parameters.AddWithValue("@IdGoods", goods.IdGoods.ToString());
                command.Parameters.AddWithValue("@Name", goods.Name);
                command.Parameters.AddWithValue("@Quantity", goods.Quantity.ToString());
                command.Parameters.AddWithValue("@UnitPrice", goods.UnitPrice.ToString());
                command.Parameters.AddWithValue("@Unit", goods.Unit);
                command.Parameters.AddWithValue("@ImageFile", Convert.ToBase64String(goods.ImageFile));
                command.Parameters.AddWithValue("@IsDeleted", goods.IsDeleted.ToString());
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
                string queryString = "update Goods set quantity = quantity + @quantity where idGoods=" + goods.IdGoods.ToString();
                SqlCommand command = new SqlCommand(queryString, connect);
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
        public bool IsExistGoodsName(string goodsName)
        {
            try
            {
                OpenConnection();
                string query = @"select * from Goods where IsDeleted = 0 and Name = '" + goodsName + "'";
                SqlCommand cmd = new SqlCommand(query, connect);
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
