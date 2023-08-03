using Microsoft.Win32;
using QlySanBong.data_provier;
using QlySanBong.Model;
using QlySanBong.ResourceXAML;
using QlySanBong.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QlySanBong.ViewModel
{
    public class EmployViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public string slaryBase { get; set; }
        public string MoneyPerShift { get; set; }
        public string MoneyPerFault { get; set; }
        public string StandarWorkDays { get; set; }
        public string SlaryMonth { get; set; }
        private string imagefilename;
        public string sex;
        public string id;
        public string Id { get => id; set => id = value; }
        private HomeWindow homewd;
        public HomeWindow Homewd { get => homewd; set => homewd = value; }
        private StackPanel stack = new StackPanel();
        public string SelectedMonth { get => selectedMonth; set { selectedMonth = value; OnPropertyChanged(); } }


        private string selectedMonth;

        public ICommand SeparateThousandsCommand { get; set; }

        public ICommand ExitCommand { get; set; }
        public ICommand OpenAddEmloyCommand { get; set; }
        public ICommand LoadEmployeesToViewCommand { get; set; }
        //AddemloyCommand
        public ICommand CancelCommand { get; set; }
        public ICommand SaveEmloyeeCommand { get; set; }
        public ICommand SelectImage { get; set; }
        public ICommand SetSalaryCommand { get; set; }

        //setsalary
        public ICommand BackfromSetSalaryCommand { get; set; }
        public ICommand SetSalaryBaseCommand { get; set; }
        public ICommand SelectionChangeCommand { get; set; }
        public ICommand PaySalaryCommand { get; set; }


        //EmloyControl
        public ICommand ValueChangedCommand { get; set; }
        public ICommand CaculateSalaryCommand { get; set; }

        public ICommand DeleteCommand{ get; set; }
        public ICommand UpdateCommand { get; set; }

        public EmployViewModel()
        {
            SeparateThousandsCommand = new RelayCommand<TextBox>((parameter) => true, (parameter) => SeparateThousands(parameter));
            SelectImage = new RelayCommand<Grid>((parameter) => true, (parameter) => SelectImageWindow(parameter));
            SaveEmloyeeCommand = new RelayCommand<addemployee>((parameter) => true, (parameter) => SaveWindow(parameter));
            ExitCommand = new RelayCommand<addemployee>((parameter) => true, (parameter) => parameter.Close());
            OpenAddEmloyCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => OpenAddEmployWindow(parameter));
            LoadEmployeesToViewCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => LoadEmployeesToView(parameter));
            SetSalaryCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => OpensetsalaryWindow(parameter));
            BackfromSetSalaryCommand = new RelayCommand<setsalary>((parameter) => true, (parameter) => parameter.Close());
            SetSalaryBaseCommand = new RelayCommand<setsalary>((parameter) => true, (parameter) => setsalarybaseWindow(parameter));
            SelectionChangeCommand = new RelayCommand<setsalary>((parameter) => true, (parameter) => SelectionChangeCommandWindow(parameter));
            ValueChangedCommand = new RelayCommand<EmployControl>((parameter) => true, (parameter) => ValueChangedWindow(parameter));
            CaculateSalaryCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => CaculateSalaryWindow(parameter));
            UpdateCommand = new RelayCommand<EmployControl>((parameter) => true, (parameter) => UpdateEmployWWindow(parameter));
            DeleteCommand = new RelayCommand<EmployControl>((parameter) => true, (parameter) => DeleteEmployWWindow(parameter));
            PaySalaryCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => PaySalaryWindow(parameter));
    
        }

        private void PaySalaryWindow(HomeWindow parameter)
        {

            if (SalarySettingDP.Instance.ConvertDBToList().Count == 0)
            {
                MessageBox.Show("Chưa thiết lập lương! Mời thực hiện", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                setsalary wdSetSalary = new setsalary();
                wdSetSalary.ShowDialog();
                return;
            }
            if (parameter.stkEmployee.Children.Count <= 0) return;
            bool success = true;
            int idSalaryRecord = SalaryRecordDP.Instance.SetID();
            long total = -1;
            for (int i = 0; i < homewd.stkEmployee.Children.Count; i++)
            {
                EmployControl control = (EmployControl)parameter.stkEmployee.Children[i];
                total += ConvertToNumber(control.txbTotalSalary.Text);
                Employee employee = EmployDP.Instance.GetEmployeeByIdEmployee(control.txbId.Text);
                employee.Startingdate = DateTime.Now;
                EmployDP.Instance.UpdateOnDB(employee);
            }
            if (idSalaryRecord != -1 && total != -1)
            {
                SalaryRecord salaryRecord = new SalaryRecord(idSalaryRecord, DateTime.Now, total, CurrentAccount.IdAccount);
                if (!SalaryRecordDP.Instance.AddIntoDB(salaryRecord))
                {
                    success = false;
                }
            if (!SalaryDP.Instance.UpdateIdSalaryRecord(idSalaryRecord, DateTime.Now.Month, DateTime.Now.Year))
            {
                success = false;
            }
            }
            else
            {
                success = false;
            }
            if (success)
            {
                MessageBox.Show("Trả lương thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                parameter.TxbTongluong.Text = "0";
                LoadEmployeesToView(homewd);
            }
            else
            {
                MessageBox.Show("Trả lương thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        FrameworkElement GetWindowParent(UserControl p)
        {
            FrameworkElement parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }

            return parent;
        }

        private void DeleteEmployWWindow(EmployControl parameter)
        {

            MessageBoxResult result = MessageBox.Show("Xác nhận xóa nhân viên?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Employee employee = EmployDP.Instance.GetEmployeeByIdEmployee(parameter.txbId.Text);
                Account account = new Account(employee.IdAccount, "", "", 3);
                employee.IsDeleted = 1;
                //HomeWindow homeWindow = (HomeWindow)(((Grid)((Grid)((Grid)((Grid)((Grid)((ScrollViewer)((StackPanel)(parameter.Parent))
                //.Parent).Parent).Parent).Parent).Parent).Parent).Parent);
                FrameworkElement homewindow = GetWindowParent(parameter);
                var hw = homewindow as HomeWindow;
                if (EmployDP.Instance.UpdateOnDB(employee) && (employee.IdAccount == -1 || AccountDP.Instance.UpdateType(account)))
                {
                    hw.stkEmployee.Children.Remove(parameter);
                    bool flag = false;
                    for (int i = 0; i < hw.stkEmployee.Children.Count; i++)
                    {
                        EmployControl temp = (EmployControl)hw.stkEmployee.Children[i];
                        flag = !flag;
                        if (flag)
                        {
                            temp.grdMain.Background = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                        }
                        else
                        {
                            temp.grdMain.Background = (Brush)new BrushConverter().ConvertFromString("#F4EEFF");
                        }
                        temp.txbSerial.Text = (i + 1).ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Xoá thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void UpdateEmployWWindow(EmployControl parameter)
        {
            Employee employee = EmployDP.Instance.GetEmployeeByIdEmployee(parameter.txbId.Text);
            addemployee child = new addemployee();
            if (employee.IdEmployee.ToString() == parameter.txbId.Text)
            {
                child.txtIDEmployee.Text = employee.IdEmployee.ToString();

                child.txtName.Text = employee.Name;
                child.txtName.SelectionStart = child.txtName.Text.Length;
                child.txtName.SelectionLength = 0;

                child.txtTelephoneNumber.Text = employee.Phonenumber;
                child.txtTelephoneNumber.SelectionStart = child.txtTelephoneNumber.Text.Length;
                child.txtTelephoneNumber.SelectionLength = 0;

                child.txtAddress.Text = employee.Address;
                child.txtAddress.SelectionStart = child.txtAddress.Text.Length;
                child.txtAddress.SelectionLength = 0;

                child.cboPosition.Text = employee.Position;

                if (employee.Gender == "Nam")
                    child.rdoMale.IsChecked = true;
                else
                    child.rdoFemale.IsChecked = true;
                child.dpBirthDate.SelectedDate = DateTime.Parse(employee.DateOfBirth.ToString());
                child.dpWorkDate.SelectedDate = DateTime.Parse(employee.Startingdate.ToString());
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = Converter.Instance.ConvertByteToBitmapImage(employee.ImageFile);
                child.grdSelectImage.Background = imageBrush;
                if (child.grdSelectImage.Children.Count > 1)
                {
                    child.grdSelectImage.Children.Remove(child.grdSelectImage.Children[0]);
                    child.grdSelectImage.Children.Remove(child.grdSelectImage.Children[1]);
                }
            }
            child.btnSave.ToolTip = "Cập nhật thông tin nhân viên";
            child.Title = "Cập nhật thông tin nhân viên";
            child.ShowDialog();
            Employee fixedEmployee = EmployDP.Instance.GetEmployeeByIdEmployee(parameter.txbId.Text);
            parameter.txbName.Text = fixedEmployee.Name;
            parameter.txbPosition.Text = fixedEmployee.Position;
        }

        private void CaculateSalaryWindow(HomeWindow parameter)
        {

            if (parameter.stkEmployee.Children.Count <= 0)
            {
                MessageBox.Show("Hiện không có nhân viên", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            foreach (string item in EmployDP.Instance.GetAllPosition())
            {
                SalarySetting salarySetting = SalarySettingDP.Instance.GetSalarySettings(item);
                if(salarySetting == null)
                {
                    MessageBox.Show("Vui lòng thiết lập lương cho ''" + item + " ''! ", "Thống báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    setsalary setsalary = new setsalary();
                    setsalary.cbbEmloy.Text = item;
                    setsalary.ShowDialog();
                    return;
                }
            }
            bool success = false;
            foreach(var salary in SalaryDP.Instance.GetSalaryByMonth(DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString()))
            {
                int DayExtra=0;
                string id = salary.IdEmployee.ToString();
                Employee emloy = EmployDP.Instance.GetEmployeeByIdEmployee(id);
                int workday = (int) double.Parse((DateTime.Now-emloy.Startingdate ).TotalDays.ToString());
                string positonemploy = EmployDP.Instance.GetPosition(salary.IdEmployee.ToString());
                SalarySetting setting = SalarySettingDP.Instance.GetSalarySettings(positonemploy);
                if (emloy.IsDeleted != 1)
                {
                    if (workday > setting.StandardWorkDays)
                    {
                        DayExtra = workday - setting.StandardWorkDays;
                        
                        salary.TotalSalary = setting.SalaryBase + (setting.SalaryBase / setting.StandardWorkDays) * DayExtra;
                    }
                    else
                    if (workday < setting.StandardWorkDays)
                    {
                        salary.TotalSalary = (setting.SalaryBase / setting.StandardWorkDays) * workday;
                    }
                    else
                    if (workday == setting.StandardWorkDays)
                    {
                        salary.TotalSalary = setting.SalaryBase;
                    }

                    salary.TotalSalary = salary.TotalSalary - (salary.NumOfFault * setting.MoneyPerFault) + (salary.NumOfShift * setting.MoneyPerShift);
                    if (SalaryDP.Instance.UpdateTotalSalary(salary))
                    {
                        success = true;
                    }
                }
            }
            if (success) {
                MessageBox.Show("Tính lương thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                long tongluong = 0; ;
                long t;
                    for (int i = 0; i < parameter.stkEmployee.Children.Count; i++)
                    {
                        EmployControl control = (EmployControl)parameter.stkEmployee.Children[i];
                         t = ConvertToNumber(control.txbTotalSalary.Text.ToString());
                        control.txbTotalSalary.Text = string.Format("{0:N0}",SalaryDP.Instance.GetTotalSalary(
                            control.txbId.Text,DateTime.Now.Month.ToString(),DateTime.Now.Year.ToString()));
                        tongluong += SalaryDP.Instance.GetTotalSalary(control.txbId.Text, DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString());
                    }
                homewd.TxbTongluong.Text =string.Format("{0:N0}" , tongluong);
            }
            else
            {
                MessageBox.Show("Tính lương thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void ValueChangedWindow(EmployControl parameter)
        {
            Salary salary = new Salary();
            salary.NumOfFault = int.Parse(parameter.nsNumOfFault.Text.ToString());
            salary.NumOfShift = int.Parse(parameter.nsNumOfShift.Text.ToString());
            salary.IdEmployee = int.Parse(parameter.txbId.Text);
            salary.SalaryMonth = DateTime.Now;
            SalaryDP.Instance.UpdateQuantity(salary);
        }

        private void SelectionChangeCommandWindow(setsalary parameter)
        {
            ComboBoxItem cbI = (ComboBoxItem)parameter.cbbEmloy.SelectedItem;
            SalarySetting salarySetting = SalarySettingDP.Instance.GetSalarySettings(cbI.Content.ToString());
            if (salarySetting != null)
            {
                parameter.cbbSalaryperMonth.Text = salarySetting.SalaryBase.ToString();
                parameter.txbNumofWordDay.Text = salarySetting.StandardWorkDays.ToString();
                parameter.txbMoneybonus.Text = salarySetting.MoneyPerShift.ToString();
                parameter.txbMoneyError.Text = salarySetting.MoneyPerFault.ToString();
            }
            else
            {
                parameter.cbbSalaryperMonth.Text = null;
                parameter.txbNumofWordDay.Text = null;
                parameter.txbMoneybonus.Text = null;
                parameter.txbMoneyError.Text = null;
            }
        }

        private void setsalarybaseWindow(setsalary parameter)
        {
            if (parameter == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(parameter.cbbEmloy.Text))
            {
                parameter.cbbEmloy.Focus();
                return;
            }
            if (string.IsNullOrEmpty(parameter.cbbSalaryperMonth.Text))
            {
                parameter.cbbSalaryperMonth.Focus();
                parameter.cbbSalaryperMonth.Text = "";
                return;
            }
            if((!Regex.IsMatch(parameter.txbNumofWordDay.Text,@"^[0-9]+$") || 
                (int.Parse(parameter.txbNumofWordDay.Text) <1) || (int.Parse(parameter.txbNumofWordDay.Text) > 30)))
            {
                parameter.txbNumofWordDay.Focus();
                return;
            }
            if (string.IsNullOrEmpty(parameter.txbMoneybonus.Text))
            {
                parameter.txbMoneybonus.Focus();
                parameter.txbMoneybonus.Text ="";
                return;
            }
            if (string.IsNullOrEmpty(parameter.txbMoneyError.Text))
            {
                parameter.txbMoneyError.Focus();
                parameter.txbMoneyError.Text ="";
                return;
            }
            SalarySetting setting = new SalarySetting(ConvertToNumber(parameter.cbbSalaryperMonth.Text), 
                ConvertToNumber(parameter.txbMoneybonus.Text), ConvertToNumber(parameter.txbMoneyError.Text),
                parameter.cbbEmloy.Text, int.Parse(parameter.txbNumofWordDay.Text));

            bool exsit = false;
            if (SalarySettingDP.Instance.GetSalarySettings(parameter.cbbEmloy.Text) != null)
            {
                exsit = !exsit;
                if (SalarySettingDP.Instance.UpdateDB(setting))
                {
                    MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            if (!exsit)
            {
                if (SalarySettingDP.Instance.AddIntoDB(setting))
                {
                    MessageBox.Show("Đã thiết lập lương cho '' " + setting.TypeEmployee + "'' thành công ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                {
                    MessageBox.Show("Thiết lập lương cho '' " + setting.TypeEmployee + "'' đã thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OpensetsalaryWindow(HomeWindow parameter)
        {
            setsalary ssalary = new setsalary();
            ssalary.ShowDialog();
        }

        private void OpenAddEmployWindow(HomeWindow parameter)
        {
            stack = parameter.stkEmployee;
            addemployee addEmployee = new addemployee();
            try
            {
                addEmployee.txtIDEmployee.Text = (EmployDP.Instance.GetMaxIdEmployee() + 1).ToString();
            }
            catch
            {
                addEmployee.txtIDEmployee.Text = "1";
            }
            addEmployee.btnSave.Content = "Thêm";
            if (CurrentAccount.Type == 1)
                addEmployee.cboPositionManage.IsEnabled = false;

            addEmployee.txtName.Text = null;
            addEmployee.txtAddress.Text = null;
            addEmployee.txtTelephoneNumber.Text = null;
            addEmployee.ShowDialog();
            if (addEmployee.IsAdded.Text == "1")
            {
                LoadEmployeesToView(parameter);
            }
        }

        private void LoadEmployeesToView(HomeWindow parameter)
        {
            this.homewd = parameter;
            int i = 1;
            parameter.stkEmployee.Children.Clear();
            bool flag = false;
            List<Salary> salaries = SalaryDP.Instance.GetSalaryByMonth(DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString());
            foreach (var employee in EmployDP.Instance.ConvertDBToList())
            {
                EmployControl temp = new EmployControl();
                flag = !flag;
                if (flag)
                {
                    temp.grdMain.Background = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                }
                temp.txbSerial.Text = i.ToString();
                i++;
                string total ="0";
                // load number fault and overtime and salary
                if (salaries.Count < 1)
                {
                    Salary tmp = new Salary();
                    tmp.IdEmployee = employee.IdEmployee;
                    tmp.NumOfFault = 0;
                    tmp.NumOfShift = 0;
                    tmp.TotalSalary = -1;
                    tmp.SalaryMonth = DateTime.Now.Date;
                    SalaryDP.Instance.AddIntoDB(tmp);
                    temp.nsNumOfShift.Text = 0.ToString();
                    temp.nsNumOfFault.Text = 0.ToString();
                    temp.txbTotalSalary.Text = total;
                }
                foreach (var salary in SalaryDP.Instance.GetUnPaidSalary(employee.IdEmployee.ToString(), 
                    DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString()))
                {
                    if (employee.IdEmployee == salary.IdEmployee && salary.IdSalaryRecord == -1)
                    {
                        temp.nsNumOfShift.IsEnabled = true;
                        temp.nsNumOfFault.IsEnabled = true;
                        temp.nsNumOfShift.Text = salary.NumOfShift.ToString();
                        temp.nsNumOfFault.Text = salary.NumOfFault.ToString();

                        if (salary.TotalSalary == -1)
                        {
                            temp.txbTotalSalary.Text = total;
                        }
                        else
                        {
                            temp.txbTotalSalary.Text = string.Format("{0:n0}", total);
                        }
                        break;
                    }
                }
                temp.txbId.Text = employee.IdEmployee.ToString();
                temp.txbName.Text = employee.Name.ToString();
                temp.txbPosition.Text = employee.Position.ToString();
                if (CurrentAccount.Type == 1)
                {
                        temp.btnEditEmployee.IsEnabled = false;
                }
                parameter.stkEmployee.Children.Add(temp);
            }
        }

        private void SetSalary(addemployee parameter)
        {
            if (!SalaryDP.Instance.IsExist(parameter.txtIDEmployee.Text, DateTime.Now))
            {
                Salary salary = new Salary();
                salary.IdEmployee = int.Parse(parameter.txtIDEmployee.Text);
                salary.NumOfFault = 0;
                salary.NumOfShift = 0;
                salary.TotalSalary = 0;
                salary.SalaryMonth = DateTime.Now;
                if (!SalaryDP.Instance.IsExistIdSalaryRecord(DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString()))
                {
                    SalaryDP.Instance.AddIntoDB(salary);
                }
            }
        }

        private void SaveWindow(addemployee parameter)
        {
            if (string.IsNullOrEmpty(parameter.txtName.Text))
            {
                parameter.txtName.Text = "";
                parameter.txtName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(parameter.cboPosition.Text))
            {
                parameter.cboPosition.Focus();
            }
            if (parameter.dpBirthDate.Text == "")
            {
                parameter.dpBirthDate.Focus();
                return;
            }
            else
            {
                DateTime time = new DateTime();
                if (!DateTime.TryParse(parameter.dpBirthDate.Text, out time))
                {
                    MessageBox.Show("Vui lòng nhập lại ngày sinh!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    parameter.dpBirthDate.Focus();
                    return;
                }
            }
            if (parameter.txtAddress.Text == "")
            {
                parameter.txtAddress.Focus();
                parameter.txtAddress.Text = "";
                return;
            }
            if (parameter.txtTelephoneNumber.Text == "" || !Regex.IsMatch(parameter.txtTelephoneNumber.Text, @"^[0-9]+$"))
            {
                parameter.txtTelephoneNumber.Focus();
                parameter.txtTelephoneNumber.Text = "";
                return;
            }
            else
            {
                DateTime time = new DateTime();
                if (!DateTime.TryParse(parameter.dpWorkDate.Text, out time))
                {
                    MessageBox.Show("Vui lòng nhập lại ngày vào làm!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                if (time <= DateTime.Parse(parameter.dpBirthDate.Text))
                {
                    MessageBox.Show("Vui lòng nhập ngày vào làm lớn hơn ngày sinh!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    parameter.dpWorkDate.Focus();
                    parameter.dpWorkDate.Text = null;
                    return;
                }
                int result = DateTime.Compare(time, DateTime.Now);
                if (result >0 )
                {
                    MessageBox.Show("Vui lòng nhập ngày vào làm phải nhỏ hơn ngày hiện tại","Thông báo",MessageBoxButton.OK,MessageBoxImage.Error);
                    return;
                }
            }
            if (parameter.rdoMale.IsChecked.Value == true)
            {
                sex = "Nam";
            }
            else sex = "Nữ";
            if (parameter.grdSelectImage.Background == null)
            {
                MessageBox.Show("Vui lòng thêm hình ảnh!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            byte[] imagebyte;
            try
            {
                imagebyte = Converter.Instance.ConvertImageToBytes(imagefilename);
            }
            catch
            {
                imagebyte = EmployDP.Instance.GetEmployeeByIdEmployee(parameter.txtIDEmployee.Text).ImageFile;
            }
            //imagefilename = null;
            Employee employee = new Employee(int.Parse(parameter.txtIDEmployee.Text), 
                parameter.txtName.Text, sex, parameter.txtTelephoneNumber.Text, parameter.txtAddress.Text,
                DateTime.Parse(parameter.dpBirthDate.Text), parameter.cboPosition.Text, DateTime.Parse(parameter.dpWorkDate.Text), -1, imagebyte, 0);
            Employee current = EmployDP.Instance.GetEmployeeByIdEmployee(parameter.txtIDEmployee.Text);
            if (current != null && current.IdAccount != -1)
            {
                if (employee.Position == "Nhân viên")
                {
                    AccountDP.Instance.UpdateType(new Account(current.IdAccount, "", "", 1));
                }
                if (employee.Position == "Bảo vệ")
                {
                    int t = current.IdAccount;
                    current.IdAccount = -1;
                    EmployDP.Instance.UpdateIdAccount(current);
                    AccountDP.Instance.DeleteAcount(t.ToString());
                }
            }
            EmployDP.Instance.AddEmployee(employee);
            SetSalary(parameter);
            parameter.IsAdded.Text = "-1";
            parameter.Close();
            LoadEmployeesToView(homewd);
        }

        private void SelectImageWindow(Grid parameter)
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
    }
}
