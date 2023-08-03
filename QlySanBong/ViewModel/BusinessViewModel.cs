using QlySanBong.data_provier;
using QlySanBong.Model;
using QlySanBong.ResourceXAML;
using QlySanBong.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace QlySanBong.ViewModel
{
    class BusinessViewModel : BaseViewModel
    {
        //Biển để set validation
        public DateTime SetDateBooking { get; set; }
        public string TypeField { get; set; }
        public string TimeFrame { get; set; }
        public string Field { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Discount { get; set; }

        DateTime selectedDate;
        public HomeWindow home;
        private FieldButtonControl PickedField;
        private int currentPage;
        private int numberofFields;
        private int maxPage;
        public int CurrentPage { get => currentPage; set => currentPage = value; }
        public int NumberofFields { get => numberofFields; set => numberofFields = value; }
        public int MaxPage { get => maxPage; set => maxPage = value; }

        private ObservableCollection<FootballField> itemSourceField = new ObservableCollection<FootballField>();

        public ObservableCollection<FootballField> ItemSourceField { get => itemSourceField; set { itemSourceField = value; OnPropertyChanged(); } }

        public FootballField SelectedField { get => selectedField; set { selectedField = value; OnPropertyChanged("SelectedField"); } }

        private FootballField selectedField = new FootballField();

        private ObservableCollection<TimeFrame> itemSourceTimeFrame = new ObservableCollection<TimeFrame>();

        public ObservableCollection<TimeFrame> ItemSourceTimeFrame { get => itemSourceTimeFrame; set { itemSourceTimeFrame = value; OnPropertyChanged(); } }

        public TimeFrame SelectedFrame { get => selectedFrame; set { selectedFrame = value; OnPropertyChanged("SelectedFrame"); } }

        private TimeFrame selectedFrame = new TimeFrame();

        //HomeWindow
        public ICommand LoadFieldCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand LastPageCommand { get; set; }
        public ICommand PreviousPageCommand { get; set; }
        public ICommand FirstPageCommand { get; set; }
        public ICommand LoadTodayFieldCommand { get; set; }
        public ICommand PickFieldCommand { get; set; }
        public ICommand OpenBookingCommand { get; set; }
        //BookingWindow
        public ICommand LoadAllTimeFrameCommand { get; set; }
        public ICommand HireFieldCommand { get; set; }
        public ICommand LoadFieldTypeBKCommand { get; set; }
        public ICommand LoadFieldNameBKCommand { get; set; }
        public ICommand LoadTimeFrameBKCommand { get; set; }
        public ICommand LoadPriceBKCommand { get; set; }
        //CheckInWindow
        public ICommand ConfirmCommand { get; set; }
        public ICommand CheckInCommand { get; set; }
        public ICommand CancelFieldCommand { get; set; }
        public ICommand ChangeFieldCommand { get; set; }
        public ICommand LoadFieldTypeCICommand { get; set; }
        public ICommand LoadTimeFrameCICommand { get; set; }
        public ICommand LoadFieldNameCICommand { get; set; }
        public ICommand LoadPriceCICommand { get; set; }

        public BusinessViewModel(HomeWindow homeWindow)
        {
            LoadFieldsToView(homeWindow, currentPage * 6);
        }
        public BusinessViewModel()
        {
            currentPage = 0;
            PickFieldCommand = new RelayCommand<FieldButtonControl>((parameter) => true, (parameter) => OpenNewWindow(parameter));
            LoadFieldTypeBKCommand = new RelayCommand<bookingpitch>((parameter) => true, (parameter) => LoadFieldTypeBK(parameter));
            LoadAllTimeFrameCommand = new RelayCommand<ComboBox>((parameter) => true, (parameter) => LoadAllTimeFrame(parameter));
            PreviousPageCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => GoToPreviousPage(parameter));
            NextPageCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => GoToNextPage(parameter));
            LoadFieldCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => LoadFieldsToView(parameter, currentPage));
            OpenBookingCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => { OpenBookingWindow(parameter); LoadFieldsToView(parameter, currentPage * 6); });
            LoadTodayFieldCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => { this.home = parameter; LoadToday(parameter); });
            LoadTimeFrameBKCommand = new RelayCommand<bookingpitch>((parameter) => true, (parameter) =>
            {
                LoadTimeFrame(parameter.dpSetDate.SelectedDate.ToString()); parameter.cboTime.ItemsSource = itemSourceTimeFrame;
            });
            LoadFieldNameBKCommand = new RelayCommand<bookingpitch>((parameter) => true, (parameter) =>
            {
                if (parameter.cboTypeField.SelectedItem != null)
                {
                    LoadFieldName(parameter.cboTypeField.SelectedItem.ToString());
                    parameter.txbPrice.Text = "0";
                }
            });
            LoadPriceBKCommand = new RelayCommand<bookingpitch>((parameter) => true, (parameter) => LoadPriceBK(parameter));
            HireFieldCommand = new RelayCommand<bookingpitch>((parameter) => true, (parameter) => HireField(parameter));
            LoadFieldTypeCICommand = new RelayCommand<checkInbook>((parameter) => true, (parameter) =>
            {
                if (parameter != null)
                {
                    LoadFieldTypeCI(parameter);
                }
            });
            LoadFieldNameCICommand = new RelayCommand<checkInbook>((parameter) => true, (parameter) =>
            {
                if (parameter.cboTypeField.SelectedItem != null)
                {
                    LoadFieldName(parameter.cboTypeField.SelectedItem.ToString());
                    parameter.txbPrice.Text = "0";
                }
                parameter.cboPickField.ItemsSource = itemSourceField;
            });
            LoadPriceCICommand = new RelayCommand<checkInbook>((parameter) => true, (parameter) => LoadPriceCI(parameter));
            ConfirmCommand = new RelayCommand<checkInbook>((parameter) => true, (parameter) => ConfirmField(parameter));
            CancelFieldCommand = new RelayCommand<checkInbook>((parameter) => true, (parameter) => CancelField(parameter));
            ChangeFieldCommand = new RelayCommand<checkInbook>((parameter) => true, (parameter) => ChangeField(parameter));
            CheckInCommand = new RelayCommand<checkInbook>((parameter) => true, (parameter) => CheckInField(parameter));
        }


        public void GoToNextPage(HomeWindow homeWindow)
        {
            if (currentPage < maxPage)
            {
                currentPage++;
            }
            LoadFieldsToView(homeWindow, currentPage * 6); // Load lại
        }
        public void GoToPreviousPage(HomeWindow homeWindow)
        {
            if (currentPage > 0)
            {
                currentPage--;
            }
            LoadFieldsToView(homeWindow, currentPage * 6); // Load lại
        }
        public void CheckInField(checkInbook checkInWindow)
        {
            checkInWindow.cboPickField.IsEnabled = false;
            checkInWindow.cboTime.IsEnabled = false;
            checkInWindow.tbkCheckin.Text = "Check in";
            checkInWindow.dpSetDate.IsEnabled = false;
            checkInWindow.cboTypeField.IsEnabled = false;
            checkInWindow.changefield.Visibility = Visibility.Visible;
            checkInWindow.checkin.Visibility = Visibility.Hidden;
            checkInWindow.txbUserName.IsEnabled = false;
            checkInWindow.txbPhoneNumber.IsEnabled = false;
            checkInWindow.txtMoreInfo.IsEnabled = false;
        }
        public void ChangeField(checkInbook checkInWindow)
        {
            checkInWindow.cboPickField.IsEnabled = true;
            checkInWindow.cboTime.IsEnabled = true;
            checkInWindow.tbkCheckin.Text = "Đổi sân";
            checkInWindow.dpSetDate.IsEnabled = true;
            checkInWindow.cboTypeField.IsEnabled = true;
            checkInWindow.checkin.Visibility = Visibility.Visible;
            checkInWindow.changefield.Visibility = Visibility.Hidden;
            checkInWindow.txbUserName.IsEnabled = true;
            checkInWindow.txbPhoneNumber.IsEnabled = true;
            checkInWindow.txtMoreInfo.IsEnabled = true;
        }
        public void SetPickedDay(object sender, RoutedEventArgs e)
        {
            //Đặt cái Text có giá trị giống ngày được chọn
            currentPage = 0;
            DatePicker datePicker = sender as DatePicker;
            if (datePicker != null && datePicker.SelectedDate != null)
            {
                datePicker.Text = ((DateTime)datePicker.SelectedDate).ToString();
            }
            else
            {
                datePicker.Text = DateTime.Now.ToShortDateString();
            }
        }
        public void OpenBookingWindow(HomeWindow homeWindow)
        {
            bookingpitch bookingWindow = new bookingpitch();
            itemSourceTimeFrame.Clear();
            itemSourceField.Clear();
            bookingWindow.cboTime.ItemsSource = itemSourceTimeFrame;
            bookingWindow.cboPickField.ItemsSource = itemSourceField;
            bookingWindow.dpSetDate.IsEnabled = true;
            bookingWindow.cboTypeField.IsEnabled = true;
            bookingWindow.cboTime.IsEnabled = true;
            bookingWindow.cboPickField.IsEnabled = true;
            bookingWindow.dpSetDate.SelectedDate = null;
            bookingWindow.cboTypeField.Text = null;
            bookingWindow.cboTime.Text = null;
            bookingWindow.cboPickField.Text = null;
            bookingWindow.txtUserName.Text = null;
            bookingWindow.txbPhoneNumber.Text = null;
            bookingWindow.ShowDialog();
        }
        public void LoadToday(HomeWindow homeWindow)
        {
            //Load lúc mới open window
            currentPage = 0;
            if (homeWindow.dpPickedDate.Text != DateTime.Now.ToShortDateString())
            {
                homeWindow.dpPickedDate.Text = DateTime.Now.ToShortDateString();
            }
            LoadFieldsToView(homeWindow, currentPage * 6);
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(10)
            };
            timer.Tick += (s, e) =>
            {
                LoadFieldsToView(home, currentPage * 6);
            };
            timer.Start();
        }
        public void CancelField(checkInbook checkInWindow)
        {
            if (checkInWindow.tbkCheckin.Text == "Thanh toán")
            {
                checkInWindow.Close();
                return;
            }
            if (FieldInfoDP.Instance.DeleteFromDB(PickedField.txbidFieldInfo.Text))
            {
                //Hủy sân đã đặt
                MessageBox.Show("Hủy sân thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                checkInWindow.Close();
                PickedField.icn3.Visibility = Visibility.Hidden;
                if ((checkInWindow.dpSetDate.SelectedDate < DateTime.Today || 
                    (checkInWindow.dpSetDate.SelectedDate == DateTime.Today && 
                    string.Compare(selectedFrame.StartTime, DateTime.Now.ToString("HH:mm")) == -1)))
                {
                    PickedField.IsEnabled = false;
                    PickedField.bdrOut.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FFCDECDA");
                    PickedField.ToolTip = "Không thể đặt sân";
                }
                else
                {
                    PickedField.bdrOut.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF27AE60");
                    PickedField.ToolTip = "Đặt sân";
                }
                PickedField.icn1.Visibility = Visibility.Visible;
            }
        }
        public void ConfirmField(checkInbook checkInWindow)
        {
            if (checkInWindow.tbkCheckin.Text == "Thanh toán")
            {
                checkInWindow.txbIsPaid.Text = "1";
                MessageBox.Show("Thanh toán thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                checkInWindow.Close();
                return;
            }
            if (string.IsNullOrEmpty(checkInWindow.dpSetDate.Text) || checkInWindow.dpSetDate.SelectedDate < DateTime.Today)
            {
                checkInWindow.dpSetDate.Focus();
                return;
            }
            if (checkInWindow.cboTime.SelectedItem == null)
            {
                checkInWindow.cboTime.Focus();
                checkInWindow.cboTime.Text = "";
                return;
            }
            if (checkInWindow.dpSetDate.IsEnabled && (checkInWindow.dpSetDate.SelectedDate < DateTime.Today || 
                (checkInWindow.dpSetDate.SelectedDate == DateTime.Today && 
                string.Compare(selectedFrame.StartTime, DateTime.Now.ToString("HH:mm")) == -1)))
            {
                MessageBox.Show("Khung giờ đã qua!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                checkInWindow.dpSetDate.SelectedDate = null;
                checkInWindow.cboTime.SelectedItem = null;
                return;
            }
            if (!checkInWindow.dpSetDate.IsEnabled && (checkInWindow.dpSetDate.SelectedDate < DateTime.Today || 
                (checkInWindow.dpSetDate.SelectedDate == DateTime.Today && 
                string.Compare(selectedFrame.EndTime, DateTime.Now.ToString("HH:mm")) == -1)))
            {
                MessageBox.Show("Khung giờ đã qua!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                checkInWindow.dpSetDate.SelectedDate = null;
                checkInWindow.cboTime.SelectedItem = null;
                return;
            }
            if (checkInWindow.cboPickField.SelectedItem == null)
            {
                checkInWindow.cboPickField.Focus();
                checkInWindow.cboPickField.Text = "";
                return;
            }
            if (string.IsNullOrEmpty(checkInWindow.txbUserName.Text))
            {
                checkInWindow.txbUserName.Focus();
                checkInWindow.txbUserName.Text = "";
                return;
            }
            if (string.IsNullOrEmpty(checkInWindow.txbPhoneNumber.Text))
            {
                checkInWindow.txbPhoneNumber.Focus();
                checkInWindow.txbPhoneNumber.Text = "";
                return;
            }
            if (!Regex.IsMatch(checkInWindow.txbPhoneNumber.Text, @"^[0-9]+$"))
            {
                checkInWindow.txbPhoneNumber.Focus();
                return;
            }
            int status = 2;
            int idFieldInfo = int.Parse(checkInWindow.txbIdFieldInfo.Text);
            if (checkInWindow.cboTime.IsEnabled) // Đã chuyển sang đặt sân
            {
                status = 1;
            }

            FieldInfo fieldInfo = new FieldInfo(idFieldInfo, selectedField.IdField, 
                DateTime.Parse(checkInWindow.dpSetDate.Text + " " + selectedFrame.StartTime), 
                DateTime.Parse(checkInWindow.dpSetDate.Text + " " + selectedFrame.EndTime), 
                status, checkInWindow.txbPhoneNumber.Text, checkInWindow.txbUserName.Text, 
                checkInWindow.txtMoreInfo.Text, 0, ConvertToNumber(checkInWindow.txbPrice.Text));
            if (status == 2)
            {
                if (FieldInfoDP.Instance.UpdateOnDB(fieldInfo))
                {
                    //Chuyển sang sân đang đá
                    MessageBox.Show("Check in thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    PickedField.bdrOut.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF333333");
                    PickedField.icn3.Visibility = Visibility.Hidden;
                    PickedField.icn2.Visibility = Visibility.Visible;
                    PickedField.ToolTip = "Thanh toán";
                }
                else
                {
                    MessageBox.Show("Check in thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else //Nếu đang đặt sân
            {

                if (FieldInfoDP.Instance.DeleteFromDB(idFieldInfo.ToString()))
                {
                    fieldInfo.IdFieldInfo = FieldInfoDP.Instance.GetMaxIdFieldInfo() + 1;
                    if (FieldInfoDP.Instance.AddIntoDB(fieldInfo))
                    {
                        MessageBox.Show("Đổi sân thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        LoadFieldsToView(this.home, currentPage * 6);
                    }
                    else
                    {
                        MessageBox.Show("Đổi sân thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Đổi sân thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            checkInWindow.Close();
        }
        public void LoadPriceCI(checkInbook checkInWindow)
        {
            if (checkInWindow != null)
            {
                if (checkInWindow.cboTime.SelectedItem != null && checkInWindow.cboTypeField.SelectedItem != null && selectedFrame != null)
                {
                    string type = checkInWindow.cboTypeField.SelectedItem.ToString();
                    string[] temp = type.Split(' ');
                    if (temp.Length > 1)
                        type = temp[1];
                    string price = TimeFrameDP.Instance.GetPriceOfTimeFrame(selectedFrame.StartTime, selectedFrame.EndTime, type);
                    if (price != null)
                    {
                        checkInWindow.txbPrice.Text = string.Format("{0:N0}", long.Parse(price));
                    }
                }
                else
                {
                    checkInWindow.txbPrice.Text = "0";
                }
            }
        }
        public void LoadFieldTypeCI(checkInbook checkInWindow)
        {
            List<string> typesOfField = FootballFieldDP.Instance.GetFieldType();
            checkInWindow.cboTypeField.Items.Clear();
            foreach (var typeField in typesOfField)
            {
                checkInWindow.cboTypeField.Items.Add(typeField);
            }
        }
        public void HireField(bookingpitch bookingWindow)
        {
            if (bookingWindow.dpSetDate.SelectedDate == null || bookingWindow.dpSetDate.SelectedDate < DateTime.Today)
            {
                MessageBox.Show("Vui lòng chọn ngày đặt sân", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                bookingWindow.dpSetDate.Focus();
                return;
            }
            if (bookingWindow.cboTypeField.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn loại sân", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                bookingWindow.cboTypeField.Focus();
                bookingWindow.cboTypeField.Text = "";
                return;
            }
            if ((bookingWindow.dpSetDate.SelectedDate == DateTime.Today && 
                string.Compare(selectedFrame.StartTime, DateTime.Now.ToString("HH:mm")) == -1))
            {
                MessageBox.Show("Không thể đặt sân những giờ đã qua!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                bookingWindow.cboTime.SelectedItem = null;
                bookingWindow.cboTime.Focus();
                return;
            }
            if (bookingWindow.cboTime.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn khung giờ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                bookingWindow.cboTime.Focus();
                bookingWindow.cboTime.Text = "";
                return;
            }
            if (bookingWindow.cboPickField.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn sân", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                bookingWindow.cboPickField.Focus();
                bookingWindow.cboPickField.Text = "";
                return;
            }
            if (string.IsNullOrEmpty(bookingWindow.txtUserName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                bookingWindow.txtUserName.Focus();
                bookingWindow.txtUserName.Text = "";
                return;
            }
            if (string.IsNullOrEmpty(bookingWindow.txbPhoneNumber.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                bookingWindow.txbPhoneNumber.Focus();
                bookingWindow.txbPhoneNumber.Text = "";
                return;
            }
            if (!Regex.IsMatch(bookingWindow.txbPhoneNumber.Text, @"^[0-9]+$"))
            {
                bookingWindow.txbPhoneNumber.Focus();
                return;
            }
            int idFieldInfo = FieldInfoDP.Instance.GetMaxIdFieldInfo() + 1;
            FieldInfo fieldInfo = new FieldInfo(idFieldInfo, 
                selectedField.IdField, DateTime.Parse(bookingWindow.dpSetDate.Text + " " + selectedFrame.StartTime), 
                DateTime.Parse(bookingWindow.dpSetDate.Text + " " + selectedFrame.EndTime), 1, 
                bookingWindow.txbPhoneNumber.Text, bookingWindow.txtUserName.Text, 
                bookingWindow.txtMoreInfo.Text, 0, ConvertToNumber(bookingWindow.txbPrice.Text));

            if (FieldInfoDP.Instance.AddIntoDB(fieldInfo))
            {

                MessageBox.Show("Đặt sân thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                LoadFieldsToView(this.home, currentPage * 6);

                if (!bookingWindow.dpSetDate.IsEnabled)
                {
                    //Chuyển sang sân đã đặt
                    PickedField.bdrOut.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF1976D2");
                    PickedField.txbidFieldInfo.Text = idFieldInfo.ToString();
                    PickedField.icn1.Visibility = Visibility.Hidden;
                    PickedField.icn3.Visibility = Visibility.Visible;
                    PickedField.ToolTip = "Check in";
                }
            }
            else
            {
                MessageBox.Show("Đặt sân thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            bookingWindow.Close();
        }
        public void LoadPriceBK(bookingpitch bookingWindow)
        {
            if (bookingWindow != null)
            {
                if (bookingWindow.cboTime.SelectedItem != null && bookingWindow.cboTypeField.SelectedItem != null && bookingWindow.cboTime.SelectedItem != null && selectedFrame != null)
                {
                    string type = bookingWindow.cboTypeField.SelectedItem.ToString();
                    string[] temp = type.Split(' ');
                    if (temp.Length > 1)
                        type = temp[1];
                    string price = TimeFrameDP.Instance.GetPriceOfTimeFrame(selectedFrame.StartTime, selectedFrame.EndTime, type);
                    if (price != null)
                    {
                        bookingWindow.txbPrice.Text = string.Format("{0:N0}", long.Parse(price));
                    }
                }
                else
                {
                    bookingWindow.txbPrice.Text = "0";
                }
            }
        }
        public void LoadFieldName(string selectedType)
        {
            if (selectedType != "")
            {

                itemSourceField.Clear();
                string[] temp = selectedType.Split(' ');
                if (temp.Length > 1)
                    selectedType = temp[1];
                List<FootballField> fieldNames = new List<FootballField>();

                if (selectedFrame != null && selectedDate != null)
                {
                    fieldNames = FootballFieldDP.Instance.GetEmptyField(selectedType, selectedDate.ToShortDateString(), selectedFrame.StartTime, selectedFrame.EndTime);
                }
                foreach (var fieldName in fieldNames)
                {
                    itemSourceField.Add(fieldName);
                }
            }
        }
        public ObservableCollection<TimeFrame> LoadTimeFrame(string day)
        {
            List<TimeFrame> timeFrames;
            if (day != "")
            {
                selectedDate = DateTime.Parse(day);
                itemSourceTimeFrame.Clear();
                timeFrames = TimeFrameDP.Instance.GetTimeFrame();
                foreach (var timeFrame in timeFrames)
                {
                    itemSourceTimeFrame.Add(timeFrame);
                }
            }
            return itemSourceTimeFrame;
        }
        public void LoadAllTimeFrame(ComboBox comboBox)
        {
            Thread.Sleep(200);
            if (itemSourceTimeFrame == null)
            {
                List<TimeFrame> timeFrames = TimeFrameDP.Instance.GetTimeFrame();
                foreach (var timeFrame in timeFrames)
                {
                    itemSourceTimeFrame.Add(timeFrame);
                }
                comboBox.ItemsSource = itemSourceTimeFrame;
            }
        }
        public void LoadFieldTypeBK(bookingpitch bookingWindow)
        {
            List<string> typesOfField = FootballFieldDP.Instance.GetFieldType();
            bookingWindow.cboTypeField.Items.Clear();
            foreach (var typeField in typesOfField)
            {
                bookingWindow.cboTypeField.Items.Add(typeField);
            }
        }

        public void OpenNewWindow(FieldButtonControl fieldButtonControl)
        {
            PickedField = fieldButtonControl;
            if (fieldButtonControl.icn1.IsVisible)
            {
                //Sân trống
                bookingpitch bookingWindow = new bookingpitch();
                bookingWindow.txbidField.Text = fieldButtonControl.txbidField.Text;
                bookingWindow.dpSetDate.Text = fieldButtonControl.txbDay.Text;
                bookingWindow.cboTypeField.SelectedItem = fieldButtonControl.txbFieldType.Text;
                bookingWindow.txtUserName.Text = null;
                bookingWindow.txbPhoneNumber.Text = null;
                LoadTimeFrame(bookingWindow.dpSetDate.Text);
                for (int i = 0; i < itemSourceTimeFrame.ToList().Count; i++)
                {
                    if (fieldButtonControl.txbendTime.Text == itemSourceTimeFrame[i].EndTime && fieldButtonControl.txbstartTime.Text == itemSourceTimeFrame[i].StartTime)
                    {
                        bookingWindow.cboTime.SelectedItem = itemSourceTimeFrame[i];
                        break;
                    }
                }
                LoadFieldName(fieldButtonControl.txbFieldType.Text);
                for (int i = 0; i < itemSourceField.ToList().Count; i++)
                {
                    if (fieldButtonControl.txbidField.Text == itemSourceField[i].IdField.ToString() && fieldButtonControl.txbFieldType.Text == itemSourceField[i].Type.ToString())
                    {
                        bookingWindow.cboPickField.SelectedItem = itemSourceField[i];
                        break;
                    }
                }
                bookingWindow.txbPrice.Text = fieldButtonControl.txbPrice.Text;
                bookingWindow.ShowDialog();
                return;
            }
            if (fieldButtonControl.icn3.IsVisible || fieldButtonControl.icn2.IsVisible)
            {
                //Sân đã đặt 

                home = (HomeWindow)((Grid)((Grid)((Grid)((Grid)((Grid)((Grid)((ScrollViewer)((StackPanel)((FieldBookingControl)((Grid)((StackPanel)fieldButtonControl.Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent;
                checkInbook checkInWindow = new checkInbook();
                FieldInfo fieldInfo = FieldInfoDP.Instance.GetFieldInfo(PickedField.txbidFieldInfo.Text);
                checkInWindow.txbIdFieldInfo.Text = fieldButtonControl.txbidFieldInfo.Text;
                checkInWindow.dpSetDate.Text = fieldInfo.StartingTime.ToShortDateString();
                checkInWindow.cboTypeField.SelectedItem = fieldButtonControl.txbFieldType.Text;

                LoadTimeFrame(checkInWindow.dpSetDate.Text);
                for (int i = 0; i < itemSourceTimeFrame.ToList().Count; i++)
                {
                    if (fieldButtonControl.txbendTime.Text == itemSourceTimeFrame[i].EndTime && fieldButtonControl.txbstartTime.Text == itemSourceTimeFrame[i].StartTime)
                    {
                        checkInWindow.cboTime.SelectedItem = itemSourceTimeFrame[i];
                        break;
                    }
                }
                LoadFieldName(fieldButtonControl.txbFieldType.Text);
                SelectedField = new FootballField(int.Parse(fieldButtonControl.txbidField.Text), fieldButtonControl.txbFieldName.Text, int.Parse(fieldButtonControl.txbFieldType.Text), 0, " ", 0);
                itemSourceField.Add(SelectedField);
                itemSourceField = new ObservableCollection<FootballField>(itemSourceField.OrderBy(i => i.IdField));
                for (int i = 0; i < itemSourceField.ToList().Count; i++)
                {
                    if (fieldButtonControl.txbidField.Text == itemSourceField[i].IdField.ToString() && fieldButtonControl.txbFieldType.Text == itemSourceField[i].Type.ToString())
                    {
                        checkInWindow.cboPickField.SelectedItem = itemSourceField[i];
                        break;
                    }
                }
                checkInWindow.cboTime.ItemsSource = itemSourceTimeFrame;
                checkInWindow.cboPickField.ItemsSource = itemSourceField;
                checkInWindow.txbUserName.Text = fieldInfo.CustomerName;
                checkInWindow.txbPhoneNumber.Text = fieldInfo.PhoneNumber;
                checkInWindow.txbPhoneNumber.SelectionStart = checkInWindow.txbPhoneNumber.Text.Length;
                checkInWindow.txbPhoneNumber.SelectionLength = 0;
                checkInWindow.txtMoreInfo.Text = fieldInfo.Note;
                checkInWindow.txbPrice.Text = string.Format("{0:N0}", fieldInfo.Price);
                checkInWindow.cboTime.IsEnabled = false;
                if (fieldButtonControl.icn3.IsVisible)
                {
                    checkInWindow.tbkCheckin.Text = "Check in";
                    checkInWindow.txbConfirm.Content = "Xác nhận";
                    checkInWindow.txbCancel.Content = "Hủy sân";
                    checkInWindow.ShowDialog();
                }
                else //Sân đang đá
                {
                    checkInWindow.tbkCheckin.Text = "Thanh toán";
                    checkInWindow.txbConfirm.Content = "Thanh toán";
                    checkInWindow.txbCancel.Content = "Hủy";
                    checkInWindow.ShowDialog();
                    if (checkInWindow.txbIsPaid.Text == "1") // Thanh toán thành công!
                    {
                        fieldButtonControl.recColor.Visibility = Visibility.Visible;
                        fieldButtonControl.icn2.Visibility = Visibility.Hidden;
                        fieldButtonControl.icn4.Visibility = Visibility.Visible;
                        fieldInfo.Status = 3;
                        FieldInfoDP.Instance.UpdateOnDB(fieldInfo);
                        fieldButtonControl.ToolTip = "Đã thanh toán";
                    }
                }
                return;
            }
        }

        public void LoadFieldsToView(HomeWindow homeWindow, int firstField)
        {
            //Clear hết
            homeWindow.stkField.Children.Clear();
            homeWindow.stkFieldTitle.Children.Clear();

            List<TimeFrame> timeFrames = TimeFrameDP.Instance.GetTimeFrame();
            List<FieldInfo> fieldInfos;
            List<FootballField> footballFields = FootballFieldDP.Instance.ConvertDBToList();
            FieldBookingControl fieldBookingControl = new FieldBookingControl();

            int num = footballFields.Count();
            for (int i = 0; i < num; i++)
            {
                foreach (FootballField footballField in footballFields)
                {
                    if (footballField.Status == 0)
                    {
                        footballFields.Remove(footballField);
                        break;
                    }
                }
            }

            //Nếu bị null thì hiển thị ngày hiện tại
            try
            {
                fieldInfos = FieldInfoDP.Instance.QueryFieldInfoPerDay
                    (DateTime.Parse(homeWindow.dpPickedDate.Text).Year.ToString(), 
                    DateTime.Parse(homeWindow.dpPickedDate.Text).Month.ToString(), 
                    DateTime.Parse(homeWindow.dpPickedDate.Text).Day.ToString());
            }
            catch
            {
                fieldInfos = FieldInfoDP.Instance.QueryFieldInfoPerDay
                    (DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), 
                    DateTime.Now.Day.ToString());
            }

            //Tìm số lượng sân
            numberofFields = footballFields.Count;
            maxPage = numberofFields / 7;

            for (int i = firstField; i < firstField + 7 && i < numberofFields; i++) 
                // Load các tên sân ( 1 page có 7 sân)
            {
                FieldTitleControl fieldTitleControl = new FieldTitleControl();
                fieldTitleControl.txbName.Text = footballFields[i].Name;
                homeWindow.stkFieldTitle.Children.Add(fieldTitleControl);
            }


            foreach (var timeFrame in timeFrames) // load các khung giờ 
            {
                fieldBookingControl = new FieldBookingControl();
                fieldBookingControl.txbendTime.Text = timeFrame.EndTime;
                fieldBookingControl.txbstartTime.Text = timeFrame.StartTime;

                for (int i = firstField; i < firstField + 7 && i < numberofFields; i++) 
                    // Load button trong 1 khung giờ
                {
                    bool flag = false;
                    FieldButtonControl fieldButtonControl = new FieldButtonControl();

                    var fieldInfo = fieldInfos.Find(x => (x.IdField == footballFields[i].IdField && 
                    string.Compare(x.StartingTime.ToString("HH:mm"), fieldBookingControl.txbstartTime.Text) == 0));
                    //Kiểm tra FieldInfo nào tồn tại - sân trong giờ đã được sử dụng 
                    if (fieldInfo != null)
                    {
                        switch (fieldInfo.Status)
                        {
                            case 1:
                                fieldButtonControl.icn3.Visibility = Visibility.Visible; // Sân đã đặt
                                fieldButtonControl.bdrOut.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF1976D2");
                                fieldButtonControl.ToolTip = "Check In";
                                break;
                            case 2:
                                fieldButtonControl.bdrOut.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF333333");
                                fieldButtonControl.icn2.Visibility = Visibility.Visible; // Sân đang đá
                                fieldButtonControl.ToolTip = "Thanh toán";
                                break;
                            case 3:
                                fieldButtonControl.recColor.Visibility = Visibility.Visible;
                                fieldButtonControl.icn4.Visibility = Visibility.Visible; // Sân đã thanh toán
                                fieldButtonControl.ToolTip = "Đã thanh toán";
                                break;
                            default:

                                break;
                        }
                        flag = true;
                        fieldButtonControl.txbidFieldInfo.Text = fieldInfo.IdFieldInfo.ToString();
                        fieldInfos.Remove(fieldInfo);
                    }
                    //Những khung giờ đã qua thì icon X
                    if (!flag && (homeWindow.dpPickedDate.SelectedDate < DateTime.Today || 
                        (homeWindow.dpPickedDate.SelectedDate == DateTime.Today && 
                        string.Compare(fieldBookingControl.txbstartTime.Text, DateTime.Now.ToString("HH:mm")) == -1)))
                    {
                        flag = true;
                        //fieldButtonControl.icn5.Visibility = Visibility.Visible;
                        fieldButtonControl.icn1.Visibility = Visibility.Visible;
                        fieldButtonControl.ToolTip = "Không thể đặt sân";
                        fieldButtonControl.IsEnabled = false;
                        fieldButtonControl.bdrOut.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FFCDECDA");
                    }

                    //Nếu không có thì hiện icon còn trống
                    if (!flag)
                    {
                        fieldButtonControl.icn1.Visibility = Visibility.Visible;
                        fieldButtonControl.ToolTip = "Đặt sân";
                    }

                    //Lấy thông tin đặt sân đưa vào từng Button
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        fieldButtonControl.txbidField.Text = footballFields[i].IdField.ToString();
                        fieldButtonControl.txbstartTime.Text = fieldBookingControl.txbstartTime.Text;
                        fieldButtonControl.txbendTime.Text = fieldBookingControl.txbendTime.Text;
                        fieldButtonControl.txbDay.Text = homeWindow.dpPickedDate.Text;
                        fieldButtonControl.txbFieldType.Text = footballFields[i].Type.ToString();
                        string price = TimeFrameDP.Instance.GetPriceOfTimeFrame(fieldButtonControl.txbstartTime.Text, 
                            fieldButtonControl.txbendTime.Text, fieldButtonControl.txbFieldType.Text);
                        if (price != null)
                        {
                            fieldButtonControl.txbPrice.Text = string.Format("{0:N0}", long.Parse(price));
                        }
                        fieldButtonControl.txbFieldName.Text = footballFields[i].Name;
                    }));

                    fieldBookingControl.stkMain.Children.Add(fieldButtonControl);
                }

                homeWindow.stkField.Children.Add(fieldBookingControl);
            }

            //Xử lý các nút chuyển trang
            homeWindow.btnPreviousPage.IsEnabled = (currentPage != 0);
            homeWindow.btnNextPage.IsEnabled = (currentPage != maxPage);
        }
    }
}





