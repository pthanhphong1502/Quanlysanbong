using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlySanBong.Model
{
    class BillInfo
    {
        //Constructor
        public BillInfo()
        {

        }
        public BillInfo(int idBill, int idGoods,string username, DateTime importday ,string nameGoods,int pricegoods,int quantity,long total)
        {
            this.idBill = idBill;
            this.idGoods = idGoods;
            this.username = username;
            this.importday = importday;
            this.nameGoods = nameGoods;
            this.pricegoods = pricegoods;
            this.quantity = quantity;
            this.total = total;
        }

        //Attribute
        private long total;
        public long Total { get => total; set => total = value; }

        private int idBill;
        public int IdBill { get => idBill; set => idBill = value; }

        private int idGoods;
        public int IdGoods { get => idGoods; set => idGoods = value; }

        private int quantity;
        public int Quantity { get => quantity; set => quantity = value; }

        private int pricegoods;
        public int  Pricegoods{ get => pricegoods; set => pricegoods = value; }

        private string username;
        public string Username { get => username; set => username =value; }

        private string nameGoods;
        public string NameGoods { get => nameGoods; set => nameGoods= value; }

        private DateTime importday;

        public DateTime ImportDay { get => importday; set => importday = value;  }
    }
}
