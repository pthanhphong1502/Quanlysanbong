using QlySanBong.data_provier;
using QlySanBong.Model;
using QlySanBong.ResourceXAML;
using QlySanBong.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QlySanBong.ViewModel
{
    public class LoginViewModel:BaseViewModel
    {

        public ICommand OpenSignUpWindowCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }
        public ICommand LogInCommand { get; set; }
        private string password;
        public string Password { get => password; set { password = value; OnPropertyChanged(); } }
        private string userName;
        public string UserName { get => userName; set { userName = value; OnPropertyChanged(); } }
        private bool isLogin;
        public bool IsLogin { get => isLogin; set => isLogin = value; }
        public LoginViewModel()
        {
            OpenSignUpWindowCommand = new RelayCommand<Window>((parameter) => true, (parameter) => OpenSignUpWindow(parameter));
            ChangePasswordCommand = new RelayCommand<loginWindow>((parameter) => true, (parameter) => OpenChangePasswordCommand(parameter));
            LogInCommand = new RelayCommand<loginWindow>((parameter) => true, (parameter) => Login(parameter));
        }

        public void OpenSignUpWindow(Window parameter)
        {
            var signUp = new register();
            parameter.Hide();
            signUp.ShowDialog();
            parameter.Show();
        }

        public void Login(loginWindow parameter)
        {
            isLogin = false;
            if (parameter == null)
            {
                return;
            }
            
            List<Account> accounts = AccountDP.Instance.ConvertDBToList();
            //check username
            if (string.IsNullOrEmpty(parameter.txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtUsername.Focus();
                return;
            }
            //check password
            if (string.IsNullOrEmpty(parameter.txtPassword.Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtPassword.Focus();
                return;
            }
            foreach (var account in accounts)
            {
                if (account.Username == parameter.txtUsername.Text.ToString() && account.Password == parameter.txtPassword.Password.ToString() && account.Type != 3)
                {
                    CurrentAccount.Type = account.Type; // Kiểm tra quyền
                    if (CurrentAccount.Type != 0)
                    {
                    }
                    CurrentAccount.IdAccount = account.IdAccount;
                    CurrentAccount.Password = password;
                    isLogin = true;
                    break;
                }
            }
            if (isLogin == true)
            {
                HomeWindow home = new HomeWindow();
                // Gán thông tin cho các uc chú thích 
                home.ucField1.icn1.Visibility = Visibility.Visible;
                home.ucField2.icn3.Visibility = Visibility.Visible;
                home.ucField1.btn.Cursor = null;
                home.ucField2.btn.Cursor = null;
                home.ucField3.btn.Cursor = null;
                home.ucField4.btn.Cursor = null;
                home.ucField2.bdrOut.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF1976D2");
                home.ucField3.bdrOut.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF333333");
                home.ucField3.icn2.Visibility = Visibility.Visible;
                home.ucField4.recColor.Visibility = Visibility.Visible;
                home.ucField4.icn4.Visibility = Visibility.Visible;
                home.ucField5.icn1.Visibility = Visibility.Visible;
                home.ucField5.bdrOut.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FFCDECDA");
                //SetJurisdiction(home);
                DisplayAccount(home);
                parameter.Hide();
                home.ShowDialog();
                parameter.txtPassword.Password = null;
                parameter.Show();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void DisplayAccount(HomeWindow home)
        {
            if (CurrentAccount.Type != 0)
            {
                home.lbAccount.Content = CurrentAccount.DisplayName;// Hiển thị tên nhân viên
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bitmapImage = Converter.Instance.ConvertByteToBitmapImage(CurrentAccount.Image);
                imageBrush.ImageSource = bitmapImage;
                if (bitmapImage != null)
                    home.Imaaccount.Fill = imageBrush; // Hiển thị hình ảnh 
            }
        }
        //public void SetJurisdiction(HomeWindow home)
        //{
        //    if (CurrentAccount.Type != 0)
        //    {
        //        //Không cấp quyền cho nhân viên
        //        home.grdBody_Home.Visibility = Visibility.Hidden;
        //        home.grdBody_Business.Visibility = Visibility.Visible;
        //        home.btnEmployee.IsEnabled = false;
        //        home.btnReport.IsEnabled = false;
        //        home.btnAddGoods.IsEnabled = false;
        //        home.btnPaySalary.IsEnabled = false;
        //        home.btnSetSalary.IsEnabled = false;
        //        home.btnHome.IsEnabled = false;
        //        home.btnAddField.IsEnabled = false;
        //    }
        //    if (CurrentAccount.Type == 1)
        //    {
        //        home.btnAddGoods.IsEnabled = true;
        //        home.btnEmployee.IsEnabled = true;
        //    }
        //}

        public void OpenChangePasswordCommand(Window parameter)
        {
            forgotPass forgotpas = new forgotPass();
            parameter.Hide();
            forgotpas.ShowDialog();
            parameter.Show();
        }

    }
}
