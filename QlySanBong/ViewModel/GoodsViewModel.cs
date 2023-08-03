using Microsoft.Win32;
using QlySanBong.data_provier;
using QlySanBong.Model;
using QlySanBong.ResourceXAML;
using QlySanBong.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data.SqlClient;
using System.Diagnostics;

namespace QlySanBong.ViewModel
{
    public class GoodsViewModel: BaseViewModel,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private HomeWindow homewd;
        public HomeWindow Homewd { get => homewd; set => homewd = value; }

        public long total;
        public long Total
        {
            get => total;
            set
            {
                total = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Total)));
            }
        }


        private string imagefilename;

        public ICommand loadGoodsCommand { get; set; }

        public ICommand AddGoodsCommand { get; set; }

        public ICommand PayGoodsCommand { get; set; }

        public ICommand SelectImageCommand { get; set; }

        public ICommand SaveGoodsCommand { get; set; }

        public ICommand CloseAddGoodsCommand { get; set; }

        public ICommand LoadedHistoryImportCommand { get; set; }

        public ICommand OpenImstockGoodsCommand { get; set; }
        public ICommand OpenHistoryImportCommand { get; set; }

        public ICommand EditGoodsCommand   { get; set; }
        public ICommand DeleteGoodsCommand { get; set; }
        public ICommand ImportGoodsCommand { get; set; }

        public ICommand CancelImportCommand { get; set; }
        public ICommand CaculateTotal { get; set; }

        public ICommand GetWindowCommand { get; set; }
        public ICommand ExitImportStockCommand { get; set; }

        public ICommand PayCommand { get; set; }
        public ICommand SaveImport { get; set; }

        public ICommand LoadBillImportCommand { get; set; }
        public ICommand printBillCommand { get; set; }
        public ICommand ShowStockInfoCommad { get; set; }
        public ICommand SeparateThousandsCommand { get; set; }
        //public ICommand 

        public GoodsViewModel()
        {
            SeparateThousandsCommand = new RelayCommand<TextBox>((parameter) => true, (parameter) => SeparateThousands(parameter));

            loadGoodsCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => LoadGoodsswindow(parameter));

            AddGoodsCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => Openaddgoodswindow(parameter));
            
            SelectImageCommand = new RelayCommand<Grid>((parameter) => true, (parameter) => SelectImage(parameter));

            SaveGoodsCommand = new RelayCommand<addgoods>((parameter) => true, (parameter) =>AddGoodswindow(parameter));

            CloseAddGoodsCommand = new RelayCommand<addgoods>((parameter) => true, (parameter) => CloseWindow(parameter));

            EditGoodsCommand = new RelayCommand<TextBlock>((parameter) => true, (parameter) => 
            {
                if (CurrentAccount.Type == 0)
                    EditGoodsWindow(parameter);
                    });

            ImportGoodsCommand = new RelayCommand<TextBlock>((parameter) => true, (parameter) => ImportGoodsWindow(parameter));

            DeleteGoodsCommand = new RelayCommand<ControlGoods>((parameter) => true, (parameter) => 
            {
                if (CurrentAccount.Type == 0) DeleteGoodsWindow(parameter);
            }
            );
            CancelImportCommand = new RelayCommand<importgoods>((parameter) => true, (parameter) => parameter.Close());
            CaculateTotal = new RelayCommand<importgoods>((parameter) => true, (parameter) => CaculateTotalWindow(parameter));


            SaveImport = new RelayCommand<importgoods>((parameter) => true, (parameter) => SaveImportWindow(parameter));

            OpenHistoryImportCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => OpenHistoryImportWindow(parameter));

            LoadedHistoryImportCommand = new RelayCommand<historyImport>((parameter) => true, (parameter) => LoadedHistoryImportWindow(parameter));
            ShowStockInfoCommad = new RelayCommand<Historyimportcontrol>((parameter) => true, (parameter) => ShowStockInfoWindow(parameter));

            LoadBillImportCommand = new RelayCommand<ReceiptImport>((parameter) => true, (parameter) => LoadBillImportWinodow(parameter));

            printBillCommand = new RelayCommand<ReceiptImport>((parameter) => true, (parameter) => printBillCommandWinodow(parameter));
        }

        private void ShowStockInfoWindow(Historyimportcontrol parameter)
        {
            StockReceiptInfo stockReceiptInfo = StockReceiptInfoDP.Instance.GetStockReceiptInfoById(parameter.txbidStockReceipt.Text);
            ViewStockInfo view = new ViewStockInfo();
            
            view.txbIdStock.Text = "#"+stockReceiptInfo.IdStockReceipt.ToString();
            view.txbIdGood.Text = "#"+stockReceiptInfo.IdGoods.ToString();
            view.txbImportPrice.Text = string.Format("{0:N0}", stockReceiptInfo.ImportPrice) +"VNĐ";
            view.txbQuantity.Text = stockReceiptInfo.Quantity.ToString();
            view.ShowDialog();
        }

        private void printBillCommandWinodow(ReceiptImport parameter)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    parameter.btnPrintBill.Visibility = Visibility.Hidden;
                    printDialog.PrintVisual(parameter.grBill, "Bill");
                    
                }
            }
            finally
            {
                parameter.btnPrintBill.Visibility = Visibility.Visible;
                parameter.Close();
            }
        }

        private void LoadBillImportWinodow(ReceiptImport parameter)
        {
            int idbill = StockReceiptDP.Instance.GetMaxId();
            StockReceipt stockReceipt = StockReceiptDP.Instance.GetDBByID(idbill.ToString());
            parameter.txbIdReceipt.Text = "#" +(idbill).ToString();
            StockReceiptInfo stockReceiptInfo = StockReceiptInfoDP.Instance.GetStockReceiptInfoById(idbill.ToString());
            parameter.txbIdName.Text = "#" + CurrentAccount.IdAccount.ToString();
            parameter.txbDayImport.Text = stockReceipt.DateTimeStockReceipt.ToString("dd/MM/yyyy");
            parameter.txbImportPrice.Text = stockReceiptInfo.ImportPrice.ToString();
            parameter.txbNameGoods.Text = "#" + stockReceiptInfo.IdGoods.ToString();
            parameter.txbQuanity.Text = stockReceiptInfo.Quantity.ToString();
            parameter.txbTotal.Text = string.Format("{0:N0}",stockReceipt.Total)+" VNĐ";
        }

        private void LoadedHistoryImportWindow(historyImport parameter)
        {
            parameter.stkHistoryImportGoods.Children.Clear();
            bool flag = false; 
            foreach (var lsstockReceipt in StockReceiptDP.Instance.ConvertDBToList())
            {
                Historyimportcontrol hic = new Historyimportcontrol();
                flag = !flag;
                hic.txbidStockReceipt.Text = lsstockReceipt.IdStockReceipt.ToString();
                hic.txbIdAccountReceipt.Text = lsstockReceipt.IdAccount.ToString();
                hic.txbTimeReceipt.Text = lsstockReceipt.DateTimeStockReceipt.ToString("dd/MM/yyyy");
                hic.txbToTalReceipt.Text =string.Format("{0:N0}", lsstockReceipt.Total)+" VNĐ";
                if (flag)
                {
                    hic.grdMain.Background = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                }
                else
                {
                    hic.grdMain.Background = (Brush)new BrushConverter().ConvertFromString("#e4e4e4");
                }
                parameter.stkHistoryImportGoods.Children.Add(hic);
       
            }
        }

        private void OpenHistoryImportWindow(HomeWindow parameter)
        {
            if (CurrentAccount.Type == 0)
            {
                historyImport hi = new historyImport();
                hi.ShowDialog();
            }
            else
            {
                MessageBox.Show("Không đủ quyền hạn","Thông báo",MessageBoxButton.OK,MessageBoxImage.Stop);
            }    
        }

        private void SaveImportWindow(importgoods parameter)
        {
            if (string.IsNullOrEmpty(parameter.dpImportDate.Text))
            {
                parameter.dpImportDate.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(parameter.txtImportPrice.Text))
            {
                parameter.txtImportPrice.Focus();
                parameter.txtImportPrice.Text = "";
                return;
            }
            if (string.IsNullOrWhiteSpace(parameter.txtQuantity.Text) || !Regex.IsMatch(parameter.txtQuantity.Text, @"^[0-9]+$"))
            {
                parameter.txtQuantity.Focus();
                parameter.txtQuantity.Text = "";
                return;
            }

            Goods goods = new Goods(int.Parse(parameter.txtIdGoods.Text), parameter.txtName.Text,
                parameter.cboUnit.Text, 1, GoodsDP.Instance.GetGoods(parameter.txtIdGoods.Text).ImageFile, int.Parse(parameter.txtQuantity.Text));
            bool isSuccessed1 = GoodsDP.Instance.ImportToDB(goods);

            StockReceipt stockReceipt = new StockReceipt(int.Parse(parameter.txtIdStockReceipt.Text), CurrentAccount.IdAccount,
                DateTime.Parse(parameter.dpImportDate.Text), ConvertToNumber(parameter.txtTotal.Text));
            bool isSuccessed2 = StockReceiptDP.Instance.AddIntoDB(stockReceipt);

            StockReceiptInfo stockReceiptInfo = new StockReceiptInfo(int.Parse(parameter.txtIdStockReceipt.Text),
                int.Parse(parameter.txtIdGoods.Text), Convert.ToInt32(ConvertToNumber(parameter.txtQuantity.Text)),
                ConvertToNumber(parameter.txtImportPrice.Text));

            bool isSuccessed3 = StockReceiptInfoDP.Instance.AddIntoDB(stockReceiptInfo);

            if (isSuccessed1 && isSuccessed2 && isSuccessed3)
            {
                MessageBox.Show("Nhập hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                MessageBoxResult result = MessageBox.Show("Bạn có muốn IN HÓA ĐƠN không ?","Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    ReceiptImport receiptImport = new ReceiptImport();
                    receiptImport.ShowDialog();
                }
                parameter.Close();
                LoadGoodsswindow(homewd);
            }
            else
            {
                MessageBox.Show("Thực hiện thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

  

        private void CaculateTotalWindow(importgoods parameter)
        {
            long Price = 0;
            int quantitysum = 0;
            if (!string.IsNullOrEmpty(parameter.txtImportPrice.Text))
            {
                Price = ConvertToNumber(parameter.txtImportPrice.Text);
            }
            int.TryParse(parameter.txtQuantity.Text, out quantitysum);
            parameter.txtTotal.Text = string.Format("{0:N0}", Price * quantitysum);
        }

        private void DeleteGoodsWindow(ControlGoods parameter)
        {

            MessageBoxResult result = MessageBox.Show("Xác nhận xóa ?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                string idgoods = parameter.txbId.Text;
                bool issuccess = GoodsDP.Instance.DeleteFromDB(idgoods);
                if (issuccess)
                    homewd.stkGoods.Children.Remove(parameter);
                else
                    MessageBox.Show("Xóa thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImportGoodsWindow(TextBlock parameter)
        {
            Goods goods = GoodsDP.Instance.GetGoods(parameter.Text);
            importgoods importgoods = new importgoods();
            importgoods.txtImportPrice.Focus();
            importgoods.dpImportDate.Text = DateTime.Now.ToString("HH:mm:ss dd:MM:yy");
            try
            {
                importgoods.txtIdStockReceipt.Text = (StockReceiptDP.Instance.GetMaxId() + 1).ToString();
            }
            catch
            {
                importgoods.txtIdStockReceipt.Text = "1";
            }
            importgoods.txtIdGoods.Text = goods.IdGoods.ToString();

            importgoods.txtName.Text = goods.Name;
            importgoods.txtName.SelectionStart = importgoods.txtName.Text.Length;

            importgoods.cboUnit.Text = goods.Unit;

            importgoods.txtQuantity.SelectionStart = importgoods.txtQuantity.Text.Length;
            importgoods.txtQuantity.Select(0, importgoods.txtQuantity.Text.Length);
            importgoods.txtQuantity.Clear();

            importgoods.txtImportPrice.SelectionStart = importgoods.txtImportPrice.Text.Length;
            importgoods.txtImportPrice.Select(0, importgoods.txtImportPrice.Text.Length);
            importgoods.txtImportPrice.Clear();

            ImageBrush imageBrush = new ImageBrush();
            byte[] blob = goods.ImageFile;
            BitmapImage bitmap = Converter.Instance.ConvertByteToBitmapImage(blob);
            imageBrush.ImageSource = bitmap;
            importgoods.grdSelectImg.Background = imageBrush;
            if (importgoods.grdSelectImg.Children.Count >1)
            {
                importgoods.grdSelectImg.Children.Remove(importgoods.grdSelectImg.Children[0]);
                importgoods.grdSelectImg.Children.Remove(importgoods.grdSelectImg.Children[1]);
            }
            importgoods.ShowDialog();
        }

        private void EditGoodsWindow(TextBlock parameter)
        {
            Goods goods = GoodsDP.Instance.GetGoods(parameter.Text);
            addgoods updatewd = new addgoods();
            updatewd.txtIdGoods.Text = goods.IdGoods.ToString();
            updatewd.txtName.Text = goods.Name;
            updatewd.txtName.SelectionStart = updatewd.txtName.Text.Length;
            updatewd.txtName.Select(0, updatewd.txtName.Text.Length);
            updatewd.cboUnit.Text = goods.Unit;
            updatewd.txtUnitPrice.Text = string.Format("{0:N0}", goods.UnitPrice);
            updatewd.txtUnitPrice.SelectionStart = updatewd.txtUnitPrice.Text.Length;
            updatewd.txtUnitPrice.Select(0, updatewd.txtUnitPrice.Text.Length);
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = Converter.Instance.ConvertByteToBitmapImage(goods.ImageFile);
            updatewd.grdSelectImg.Background = imageBrush;

            if (updatewd.grdSelectImg.Children.Count > 1)
            {
                updatewd.grdSelectImg.Children.Remove(updatewd.grdSelectImg.Children[0]);
                updatewd.grdSelectImg.Children.Remove(updatewd.grdSelectImg.Children[1]);
            }
            updatewd.Title= "Cập nhật thông báo";
            updatewd.Tag = "cập nhật";
            updatewd.ShowDialog();
        }

        private void CloseWindow(addgoods parameter)
        {
            parameter.Close();
        }

        private void AddGoodswindow(addgoods parameter)
        {
            List<Goods> goodsList = GoodsDP.Instance.ConverDBtoList();
            if (string.IsNullOrWhiteSpace(parameter.txtName.Text))
            {
                parameter.txtName.Focus();
                parameter.txtName.Text = "";
                return;
            }
            if (string.IsNullOrEmpty(parameter.cboUnit.Text))
            {
                parameter.cboUnit.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(parameter.txtUnitPrice.Text))
            {
                parameter.txtUnitPrice.Focus();
                parameter.txtUnitPrice.Text = "";
                return;
            }
            if(parameter.grdSelectImg.Background == null)
            {
                MessageBox.Show("Vui lòng thêm hình ảnh!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            byte[] imagebyteArr ;
           
            try
            {
                imagebyteArr = Converter.Instance.ConvertImageToBytes(imagefilename);
            }
            catch(Exception ex)
            {
                imagebyteArr = GoodsDP.Instance.GetGoods(parameter.txtIdGoods.Text).ImageFile;
            }
            imagefilename = null;
            Goods newGoods = new Goods(int.Parse(parameter.txtIdGoods.Text), parameter.txtName.Text,
                parameter.cboUnit.Text, ConvertToNumber(parameter.txtUnitPrice.Text), imagebyteArr);
            bool isSuccessed1 = true, isSuccessed2 = true;
            if (goodsList.Count == 0 || newGoods.IdGoods > goodsList[goodsList.Count - 1].IdGoods)
            {
                if (GoodsDP.Instance.IsExistGoodsName(parameter.txtName.Text))
                {
                    MessageBox.Show("Mặt hàng đã tồn tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    parameter.txtName.Focus();
                    parameter.txtName.Text = "";
                    return;
                }
                isSuccessed1 = GoodsDP.Instance.AddIntoDB(newGoods);

                if (isSuccessed1)
                {
                    MessageBox.Show("Thêm mặt hàng thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
            else
            {
                if (GoodsDP.Instance.GetGoods(parameter.txtIdGoods.Text).Name != parameter.txtName.Text)
                    if(GoodsDP.Instance.IsExistGoodsName(parameter.txtName.Text))
                            {
                                MessageBox.Show("Vui lòng nhập lại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                                parameter.txtName.Focus();
                                parameter.txtName.Text = "";
                                return;
                            }
                isSuccessed2 = GoodsDP.Instance.UpdateOnDB(newGoods);
                if (isSuccessed2)
                {
                    MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
            if(!isSuccessed2 || !isSuccessed1)
            {
                MessageBox.Show("Thực hiện thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadGoodsswindow(homewd);
            parameter.Close();
            
        }

        public void SelectImage(Grid parameter)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Chọn ảnh";
            open.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            if (open.ShowDialog() == true)
            {
                imagefilename = open.FileName;
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(imagefilename);
                bitmap.EndInit();
                imageBrush.ImageSource = bitmap;
                parameter.Background = imageBrush;
                if (parameter.Children.Count > 1)
                {
                    parameter.Children.Remove(parameter.Children[0]);
                    parameter.Children.Remove(parameter.Children[1]);
                }
            }
        }

        public void LoadGoodsswindow(HomeWindow parameter)
        {
            if (CurrentAccount.Type == 1) parameter.btnAddGoods.IsEnabled = false;
            this.homewd = parameter;
            parameter.stkGoods.Children.Clear();
            List<Goods> lstGoods = GoodsDP.Instance.ConverDBtoList();
            bool flag = false;
            int i = 1;
            foreach (var goods in lstGoods)
            {
                ControlGoods infgoods = new ControlGoods();
                flag = !flag;
                if (flag)
                {
                    infgoods.grdMain.Background = (Brush)new BrushConverter().ConvertFrom("#FFFFFFFF");
                }
                infgoods.txbId.Text = goods.IdGoods.ToString();
                infgoods.txbName.Text = goods.Name.ToString();
                infgoods.txbOrderNum.Text = i.ToString();
                infgoods.txbQuantity.Text = goods.Quantity.ToString();
                infgoods.txbUnit.Text = goods.Unit.ToString();
                infgoods.txbUnitPrice.Text = string.Format("{0:N0}", goods.UnitPrice);
                parameter.stkGoods.Children.Add(infgoods);
                i++;
            }

        }

        public void Openaddgoodswindow(HomeWindow parameter)
        {
            addgoods wdaddgoods = new addgoods();
            try
            {
                wdaddgoods.txtIdGoods.Text = (GoodsDP.Instance.GetMaxId() + 1).ToString();
            }
            catch
            {
                wdaddgoods.txtIdGoods.Text = 1.ToString();
            }
            wdaddgoods.txtName.Text = null;
            wdaddgoods.txtUnitPrice.Text = null;
            wdaddgoods.ShowDialog();
        }
    }
}
