using QlySanBong.Model;
using QlySanBong.ResourceXAML;
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
    class EmployDP:DataProvier
    {
        private static EmployDP instance;

        public static EmployDP Instance
        {
            get { if (instance == null) instance = new EmployDP(); return EmployDP.instance; }
            private set { EmployDP.instance = value; }
        }

        private EmployDP()
        {

        }
        public int GetMaxIdEmployee()
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = "select max(IdEmployee) from Employee ";

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
        public List<Employee> ConvertDBToList()
        {
            DataTable dt = new DataTable();
            List<Employee> employees = new List<Employee>();
            try
            {
                OpenConnection();
                string queryString = @"Select * from Employee  Where IsDeleted = 0";
                SqlCommand command = new SqlCommand(queryString, connect);
                command.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(dt);
            }
            catch
            {

            }
            finally
            {
                CloseConnection();
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int idAccount = -1;
                if (dt.Rows[i].ItemArray[8].ToString() != "")
                {
                    idAccount = int.Parse(dt.Rows[i].ItemArray[8].ToString());
                }
                Employee employee = new Employee(int.Parse(dt.Rows[i].ItemArray[0].ToString()),
                    dt.Rows[i].ItemArray[1].ToString(), dt.Rows[i].ItemArray[2].ToString(),
                    dt.Rows[i].ItemArray[3].ToString(), dt.Rows[i].ItemArray[4].ToString(),
                    DateTime.Parse(dt.Rows[i].ItemArray[5].ToString()),
                    dt.Rows[i].ItemArray[6].ToString(), DateTime.Parse(dt.Rows[i].ItemArray[7].ToString()),
                    idAccount, Convert.FromBase64String(dt.Rows[i].ItemArray[9].ToString()), int.Parse(dt.Rows[i].ItemArray[10].ToString()));
                employees.Add(employee);
            }
            return employees;
        }
        public bool UpdateIdAccount(Employee employee)
        {
            try
            {
                OpenConnection();
                string query;
                if (employee.IdAccount != -1)
                    query = "Update Employee set IdAccount = " + employee.IdAccount + " where IdEmployee = @IdEmployee";
                else
                    query = "Update Employee set IdAccount = NULL where IdEmployee = @IdEmployee";

                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@idEmployee", employee.IdEmployee);

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
        public bool AddIntoDB(Employee employee)
        {
            try
            {
                OpenConnection();
                string query = "insert into Employee( IdEmployee,Name,Gender,Phonenumber,Address,DateofBirth,Position,Startingdate,ImageFile,IsDeleted) Values(@IdEmployee,@Name,@Gender,@Phonenumber,@Address,@DateofBirth,@Position,@Startingdate,@ImageFile,@IsDeleted)";
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@IdEmployee", employee.IdEmployee);
                command.Parameters.AddWithValue("@Name", employee.Name);
                command.Parameters.AddWithValue("@Gender", employee.Gender);
                command.Parameters.AddWithValue("@Phonenumber", employee.Phonenumber);
                command.Parameters.AddWithValue("@Address", employee.Address);
                command.Parameters.AddWithValue("@DateofBirth", employee.DateOfBirth);
                command.Parameters.AddWithValue("@Position", employee.Position);
                command.Parameters.AddWithValue("@Startingdate", employee.Startingdate);
                command.Parameters.AddWithValue("@ImageFile", Convert.ToBase64String(employee.ImageFile));
                command.Parameters.AddWithValue("@IsDeleted", employee.IsDeleted);
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
        public bool UpdateOnDB(Employee employee)
        {
            try
            {
                OpenConnection();
                string query = "update Employee  set Name=@Name,Gender=@Gender,Phonenumber=@Phonenumber,Address=@Address,DateofBirth=@DateofBirth,Position=@Position," +
                    "Startingdate=@Startingdate,ImageFile=@ImageFile,IsDeleted=@IsDeleted where IdEmployee="
                    + employee.IdEmployee;
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@Name", employee.Name);
                command.Parameters.AddWithValue("@Gender", employee.Gender);
                command.Parameters.AddWithValue("@Phonenumber", employee.Phonenumber);
                command.Parameters.AddWithValue("@Address", employee.Address);
                command.Parameters.AddWithValue("@DateofBirth", employee.DateOfBirth);
                command.Parameters.AddWithValue("@Position", employee.Position);
                command.Parameters.AddWithValue("@Startingdate", employee.Startingdate);
                command.Parameters.AddWithValue("@ImageFile", Convert.ToBase64String(employee.ImageFile));
                command.Parameters.AddWithValue("@IsDeleted", employee.IsDeleted);
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
        public bool DeleteEmployee(Employee employee)
        {
            try
            {
                OpenConnection();
                string query = @"Update Employee" +"Set isDeleted=1"+" where idEmployee = "+employee.IdEmployee.ToString();
                SqlCommand command = new SqlCommand(query, connect);
                if (command.ExecuteNonQuery() > 0)
                    return true;
                else
                {
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Xóa thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
        public void AddEmployee(Employee employee)
        {
            if (ConvertDBToList().Count == 0 || employee.IdEmployee > GetMaxIdEmployee())
            {
                if (AddIntoDB(employee))
                    MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                else
                    MessageBox.Show("Thêm thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (UpdateOnDB(employee))
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                else
                    MessageBox.Show("Cập nhật thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public Employee GetEmployee(string idEmployee) // Bao gồm cả nhân viên đã xóa hoặc chưa xóa
        {
            Employee res = new Employee();
            try
            {
                OpenConnection();
                string queryString = "select * from Employee where idEmployee = " + idEmployee;

                SqlCommand command = new SqlCommand(queryString, connect);
                command.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                int idAccount = -1;
                if (dataTable.Rows[0].ItemArray[8].ToString() != "")
                {
                    idAccount = int.Parse(dataTable.Rows[0].ItemArray[8].ToString());
                }
                res = new Employee(int.Parse(dataTable.Rows[0].ItemArray[0].ToString()),
                     dataTable.Rows[0].ItemArray[1].ToString(), dataTable.Rows[0].ItemArray[2].ToString(),
                     dataTable.Rows[0].ItemArray[3].ToString(), dataTable.Rows[0].ItemArray[4].ToString(),
                     DateTime.Parse(dataTable.Rows[0].ItemArray[5].ToString()),
                     dataTable.Rows[0].ItemArray[6].ToString(), DateTime.Parse(dataTable.Rows[0].ItemArray[7].ToString()),
                     idAccount, Convert.FromBase64String(dataTable.Rows[0].ItemArray[9].ToString()), int.Parse(dataTable.Rows[0].ItemArray[10].ToString()));
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
        public Employee GetEmployeeByIdEmployee(string idEmployee) // Lấy thông tin khi biết id nhân viên - Không lấy nhân viên đã xóa
        {
            Employee res = new Employee();
            try
            {
                OpenConnection();
                string queryString = "Select * from Employee where IsDeleted = 0 and IdEmployee = " + idEmployee;

                SqlCommand command = new SqlCommand(queryString, connect);
                command.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                int idAccount = -1;
                if (dataTable.Rows[0].ItemArray[8].ToString() != "")
                {
                    idAccount = int.Parse(dataTable.Rows[0].ItemArray[8].ToString());
                }
                res = new Employee(int.Parse(dataTable.Rows[0].ItemArray[0].ToString()),
                     dataTable.Rows[0].ItemArray[1].ToString(), dataTable.Rows[0].ItemArray[2].ToString(),
                     dataTable.Rows[0].ItemArray[3].ToString(), dataTable.Rows[0].ItemArray[4].ToString(),
                     DateTime.Parse(dataTable.Rows[0].ItemArray[5].ToString()),
                     dataTable.Rows[0].ItemArray[6].ToString(), DateTime.Parse(dataTable.Rows[0].ItemArray[7].ToString()),
                     idAccount, Convert.FromBase64String(dataTable.Rows[0].ItemArray[9].ToString()), int.Parse(dataTable.Rows[0].ItemArray[10].ToString()));
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
        public Employee GetEmployeeByIdAccount(string idAccount)
        {
            Employee res = new Employee();
            try
            {
                OpenConnection();
                string queryString = "select * from Employee where IdAccount = " + idAccount;

                SqlCommand command = new SqlCommand(queryString, connect);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                res = new Employee(int.Parse(dataTable.Rows[0].ItemArray[0].ToString()),
                     dataTable.Rows[0].ItemArray[1].ToString(), dataTable.Rows[0].ItemArray[2].ToString(),
                     dataTable.Rows[0].ItemArray[3].ToString(), dataTable.Rows[0].ItemArray[4].ToString(),
                     DateTime.Parse(dataTable.Rows[0].ItemArray[5].ToString()),
                     dataTable.Rows[0].ItemArray[6].ToString(), DateTime.Parse(dataTable.Rows[0].ItemArray[7].ToString()),
                     int.Parse(idAccount), Convert.FromBase64String(dataTable.Rows[0].ItemArray[9].ToString()), int.Parse(dataTable.Rows[0].ItemArray[10].ToString()));
            }
            catch
            {
                res.Name = "Chủ sân";
            }
            finally
            {
                CloseConnection();
            }
            return res;
        }
        public List<Employee> GetEmployeesByType(string typeEmployee)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                OpenConnection();
                string queryString = "select * from Employee where IsDeleted=0 and Position = " + typeEmployee;

                SqlCommand command = new SqlCommand(queryString, connect);
                command.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    int idAccount = -1;
                    if (dataTable.Rows[i].ItemArray[8].ToString() != "")
                    {
                        idAccount = int.Parse(dataTable.Rows[i].ItemArray[8].ToString());
                    }
                    Employee employee = new Employee(int.Parse(dataTable.Rows[i].ItemArray[0].ToString()),
                        dataTable.Rows[i].ItemArray[1].ToString(), dataTable.Rows[i].ItemArray[2].ToString(),
                        dataTable.Rows[i].ItemArray[3].ToString(), dataTable.Rows[i].ItemArray[4].ToString(),
                        DateTime.Parse(dataTable.Rows[i].ItemArray[5].ToString()),
                        dataTable.Rows[i].ItemArray[6].ToString(), DateTime.Parse(dataTable.Rows[i].ItemArray[7].ToString()),
                        idAccount, Convert.FromBase64String(dataTable.Rows[i].ItemArray[9].ToString()), int.Parse(dataTable.Rows[i].ItemArray[10].ToString()));
                    employees.Add(employee);
                }
            }
            catch
            {

            }
            finally
            {
                CloseConnection();
            }
            return employees;
        }

        public string GetPosition(string id) // Lấy chức vụ khi biết id
        {
            try
            {
                OpenConnection();
                string query = "Select Position from Employee where IdEmployee = " + id;
                SqlCommand command = new SqlCommand(query, connect);
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                return dt.Rows[0].ItemArray[0].ToString();
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
        public List<string> GetAllPosition()
        {
            try
            {
                OpenConnection();
                List<string> newList = new List<string>();
                string query = "select distinct(Position) from Employee";
                SqlCommand command = new SqlCommand(query, connect);
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    newList.Add(dt.Rows[i].ItemArray[0].ToString());
                }
                return newList;
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
    }
}
