﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlySanBong.Model
{
    class Bill
    {
        //Constructor
        public Bill()
        {

        }

        public Bill(int idBill, int idAccount, DateTime invoiceDate, DateTime checkInTime, DateTime checkOutTime, int status, long totalMoney, int idFieldInfo, string note)
        {
            this.idBill = idBill;
            this.idAccount = idAccount;
            this.invoiceDate = invoiceDate;
            this.checkInTime = checkInTime;
            this.checkOutTime = checkOutTime;
            this.status = status;
            this.totalMoney = totalMoney;
            this.IdFieldInfo = idFieldInfo;
            this.note = note;
        }

        //Attribute
        private int idBill;
        public int IdBill { get => idBill; set => idBill = value; }

        private int idAccount;
        public int IdAccount { get => idAccount; set => idAccount = value; }

        private DateTime invoiceDate;
        public DateTime InvoiceDate { get => invoiceDate; set => invoiceDate = value; }

        private DateTime checkInTime;
        public DateTime CheckInTime { get => checkInTime; set => checkInTime = value; }

        private DateTime checkOutTime;
        public DateTime CheckOutTime { get => checkOutTime; set => checkOutTime = value; }

        private int status;
        public int Status { get => status; set => status = value; }

        private long totalMoney;
        public long TotalMoney { get => totalMoney; set => totalMoney = value; }

        private int idFieldInfo;
        public int IdFieldInfo { get => idFieldInfo; set => idFieldInfo = value; }

        private string note;
        public string Note { get => note; set => note = value; }
    }
}
