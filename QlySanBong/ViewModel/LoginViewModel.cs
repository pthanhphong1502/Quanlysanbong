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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace QlySanBong.ViewModel
{
    public class LoginViewModel:BaseViewModel
    {

        public ICommand OpenSignUpWindowCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }
        public ICommand LogInCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand SaveNewCommand { get; set; }
        public ICommand closeRegisterWindow { get; set; }
        public ICommand ShowIdAdmin { get; set; }
        public ICommand HideShowCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
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
            SaveNewCommand = new RelayCommand<forgotPass>((parameter) => true, (parameter) => SaveNewPassword(parameter));
            closeRegisterWindow = new RelayCommand<forgotPass>((parameter) => true, (parameter) => parameter.Close());
            ShowIdAdmin = new RelayCommand<forgotPass>((parameter) => true, (parameter) => ShowIdAdminWindow(parameter));
            HideShowCommand = new RelayCommand<loginWindow>((parameter) => true, (parameter) => HideShowCommandWindow(parameter));
            PasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => true, (parameter) => EncodingPassword(parameter));
        }

        private void HideShowCommandWindow(loginWindow parameter)
        {
            string s = parameter.pwPassword.Password.ToString();
            parameter.txtPass.Text = s;
            if (parameter.cbHideAppear.IsChecked == true)
            {
                parameter.txtPass.Text = parameter.pwPassword.Password;
                parameter.pwPassword.Visibility = Visibility.Collapsed;
                parameter.txtPass.Visibility = Visibility.Visible;
                return;

            }

            parameter.pwPassword.Password = parameter.txtPass.Text;
            parameter.pwPassword.Visibility = Visibility.Visible;
            parameter.txtPass.Visibility = Visibility.Collapsed;
        }

        private void ShowIdAdminWindow(forgotPass parameter)
        {
            if (parameter.pwIdAccount.Password == "1")
                parameter.grAccount.Visibility = Visibility.Visible;
            else
                parameter.grAccount.Visibility = Visibility.Hidden;
        }
        public void SaveNewPassword(forgotPass parameter)
        {
            string s = parameter.txAccount.Text.ToString();
            int d = int.Parse(parameter.pwIdAccount.Password);
            if (AccountDP.Instance.IsExistIdUserName(d))
            {
                if (AccountDP.Instance.IsExistUserName(s))
                {
                    if (d == 1)
                    {
                        
                        if (parameter.pwCheckIdAdMin.Password != "2002")
                        {
                            MessageBox.Show("Sai mã xác nhận dành riêng cho Admin", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                    }

                        if (AccountDP.Instance.UpdatePasswordByUsername(s, password))
                        {
                            MessageBox.Show("Đã cập nhật mật khẩu thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            parameter.Close();
                        }
                        else
                        {
                            MessageBox.Show("Cập nhật thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                }
                else
                {
                    MessageBox.Show("Đã nhập sai UserName", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }    
            }
            else
            {
                MessageBox.Show("Không tồn tại Account ! Hãy Đăng kí !", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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
            if (string.IsNullOrEmpty(parameter.pwPassword.Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.pwPassword.Focus();
                return;
            }
            foreach (var account in accounts)
            {
                if (account.Username == parameter.txtUsername.Text.ToString() && account.Password == password && account.Type != 3)
                {

                    CurrentAccount.Type = account.Type; // Kiểm tra quyền
                    CurrentAccount.IdAccount = account.IdAccount;
                    CurrentAccount.Password = password;
                    isLogin = true;
                    break;
                }
            }
            if (isLogin == true)
            {
                HomeWindow home = new HomeWindow();
                home.ucField1.icn1.Visibility = Visibility.Visible;
                home.ucField2.icn3.Visibility = Visibility.Visible;
                home.ucField1.btn.Cursor = null;
                home.ucField2.btn.Cursor = null;
                home.ucField3.btn.Cursor = null;
                home.ucField4.btn.Cursor = null;
                home.ucField2.bdrOut.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#92A8D1");
                home.ucField3.bdrOut.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF333333");
                home.ucField3.icn2.Visibility = Visibility.Visible;
                home.ucField4.recColor.Visibility = Visibility.Visible;
                home.ucField4.icn4.Visibility = Visibility.Visible;
                home.ucField5.icn1.Visibility = Visibility.Visible;
                home.ucField5.bdrOut.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FFCDECDA");
                CurrentAccount.DisplayName = parameter.txtUsername.Text;
                parameter.Hide();
                home.ShowDialog();
                parameter.pwPassword.Password = null;
                parameter.Show();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void OpenChangePasswordCommand(Window parameter)
        {
            forgotPass forgotpas = new forgotPass();
            parameter.Hide();
            forgotpas.ShowDialog();
            parameter.Show();
        }

        public void EncodingPassword(PasswordBox parameter)
        {

            this.password = parameter.Password;
            this.password = MD5Hash(this.password);
        }

    }
}
