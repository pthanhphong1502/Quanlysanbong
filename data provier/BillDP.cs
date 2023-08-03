﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using QlySanBong.Model;
using System.Data;
using System.Data.SqlClient;

namespace QlySanBong.data_provier
{
    class BillDP : DataProvier
    {
        private static BillDP instance;

        public static BillDP Instance
        {
            get { if (instance == null) instance = new BillDP(); return BillDP.instance; }
            private set { BillDP.instance = value; }
        }
        private BillDP()
        {

        }
        public bool DeleteFromDB(string idBill)
        {
            try
            {
                OpenConnection();
                string queryString = "delete from Bill where IdBill=" + idBill;
                SqlCommand command = new SqlCommand(queryString, conn);
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
        public bool AddIntoDB(Bill bill)
        {
            try
            {
                OpenConnection();
                string queryString = "insert into Bill(IdBill, IdAccount, InvoiceDate,CheckInTime,CheckOutTime,Status,TotalMoney,IdFieldInfo ) values(@idBill, @idAccount, @invoiceDate,@checkInTime,@checkOutTime,@status,@totalMoney,@idFieldInfo)";
                SqlCommand command = new SqlCommand(queryString, conn);
                command.Parameters.AddWithValue("@idBill", bill.IdBill.ToString());
                command.Parameters.AddWithValue("@idAccount", bill.IdAccount.ToString());
                command.Parameters.AddWithValue("@invoiceDate", bill.InvoiceDate);
                command.Parameters.AddWithValue("@checkInTime", bill.CheckInTime);
                command.Parameters.AddWithValue("@checkOutTime", bill.CheckOutTime);
                command.Parameters.AddWithValue("@status", bill.Status);
                command.Parameters.AddWithValue("@totalMoney", bill.TotalMoney.ToString());
                command.Parameters.AddWithValue("@idFieldInfo", bill.IdFieldInfo.ToString());
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
        public bool UpdateOnDB(Bill bill)
        {
            try
            {
                OpenConnection();
                string queryString = "update Bill set CheckOutTime=@checkOutTime,Status=@status,TotalMoney=@totalMoney,Note=@note where IdBill=@idBill";
                SqlCommand command = new SqlCommand(queryString, conn);
                command.Parameters.AddWithValue("@idBill", bill.IdBill.ToString());
                command.Parameters.AddWithValue("@checkOutTime", bill.CheckOutTime);
                command.Parameters.AddWithValue("@status", bill.Status);
                command.Parameters.AddWithValue("@totalMoney", bill.TotalMoney);
                command.Parameters.AddWithValue("@note", bill.Note);
                int rs = command.ExecuteNonQuery();
                if (rs == 1)
                {
                    return true;

                }
                else
                    return false;

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
        public List<Bill> ConvertDBToList()
        {
            try
            {
                List<Bill> bills = new List<Bill>();
                OpenConnection();
                string queryString = "select * from Bill";

                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    int idAccount = -1;
                    if (dataTable.Rows[i].ItemArray[1].ToString() != "")
                    {
                        idAccount = int.Parse(dataTable.Rows[i].ItemArray[1].ToString());
                    }
                    Bill bill = new Bill(int.Parse(dataTable.Rows[i].ItemArray[0].ToString()), idAccount, DateTime.Parse(dataTable.Rows[i].ItemArray[2].ToString()), DateTime.Parse(dataTable.Rows[i].ItemArray[3].ToString()), DateTime.Parse(dataTable.Rows[i].ItemArray[4].ToString()), int.Parse(dataTable.Rows[i].ItemArray[5].ToString()), long.Parse(dataTable.Rows[i].ItemArray[6].ToString()), int.Parse(dataTable.Rows[i].ItemArray[7].ToString()), dataTable.Rows[i].ItemArray[8].ToString());
                    bills.Add(bill);
                }
                return bills;
            }
            catch
            {
                return new List<Bill>();
            }
            finally
            {
                CloseConnection();
            }
        }
        public Bill GetBill(string idBill)
        {
            try
            {
                OpenConnection();
                string queryString = "select * from Bill where IdBill = " + idBill;

                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                Bill res;
                if (string.IsNullOrEmpty(dataTable.Rows[0].ItemArray[1].ToString()))
                {
                    res = new Bill(int.Parse(idBill), 0, DateTime.Parse(dataTable.Rows[0].ItemArray[2].ToString()),
                        DateTime.Parse(dataTable.Rows[0].ItemArray[3].ToString()), DateTime.Parse(dataTable.Rows[0].ItemArray[4].ToString()),
                        int.Parse(dataTable.Rows[0].ItemArray[5].ToString()), long.Parse(dataTable.Rows[0].ItemArray[6].ToString()),
                        int.Parse(dataTable.Rows[0].ItemArray[7].ToString()), dataTable.Rows[0].ItemArray[8].ToString());
                }
                else
                {
                    res = new Bill(int.Parse(idBill), int.Parse(dataTable.Rows[0].ItemArray[1].ToString()),
                        DateTime.Parse(dataTable.Rows[0].ItemArray[2].ToString()), DateTime.Parse(dataTable.Rows[0].ItemArray[3].ToString()),
                        DateTime.Parse(dataTable.Rows[0].ItemArray[4].ToString()), int.Parse(dataTable.Rows[0].ItemArray[5].ToString()),
                        long.Parse(dataTable.Rows[0].ItemArray[6].ToString()), int.Parse(dataTable.Rows[0].ItemArray[7].ToString()),
                        dataTable.Rows[0].ItemArray[8].ToString());
                }
                return res;
            }
            catch
            {
                return new Bill();
            }
            finally
            {
                CloseConnection();
            }
        }
        //Sau khi xóa nhân viên => xóa Account => update idAccount về NULL 
        public bool UpdateIdAccount(string idAccount)
        {
            try
            {
                OpenConnection();
                string queryString = "update Bill set IdAccount = NULL where IdAccount = " + idAccount;
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

        public DataTable LoadBillByDate(string day, string month, string year)
        {
            DataTable dataTable = new DataTable();
            try
            {
                OpenConnection();
                string queryString = string.Format("select IdBill, IdAccount, InvoiceDate, CheckOutTime, TotalMoney " +
                    "from Bill where year(InvoiceDate) = {0} and month(InvoiceDate) = {1} and day(InvoiceDate) = {2} order by IdBill", year, month, day);

                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch
            {
                return dataTable;
            }
            finally
            {
                CloseConnection();
            }
        }
        public DataTable LoadBillByMonth(string month, string year)
        {
            DataTable dataTable = new DataTable();
            try
            {
                OpenConnection();
                string queryString = string.Format("select IdBill, IdAccount, InvoiceDate, CheckOutTime, TotalMoney " +
                    "from Bill where year(InvoiceDate) = {0} and month(InvoiceDate) = {1} order by IdBill", year, month);

                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch
            {
                return dataTable;
            }
            finally
            {
                CloseConnection();
            }
        }
        public DataTable LoadBillByYear(string year)
        {
            DataTable dataTable = new DataTable();
            try
            {
                OpenConnection();
                string queryString = string.Format("select IdBill, IdAccount, InvoiceDate, CheckOutTime, TotalMoney " +
                    "from Bill where year(InvoiceDate) = {0} order by IdBill", year);

                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch
            {
                return dataTable;
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
                string queryString = @"Select max(IdBill) From Bill";

                SqlCommand command = new SqlCommand(queryString, conn);
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
