using QlySanBong.data_provier;
using QlySanBong.Model;
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



namespace QlySanBong.ViewModel
{
    public class RegisterModel: BaseViewModel
    {
        public ICommand closeRegisterWindow { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand HideShowCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand PasswordConfirmChangedCommand { get; set; }
        private string password;
        public string Password { get => password; set { password = value; OnPropertyChanged(); } }
        private string passwordConfirm;
        public string PasswordConfirm { get => passwordConfirm; set { passwordConfirm = value; OnPropertyChanged(); } }


        public RegisterModel()
        {
            closeRegisterWindow = new RelayCommand<register>((parameter) => true, (parameter) => parameter.Close());
            PasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => true, (parameter) => EncodingPassword(parameter));
            PasswordConfirmChangedCommand = new RelayCommand<PasswordBox>((parameter) => true, (parameter) => EncodingConfirmPassword(parameter));
            RegisterCommand = new RelayCommand<register>((parameter) => true, (parameter) => registerWindow(parameter));
            HideShowCommand = new RelayCommand<register>((parameter) => true, (parameter) => HideShowWindow(parameter));
        }
        public void EncodingPassword(PasswordBox parameter)
        {
            
            this.password = parameter.Password;
            this.password = MD5Hash(this.password);
        }
        public void EncodingConfirmPassword(PasswordBox parameter)
        {
            this.passwordConfirm = parameter.Password;
            this.passwordConfirm = MD5Hash(this.passwordConfirm);
        }
        private void HideShowWindow(register parameter)
        {

            string s = parameter.pwPassword.Password.ToString();
            parameter.txbshowpassWord.Text = s;
            if(parameter.cbxHideAppear.IsChecked == true)
            {
                parameter.txbshowpassWord.Text = parameter.pwPassword.Password;
                parameter.pwPassword.Visibility = Visibility.Collapsed;
                parameter.txbshowpassWord.Visibility = Visibility.Visible;
                return;

            }
                parameter.pwPassword.Password = parameter.txbshowpassWord.Text;
                parameter.pwPassword.Visibility = Visibility.Visible;
                parameter.txbshowpassWord.Visibility = Visibility.Collapsed;
        }

        private void registerWindow(register parameter)
        {
            
            if(password != passwordConfirm)
            {
                MessageBox.Show("Xác nhận mật khẩu không trùng khớp", "Thông báo ", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.pwConfirmPass.Password = null;
                return ;
            }
            Account account = new Account();
            account.IdAccount = AccountDP.Instance.SetNewID();
            account.Username = parameter.txAccount.Text;
            account.Password = passwordConfirm;
            account.Type = 1;
            foreach (var acc in AccountDP.Instance.ConvertDBToList())
            {
                if (acc.Username == account.Username)
                {
                    MessageBox.Show("Trùng tên đăng nhập", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    parameter.txAccount.Focus();
                    parameter.txAccount.Text = "";
                    return;
                }
            }
            bool check = AccountDP.Instance.AddAccount(account);
            if (check)
            {
                MessageBox.Show("Đăng kí thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                parameter.txAccount.Text = "";
                parameter.pwPassword.Password = "";
                parameter.pwConfirmPass.Password = "";
                parameter.Close();
            }
            else
            {
                MessageBox.Show("Đăng kí không thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
