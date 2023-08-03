using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlySanBong.Model
{
    class StockReceiptInfo
    {
        //Constructor
        public StockReceiptInfo()
        {

        }
        public StockReceiptInfo(int idStockReceipt, int idGoods, int quantity, long importPrice)
        {
            this.idStockReceipt = idStockReceipt;
            this.idGoods = idGoods;
            this.quantity = quantity;
            this.importPrice = importPrice;
        }
        //Attribute
        private int idStockReceipt;
        public int IdStockReceipt { get => idStockReceipt; set => idStockReceipt = value; }

        private int idGoods;
        public int IdGoods { get => idGoods; set => idGoods = value; }

        private int quantity;
        public int Quantity { get => quantity; set => quantity = value; }

        private long importPrice;
        public long ImportPrice { get => importPrice; set => importPrice = value; }
    }
}
