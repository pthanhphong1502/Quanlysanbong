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
    class SalaryDP:DataProvier
    {
        private static SalaryDP instance;

        public static SalaryDP Instance
        {
            get { if (instance == null) instance = new SalaryDP(); return SalaryDP.instance; }
            private set { SalaryDP.instance = value; }
        }
        public SalaryDP()
        {

        }
        public List<Salary> GetSalaryByMonth(string month, string year)
        {
            try
            {
                List<Salary> salaries = new List<Salary>();
                OpenConnection();
                string query = @"select * from Salary where month(SalaryMonth) = @Month and year(SalaryMonth) = @Year";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int idSalaryRecord = -1;
                    if (dt.Rows[i].ItemArray[5].ToString() != "")
                    {
                        idSalaryRecord = int.Parse(dt.Rows[i].ItemArray[5].ToString());
                    }
                    Salary salary = new Salary(int.Parse(dt.Rows[i].ItemArray[0].ToString()), int.Parse(dt.Rows[i].ItemArray[1].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[2].ToString()), long.Parse(dt.Rows[i].ItemArray[3].ToString())
                        , DateTime.Parse(dt.Rows[i].ItemArray[4].ToString()), idSalaryRecord);
                    salaries.Add(salary);
                }
                return salaries;
            }
            catch
            {
                return new List<Salary>();
            }
            finally
            {
                CloseConnection();
            }
        }
        public List<Salary> GetPaidSalary(string month, string year)
        {
            try
            {
                List<Salary> salaries = new List<Salary>();
                OpenConnection();
                string query = @"select * from Salary where month(SalaryMonth) = @Month and year(SalaryMonth) = @Year and IdSalaryRecord is not null";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int idSalaryRecord = -1;
                    if (dt.Rows[i].ItemArray[5].ToString() != "")
                    {
                        idSalaryRecord = int.Parse(dt.Rows[i].ItemArray[5].ToString());
                    }
                    Salary salary = new Salary(int.Parse(dt.Rows[i].ItemArray[0].ToString()), int.Parse(dt.Rows[i].ItemArray[1].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[2].ToString()), long.Parse(dt.Rows[i].ItemArray[3].ToString())
                        , DateTime.Parse(dt.Rows[i].ItemArray[4].ToString()), idSalaryRecord);
                    salaries.Add(salary);
                }
                return salaries;
            }
            catch
            {
                return new List<Salary>();
            }
            finally
            {
                CloseConnection();
            }
        }
        public List<Salary> GetUnPaidSalary(string idEmployee, string month, string year)
        {
            List<Salary> salaries = new List<Salary>();
            try
            {
                OpenConnection();
                string query = @"select* from Salary where month(SalaryMonth) = @Month and year(SalaryMonth) = @Year and IdEmployee =@IdEmployee and IdSalaryRecord is null";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@IdEmployee", idEmployee);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int idSalaryRecord = -1;
                    if (dt.Rows[i].ItemArray[5].ToString() != "")
                    {
                        idSalaryRecord = int.Parse(dt.Rows[i].ItemArray[5].ToString());
                    }
                    Salary salary = new Salary(int.Parse(dt.Rows[i].ItemArray[0].ToString()), int.Parse(dt.Rows[i].ItemArray[1].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[2].ToString()), long.Parse(dt.Rows[i].ItemArray[3].ToString())
                        , DateTime.Parse(dt.Rows[i].ItemArray[4].ToString()), idSalaryRecord);
                    salaries.Add(salary);
                }
            }
            catch
            {
                MessageBox.Show("Lỗi!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                CloseConnection();
            }
            return salaries;
        }

        public bool AddIntoDB(Salary salary)
        {
            try
            {
                OpenConnection();
                string query = @"insert into Salary (IdEmployee, NumOfShift, NumOfFault,TotalSalary, SalaryMonth) values(@IdEmployee, @NumOfShift, @NumOfFault,@TotalSalary, @SalaryMonth)";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@IdEmployee", salary.IdEmployee.ToString());
                cmd.Parameters.AddWithValue("@NumOfShift", salary.NumOfShift.ToString());
                cmd.Parameters.AddWithValue("@NumOfFault", salary.NumOfFault.ToString());
                cmd.Parameters.AddWithValue("@TotalSalary", salary.TotalSalary.ToString());
                cmd.Parameters.AddWithValue("@SalaryMonth", salary.SalaryMonth);
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

        //update ngày trả lương và người trả lương
        public bool UpdateIdSalaryRecord(int id, int month, int year)
        {
            try
            {
                OpenConnection();
                string query = "update Salary set IdSalaryRecord = @IdSalaryRecord where month(SalaryMonth) = @Month and year(SalaryMonth) = @Year "
                                + "and IdEmployee in (select IdEmployee from Employee where IsDeleted = 0)";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@IdSalaryRecord", id.ToString());
                cmd.Parameters.AddWithValue("@Month", month.ToString());
                cmd.Parameters.AddWithValue("@Year", year.ToString());
                int rs = cmd.ExecuteNonQuery();
                if (rs < 1)
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
        public bool UpdateQuantity(Salary salary)
        {
            try
            {
                OpenConnection();
                string query = "update Salary  set NumOfShift=@NumOfShift,NumOfFault=@NumOfFault where IdEmployee= @IdEmployee and month(SalaryMonth) = @Month and year(SalaryMonth) = @Year";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@NumOfShift", salary.NumOfShift.ToString());
                cmd.Parameters.AddWithValue("@NumOfFault", salary.NumOfFault.ToString());
                cmd.Parameters.AddWithValue("@IdEmployee", salary.IdEmployee.ToString());
                cmd.Parameters.AddWithValue("@Month", salary.SalaryMonth.Month.ToString());
                cmd.Parameters.AddWithValue("@Year", salary.SalaryMonth.Year.ToString());
                int rs = cmd.ExecuteNonQuery();
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
        public bool UpdateTotalSalary(Salary salary)
        {
            try
            {
                OpenConnection();
                string query = "update Salary  set TotalSalary=@TotalSalary where IdEmployee=@IdEmployee and month(SalaryMonth) = @Month and year(SalaryMonth) = @Year";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@TotalSalary", salary.TotalSalary.ToString());
                cmd.Parameters.AddWithValue("@IdEmployee", salary.IdEmployee.ToString());
                cmd.Parameters.AddWithValue("@Month", salary.SalaryMonth.Month.ToString());
                cmd.Parameters.AddWithValue("@Year", salary.SalaryMonth.Year.ToString());
                int rs = cmd.ExecuteNonQuery();
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
        public bool IsExist(string idEmployee, DateTime date)
        {
            try
            {
                OpenConnection();
                string query = @"select * from Salary where IdEmployee = @IdEmployee and  month(SalaryMonth) = @month and year(SalaryMonth) = @Year";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@IdEmployee", idEmployee.ToString());
                cmd.Parameters.AddWithValue("@Month", date.Month.ToString());
                cmd.Parameters.AddWithValue("@Year", date.Year.ToString());
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count < 1)
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
                MessageBox.Show("Lỗi!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
        public bool IsExistIdSalaryRecord(string month, string year)
        {
            try
            {
                OpenConnection();
                string query = @"select distinct(IdSalaryRecord) from Salary where month(SalaryMonth) = @Month and year(SalaryMonth) = @Year and IdSalaryRecord is not null";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count < 1)
                    return false;
                else
                {
                    return true;
                }
            }
            catch
            {
                MessageBox.Show("Lỗi!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public long GetTotalSalary(string idEmployee, string month, string year)
        {
            try
            {
                OpenConnection();
                string query = "select TotalSalary from Salary where IdEmployee = @IdEmployee and month(SalaryMonth) = @Month and year(SalaryMonth) = @Year";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@IdEmployee", idEmployee);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows[0].ItemArray[0].ToString() != "")
                {
                    return long.Parse(dt.Rows[0].ItemArray[0].ToString());
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
        public List<Salary> GetSalaryOfEmployee(string month, string year) // Lấy lương của những nhân viên đang làm việc
        {
            List<Salary> salaries = new List<Salary>();
            try
            {
                OpenConnection();
                string query = "select Salary.IdEmployee, Salary.NumOfShift, Salary.NumOfFault, Salary.TotalSalary , Salary.SalaryMonth, Salary.IdSalaryRecord from Salary " +
                                "join Employee on Salary.IdEmployee = Employee.IdEmployee" +
                                 " where Employee.IsDeleted = 0 and month(Salary.SalaryMonth) = @Month and year(Salary.SalaryMonth) =@Year";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int idSalaryRecord = -1;
                    if (dt.Rows[i].ItemArray[5].ToString() != "")
                    {
                        idSalaryRecord = int.Parse(dt.Rows[i].ItemArray[5].ToString());
                    }
                    Salary salary = new Salary(int.Parse(dt.Rows[i].ItemArray[0].ToString()), int.Parse(dt.Rows[i].ItemArray[1].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[2].ToString()), long.Parse(dt.Rows[i].ItemArray[3].ToString())
                        , DateTime.Parse(dt.Rows[i].ItemArray[4].ToString()), idSalaryRecord);
                    salaries.Add(salary);
                }
            }
            catch
            {
                MessageBox.Show("Lỗi!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                CloseConnection();
            }
            return salaries;
        }

        public List<Salary> GetSalaryInfoById(string idSalaryRecord)
        {
            List<Salary> listSalaryInfo = new List<Salary>();
            try
            {
                OpenConnection();
                string queryString = "select * from Salary where IdSalaryRecord = " + idSalaryRecord;
                SqlCommand command = new SqlCommand(queryString, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Salary salary = new Salary(int.Parse(dataTable.Rows[i].ItemArray[0].ToString()), int.Parse(dataTable.Rows[i].ItemArray[1].ToString()),
                        int.Parse(dataTable.Rows[i].ItemArray[2].ToString()), long.Parse(dataTable.Rows[i].ItemArray[3].ToString()),
                        DateTime.Parse(dataTable.Rows[i].ItemArray[4].ToString()), int.Parse(dataTable.Rows[i].ItemArray[5].ToString()));
                    listSalaryInfo.Add(salary);
                }
                return listSalaryInfo;
            }
            catch
            {
                return new List<Salary>();
            }
            finally
            {
                CloseConnection();
            }
        }

      
    }
}
