using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using QlySanBong.View;
using System.Windows.Controls;
using QlySanBong.ResourceXAML;
using QlySanBong.Model;
using QlySanBong.data_provier;
using System.Windows.Media.Imaging;

namespace QlySanBong.ViewModel
{
    public class HomeViewModel:BaseViewModel
    {

        public ICommand OpengrEmloyCommand { get; set; }
        public ICommand OpengrGoodsCommand { get; set; }
        public ICommand OpengrPitchCommand { get; set; }
        public ICommand OpengrBussinessCommand { get; set; }
        public ICommand OpengrHomeCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
       public ICommand loadedImageAndNameCommand { get; set; }
       
        public HomeViewModel homeVM;
        public HomeViewModel HomeVM { get => homeVM; set => homeVM = value; }

        public HomeViewModel()
        {
            OpengrEmloyCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => OpengrEmloywindow(parameter));
            OpengrGoodsCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => OpengrGoodswindow(parameter));
            OpengrPitchCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => OpengrPitchwindow(parameter));
            OpengrBussinessCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => OpengrBussinesswindow(parameter));
            OpengrHomeCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => OpengrHomewindow(parameter));
            LogOutCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => LogOutWindow(parameter));
            loadedImageAndNameCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => loadedImageAndNameWindow(parameter));
      
        }

        private void loadedImageAndNameWindow(HomeWindow parameter)
        {
            if (CurrentAccount.Type == 0)
            {
                parameter.recAccount.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Image/avatarsboss.jpeg")) };
                parameter.tbUserName.Text = "Chủ sân";
            }
            else
            {
                parameter.recAccount.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Image/employee.jpg")) };
                parameter.tbUserName.Text = "Nhân viên";
            }
        }

        private void LogOutWindow(HomeWindow parameter)
        {
            MessageBoxResult result = MessageBox.Show("Xác nhận thoát!", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                parameter.Close();
            }
            else
            {
                return;
            }    
        }


        private void OpengrPitchwindow(HomeWindow parameter)
        {
            parameter.grBody_Home.Visibility = Visibility.Hidden;
            parameter.grdBody_Business.Visibility = Visibility.Hidden;
            parameter.grdBody_Employee.Visibility = Visibility.Hidden;
            parameter.grdBody_Field.Visibility = Visibility.Visible;
            parameter.grdBody_Goods.Visibility = Visibility.Hidden;

            parameter.recPitch.Visibility = Visibility.Visible;
            parameter.recHome.Visibility = Visibility.Hidden;
            parameter.recBussiness.Visibility = Visibility.Hidden;
            parameter.recGoods.Visibility = Visibility.Hidden;
            parameter.recEmloy.Visibility = Visibility.Hidden;

            parameter.btnPitch.Foreground = (Brush)new BrushConverter().ConvertFrom("#FFFFFFFF");
            parameter.icPitch.Foreground = (Brush)new BrushConverter().ConvertFrom("#FFFFFFFF");

            parameter.btnHome.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icHome.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");

            parameter.btnbusiness.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icbusiness.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");

            parameter.btnEmloy.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icEmloy.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");

            parameter.btnGoods.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icGoods.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
        }

        private void OpengrHomewindow(HomeWindow parameter)
        {
            ReportViewModel reportViewModel = new ReportViewModel(parameter);
            parameter.grBody_Home.Visibility = Visibility.Visible;
            parameter.grdBody_Business.Visibility = Visibility.Hidden;
            parameter.grdBody_Employee.Visibility = Visibility.Hidden;
            parameter.grdBody_Field.Visibility =    Visibility.Hidden;
            parameter.grdBody_Goods.Visibility =    Visibility.Hidden;
            parameter.recPitch.Visibility = Visibility.Hidden;
            parameter.recHome.Visibility = Visibility.Visible;
            parameter.recBussiness.Visibility = Visibility.Hidden;
            parameter.recGoods.Visibility = Visibility.Hidden;
            parameter.recEmloy.Visibility = Visibility.Hidden;

            parameter.btnHome.Foreground = (Brush)new BrushConverter().ConvertFrom("#FFFFFFFF");
            parameter.icHome.Foreground = (Brush)new BrushConverter().ConvertFrom("#FFFFFFFF");

            parameter.btnbusiness.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");///black
            parameter.icbusiness.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");

            parameter.btnEmloy.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icEmloy.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");

            parameter.btnGoods.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icGoods.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");

            parameter.btnPitch.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icPitch.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
        }

        private void OpengrBussinesswindow(HomeWindow parameter)
        {
            BusinessViewModel businessViewModel = new BusinessViewModel(parameter);
            parameter.grBody_Home.Visibility = Visibility.Hidden;
            parameter.grdBody_Business.Visibility = Visibility.Visible;
            parameter.grdBody_Employee.Visibility = Visibility.Hidden;
            parameter.grdBody_Field.Visibility = Visibility.Hidden;
            parameter.grdBody_Goods.Visibility = Visibility.Hidden;
            parameter.recPitch.Visibility = Visibility.Hidden;
            parameter.recHome.Visibility = Visibility.Hidden;
            parameter.recBussiness.Visibility = Visibility.Visible;
            parameter.recGoods.Visibility = Visibility.Hidden;
            parameter.recEmloy.Visibility = Visibility.Hidden;

            parameter.btnHome.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icHome.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");

            parameter.btnbusiness.Foreground = (Brush)new BrushConverter().ConvertFrom("#FFFFFFFF");
            parameter.icbusiness.Foreground = (Brush)new BrushConverter().ConvertFrom("#FFFFFFFF");

            parameter.btnEmloy.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icEmloy.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");

            parameter.btnGoods.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icGoods.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");

            parameter.btnPitch.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icPitch.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");

        }

        private void OpengrGoodswindow(HomeWindow parameter)
        {
            parameter.grBody_Home.Visibility = Visibility.Hidden;
            parameter.grdBody_Business.Visibility = Visibility.Hidden;
            parameter.grdBody_Employee.Visibility = Visibility.Hidden;
            parameter.grdBody_Field.Visibility = Visibility.Hidden;
            parameter.grdBody_Goods.Visibility = Visibility.Visible;
            parameter.recPitch.Visibility = Visibility.Hidden;
            parameter.recHome.Visibility = Visibility.Hidden;
            parameter.recBussiness.Visibility = Visibility.Hidden;
            parameter.recGoods.Visibility = Visibility.Visible;
            parameter.recEmloy.Visibility = Visibility.Hidden;

            parameter.btnHome.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icHome.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");

            parameter.btnbusiness.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icbusiness.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");

            parameter.btnEmloy.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icEmloy.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");

            parameter.btnGoods.Foreground = (Brush)new BrushConverter().ConvertFrom("#FFFFFFFF");
            parameter.icGoods.Foreground = (Brush)new BrushConverter().ConvertFrom("#FFFFFFFF");

            parameter.btnPitch.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icPitch.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
        }

        private void OpengrEmloywindow(HomeWindow parameter)
        {
            parameter.grBody_Home.Visibility = Visibility.Hidden;
            parameter.grdBody_Business.Visibility = Visibility.Hidden;
            if (CurrentAccount.Type == 0)  parameter.grdBody_Employee.Visibility = Visibility.Visible;
            parameter.grdBody_Field.Visibility = Visibility.Hidden;
            parameter.grdBody_Goods.Visibility = Visibility.Hidden;

            parameter.recPitch.Visibility = Visibility.Hidden;
            parameter.recHome.Visibility = Visibility.Hidden;
            parameter.recBussiness.Visibility = Visibility.Hidden;
            parameter.recGoods.Visibility = Visibility.Hidden;
            parameter.recEmloy.Visibility = Visibility.Visible;

            parameter.btnEmloy.Foreground = (Brush)new BrushConverter().ConvertFrom("#FFFFFFFF");
            parameter.icEmloy.Foreground = (Brush)new BrushConverter().ConvertFrom("#FFFFFFFF");

            parameter.btnPitch.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icPitch.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");

            parameter.btnGoods.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icGoods.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");

            parameter.btnHome.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icHome.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");

            parameter.btnbusiness.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
            parameter.icbusiness.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF000000");
        }
    }
}
