using QlySanBong.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlySanBong.data_provier
{
    class SalarySettingDP:DataProvier
    {
        private static SalarySettingDP instance;

        public static SalarySettingDP Instance
        {
            get
            {
                if (instance == null)
                    return new SalarySettingDP();
                return instance;
            }
            private set { instance = value; }
        }
        public List<SalarySetting> ConvertDBToList()
        {
            DataTable data;
            List<SalarySetting> tmp = new List<SalarySetting>();
            try
            {

                data = LoadData("SalarySetting");
            }
            catch
            {
                CloseConnection();
                data = LoadData("SalarySetting");
            }
            for (int i = 0; i < data.Rows.Count; i++)
            {
                SalarySetting newItem = new SalarySetting(long.Parse(data.Rows[i].ItemArray[0].ToString()), long.Parse(data.Rows[i].ItemArray[1].ToString()), long.Parse(data.Rows[i].ItemArray[2].ToString()), data.Rows[i].ItemArray[3].ToString(), int.Parse(data.Rows[i].ItemArray[4].ToString()));
                tmp.Add(newItem);
            }
            return tmp;
        }

        public bool AddIntoDB(SalarySetting salarySetting)
        {
            try
            {
                OpenConnection();
                string query = @"insert into SalarySetting (salaryBase, moneyPerShift, moneyPerFault, typeEmployee, standardWorkDays) " +
                                "values(@salaryBase, @moneyPerShift, @moneyPerFault, @typeEmployee, @standardWorkDays)";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@salaryBase", salarySetting.SalaryBase.ToString());
                cmd.Parameters.AddWithValue("@moneyPerShift", salarySetting.MoneyPerShift.ToString());
                cmd.Parameters.AddWithValue("@moneyPerFault", salarySetting.MoneyPerFault.ToString());
                cmd.Parameters.AddWithValue("@typeEmployee", salarySetting.TypeEmployee);
                cmd.Parameters.AddWithValue("@standardWorkDays", salarySetting.StandardWorkDays.ToString());
                if (cmd.ExecuteNonQuery() < 1)
                    return false;
                else
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

        public bool UpdateDB(SalarySetting salarySetting)
        {
            try
            {
                OpenConnection();
                string query = @"update SalarySetting set salaryBase = @salaryBase,moneyPerShift = @moneyPerShift,moneyPerFault = @moneyPerFault, standardWorkDays = @standardWorkDays where typeEmployee = N'" + salarySetting.TypeEmployee + "'";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@salaryBase", salarySetting.SalaryBase.ToString());
                cmd.Parameters.AddWithValue("@moneyPerShift", salarySetting.MoneyPerShift.ToString());
                cmd.Parameters.AddWithValue("@moneyPerFault", salarySetting.MoneyPerFault.ToString());
                cmd.Parameters.AddWithValue("@standardWorkDays", salarySetting.StandardWorkDays.ToString());
                if (cmd.ExecuteNonQuery() < 1)
                    return false;
                else
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

        public SalarySetting GetSalarySettings(string typeEmployee) // lấy ra nhân viên có chức vụ 
        {
            try
            {
                OpenConnection();
                string query = @"select * from SalarySetting where typeEmployee = N'" + typeEmployee + "'";
                SqlCommand cmd = new SqlCommand(query, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable data = new DataTable();
                adapter.Fill(data);
                if (data.Rows.Count > 0)
                {
                    SalarySetting newItem = new SalarySetting(long.Parse(data.Rows[0].ItemArray[0].ToString()),
                   long.Parse(data.Rows[0].ItemArray[1].ToString()), long.Parse(data.Rows[0].ItemArray[2].ToString()),
                   data.Rows[0].ItemArray[3].ToString(), int.Parse(data.Rows[0].ItemArray[4].ToString()));
                    return newItem;
                }
                else
                {
                    return null;
                }
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
        public string GetBaseSalary(string typeEmployee)
        {
            string res = "0";
            try
            {
                OpenConnection();
                string query = @"select salaryBase from SalarySetting where typeEmployee = N'" + typeEmployee + "'";
                SqlCommand cmd = new SqlCommand(query, connect);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable data = new DataTable();
                adapter.Fill(data);
                if (data.Rows.Count > 0)
                {
                    res = data.Rows[0].ItemArray[0].ToString();
                }
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
