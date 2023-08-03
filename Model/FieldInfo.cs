﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlySanBong.Model
{
    class FieldInfo
    {
        public FieldInfo()
        {

        }
        public FieldInfo(int idFieldInfo, int idField, DateTime startingTime, DateTime endingTime, int status, string phoneNumber, string customerName, string note, long price)
        {
            this.idFieldInfo = idFieldInfo;
            this.idField = idField;
            this.startingTime = startingTime;
            this.endingTime = endingTime;
            this.status = status;
            this.phoneNumber = phoneNumber;
            this.customerName = customerName;
            this.note = note;
            this.price = price;
        }
        private int idFieldInfo;
        public int IdFieldInfo { get => idFieldInfo; set => idFieldInfo = value; }

        private int idField;
        public int IdField { get => idField; set => idField = value; }

        private DateTime startingTime;
        public DateTime StartingTime { get => startingTime; set => startingTime = value; }

        private DateTime endingTime;
        public DateTime EndingTime { get => endingTime; set => endingTime = value; }

        int status;
        public int Status { get => status; set => status = value; }

        private string phoneNumber;
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }

        private string customerName;
        public string CustomerName { get => customerName; set => customerName = value; }

        private string note;
        public string Note { get => note; set => note = value; }

        private long discount;
        public long Discount { get => discount; set => discount = value; }

        private long price;
        public long Price { get => price; set => price = value; }
    }
}
