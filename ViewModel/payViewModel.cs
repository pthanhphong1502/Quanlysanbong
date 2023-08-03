using QlySanBong.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using QlySanBong.data_provier;
using QlySanBong.ResourceXAML;
using QlySanBong.Model;
using System.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;

namespace QlySanBong.ViewModel
{
    public class payViewModel : BaseViewModel, INotifyPropertyChanged
    {
        //Binding 
        private pay payWindow;
        private string total;
        private string totalGoods;
        public string Total
        {
            get => total;
            set
            {
                total = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Total)));
                OnPropertyChanged();
            }
        }
        public string TotalGoods
        {
            get => totalGoods;
            set
            {
                totalGoods = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalGoods)));
                OnPropertyChanged();
            }
        }
        public pay PayWindow { get => payWindow; set => payWindow = value; }
        public event PropertyChangedEventHandler PropertyChanged;
        //Pay Window
        public ICommand LoadGoodsCommand { get; set; }
        public ICommand LoadBillInfoCommand { get; set; }
        public ICommand LoadTotalCommand { get; set; }
        public ICommand ClosingWdCommnad { get; set; }
        public ICommand PayBillCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand PickGoodsCommand { get; set; } // Chọn 1 hàng 
        public ICommand GetWindowCommand { get; set; }
        //UC Product Detail
        public ICommand DeleteBillInfoCommand { get; set; }
        public ICommand ChangeQuantityCommand { get; set; }

        public ICommand ViewBillCommand { get; set; }

        public payViewModel()
        {
            LoadBillInfoCommand = new RelayCommand<pay>((parameter) => true, (parameter) => LoadBillInfoToView(parameter)); // Hiển thị các mặt hàng được chọn
        }


        public void LoadBillInfoToView(pay parameter)
        {
            TotalGoods = "0";
            parameter.stkPickedGoods.Children.Clear();
            List<BillInfo> billInfos = BillInfoDP.Instance.GetBillInfos(parameter.txbIdBill.Text);
            for (int i = 0; i < billInfos.Count; i++)
            {
                ProductDetailsControl infoControl = new ProductDetailsControl();
                infoControl.txbNo.Text = (i + 1).ToString();
                infoControl.txbIdGoods.Text = billInfos[i].IdGoods.ToString();
                infoControl.txbIdBill.Text = billInfos[i].IdBill.ToString();
                Goods goods = GoodsDP.Instance.GetGoods(billInfos[i].IdGoods.ToString());
                infoControl.txbName.Text = goods.Name;
                infoControl.txbPrice.Text = string.Format("{0:N0}", goods.UnitPrice);
                infoControl.nmsQuantity.Text = decimal.Parse(billInfos[i].Quantity.ToString());
                infoControl.nmsQuantity.MinValue = 1;
                infoControl.nmsQuantity.MaxValue = goods.Quantity;
                infoControl.txbtotal.Text = string.Format("{0:N0}", (infoControl.nmsQuantity.Value * ConvertToNumber(infoControl.txbPrice.Text)));
                parameter.stkPickedGoods.Children.Add(infoControl);
            }
            TotalGoods = string.Format("{0:N0}", BillInfoDP.Instance.CountSumMoney(parameter.txbIdBill.Text));
            Total = string.Format("{0:N0}", ConvertToNumber(TotalGoods) + ConvertToNumber(parameter.txbFieldPrice.Text) - ConvertToNumber(parameter.txbDiscount.Text));
        }
    }
}
