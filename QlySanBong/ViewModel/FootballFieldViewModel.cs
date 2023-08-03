using QlySanBong.View;
using QlySanBong.Model;
using QlySanBong.data_provier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using QlySanBong.ResourceXAML;

namespace QlySanBong.ViewModel
{
    class FootballFieldViewModel : BaseViewModel
    {
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public ICommand DeleteListFieldCommand { get; set; }
        public ICommand EditListFieldCommand { get; set; }
        //AddFootballFieldWindow
        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand SeparateThousandsCommand { get; set; }
        public ICommand SelectionFieldTypeCommand { get; set; } // chọn loại sân trong window add field
        public ICommand LostFocusCommand { get; set; }
        //Field Control
        public ICommand HoverCommand { get; set; }
        public ICommand LeaveCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        public ICommand EditCardFieldCommand { get; set; }
        public ICommand DeleteCardFieldCommand { get; set; }

        //Home Window
        public ICommand SelectionChangedCommand { get; set; } // thay đổi view
        public ICommand AddFieldCommand { get; set; }
        public ICommand SetTimeFrameCommand { get; set; }
        public ICommand OpenReportFieldCommand { get; set; }
        // ReportFieldWindow
        public ICommand ReportFieldCommand { get; set; }
        public ICommand LoadFieldCommand { get; set; }

        private ObservableCollection<string> itemSourceField = new ObservableCollection<string>();
        public ObservableCollection<string> ItemSourceField { get => itemSourceField; set { itemSourceField = value; OnPropertyChanged(); } }
        private ObservableCollection<FootballField> itemSourceFieldName = new ObservableCollection<FootballField>();
        public ObservableCollection<FootballField> ItemSourceFieldName { get => itemSourceFieldName; set { itemSourceFieldName = value; OnPropertyChanged(); } }
        private HomeWindow home;
        public HomeWindow Home { get => home; set => home = value; }
        public FootballField SelectedField { get => selectedField; set { selectedField = value; OnPropertyChanged("SelectedField"); } }

        private FootballField selectedField = new FootballField();
        private FieldDetailsControl detailsField;
        public FieldDetailsControl DetailsField { get => detailsField; set => detailsField = value; }
        private FieldControl cardField;
        public FieldControl CardField { get => cardField; set => cardField = value; }

        public FootballFieldViewModel()
        {
            EditListFieldCommand = new RelayCommand<FieldDetailsControl>(parameter => true, parameter => ClickEditListField(parameter));
            EditCardFieldCommand = new RelayCommand<FieldControl>(parameter => true, parameter => ClickEditCardField(parameter));
            DeleteCardFieldCommand = new RelayCommand<FieldControl>(parameter => true, parameter => DeleteCardField(parameter));
            DeleteListFieldCommand = new RelayCommand<FieldDetailsControl>(parameter => true, parameter => DeleteListField(parameter));
            SaveCommand = new RelayCommand<addfootballfield>(parameter => true, parameter => SaveField(parameter));
            ExitCommand = new RelayCommand<addfootballfield>(parameter => true, parameter => parameter.Close());
            SeparateThousandsCommand = new RelayCommand<TextBox>(parameter => true, parameter => SeparateThousands(parameter));
            HoverCommand = new RelayCommand<FieldControl>((parameter) => true, (parameter) => Hover(parameter));
            LeaveCommand = new RelayCommand<FieldControl>((parameter) => true, (parameter) => Leave(parameter));
            LoadedCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => LoadField(parameter));
            SelectionChangedCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => SelectionChanged(parameter));
            AddFieldCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => ShowAddField(parameter));
            SetTimeFrameCommand = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => ShowWdSetTimeFrame());
            LostFocusCommand = new RelayCommand<addfootballfield>((parameter) => true, (parameter) => LostFocusComboBox(parameter));
            OpenReportFieldCommand = new RelayCommand<object>((parameter) => true, (parameter) => OpenReportFieldWindow());
            LoadFieldCommand = new RelayCommand<object>((parameter) => true, (parameter) => LoadFieldName());
            ReportFieldCommand = new RelayCommand<Window>((parameter) => true, (parameter) => ReportField(parameter));
        }

        public void ReportField(Window window)
        {
            FootballField footballField = selectedField;
            footballField.Status = 0;
            if (FootballFieldDP.Instance.UpdateField(footballField))
            {
                MessageBox.Show("Báo lỗi thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                window.Close();
            }
        }
        public void LoadFieldName()
        {
            itemSourceFieldName.Clear();
            foreach (var field in FootballFieldDP.Instance.GetGoodFields())
            {
                itemSourceFieldName.Add(field);
            }
        }
        public void OpenReportFieldWindow()
        {
            reportfield reportFieldWindow = new reportfield();
            reportFieldWindow.ShowDialog();
        }
        public void ShowWdSetTimeFrame()
        {
            settimeframe wdSetTime = new settimeframe();
            wdSetTime.ShowDialog();
        }
        public void SelectionChanged(HomeWindow parameter)
        {
            if (parameter.cboViews.SelectedIndex == 0)
            {
                parameter.grdListField.Visibility = Visibility.Visible;
                parameter.grdCardField.Visibility = Visibility.Hidden;
                LoadListFieldToView(parameter.wpListField);
            }
            if (parameter.cboViews.SelectedIndex == 1)
            {
                parameter.grdListField.Visibility = Visibility.Hidden;
                parameter.grdCardField.Visibility = Visibility.Visible;
                LoadCardFieldToView(parameter);
            }
        }
        public void LoadListFieldToView(WrapPanel wrap)
        {
            int i = 1;
            wrap.Children.Clear();
            bool flag = false;
            foreach (var footballField in FootballFieldDP.Instance.ConvertDBToList())
            {
                FieldDetailsControl temp = new FieldDetailsControl();
                flag = !flag;
                if (flag)
                {
                    temp.grdMain.Background = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                }
                temp.txbOrderNum.Text = i.ToString();
                i++;
                temp.txbIdField.Text = footballField.IdField.ToString();
                temp.txbFieldName.Text = footballField.Name.ToString();
                temp.txbFieldType.Text = footballField.Type.ToString();
                if (footballField.Status == 0)
                {
                    temp.txbStatus.Text = "Hỏng";
                }
                else
                {
                    temp.txbStatus.Text = "Tốt";
                }
                if (CurrentAccount.Type == 2)
                {
                    temp.btnDeleteField.IsEnabled = false;
                    temp.btnEditField.IsEnabled = false;
                }
                wrap.Children.Add(temp);
            }
        }
        public void DeleteListField(FieldDetailsControl control)
        {
            MessageBoxResult result = MessageBox.Show("Xác nhận xóa sân bóng?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                List<FieldInfo> fieldInfos = FieldInfoDP.Instance.GetFieldInfoByIdField(control.txbIdField.Text);
                if (fieldInfos.Count == 0)
                {
                    //Lưu xuống DB
                    if (FootballFieldDP.Instance.DeleteField(control.txbIdField.Text)) // cập  nhật isDeleted=1
                    {
                        //Cập nhật lên wrap
                        home.wpListField.Children.Remove(control);
                        if (FootballFieldDP.Instance.GetFieldType().Find(x => x == control.txbFieldType.Text) == null)
                        {
                            TimeFrameDP.Instance.DeleteFieldType(control.txbFieldType.Text);
                        }
                        bool flag = false;
                        for (int i = 0; i < home.wpListField.Children.Count; i++)
                        {
                            FieldDetailsControl temp = (FieldDetailsControl)home.wpListField.Children[i];
                            flag = !flag;
                            if (flag)
                            {
                                temp.grdMain.Background = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                            }
                            else
                            {
                                temp.grdMain.Background = (Brush)new BrushConverter().ConvertFromString("#F4EEFF");
                            }
                            temp.txbOrderNum.Text = (i + 1).ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sân đang được sử dụng, không được phép xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                return;
            }
        }
        public void DeleteCardField(FieldControl control)
        {
            MessageBoxResult result = MessageBox.Show("Xác nhận xóa sân bóng?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                //Lưu xuống DB
                List<FieldInfo> fieldInfos = FieldInfoDP.Instance.GetFieldInfoByIdField(control.txbIdField.Text);
                if (fieldInfos.Count == 0)
                {
                    if (FootballFieldDP.Instance.DeleteField(control.txbIdField.Text)) // cập nhật isDeleted=1
                    {
                        //Cập nhật lên wrap
                        home.wpCardField.Children.Remove(control);
                        if (FootballFieldDP.Instance.GetFieldType().Find(x => x == control.txbFieldType.Text) == null)
                        {

                            TimeFrameDP.Instance.DeleteFieldType(control.txbFieldType.Text);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Sân đang được sử dụng, không được phép xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        public void LostFocusComboBox(addfootballfield addFieldWindow)
        {

            string str = addFieldWindow.cboFieldType.Text;
            if (string.IsNullOrEmpty(str))
            {
                return;
            }
            if (str[0] >= 48 && str[0] <= 57)
            {
                addFieldWindow.cboFieldType.Text = str;
            }
        }
        public void ShowAddField(HomeWindow home)
        {
            addfootballfield wdAddfootballfield = new addfootballfield();
            int idfields = FootballFieldDP.Instance.GetMaxIdFootballFiled();

            if (idfields == 0)
            {
                wdAddfootballfield.txtIdField.Text = "1";
            }
            else
            {
                wdAddfootballfield.txtIdField.Text = (idfields + 1).ToString();
            }

            wdAddfootballfield.txtName.Text = null;
            wdAddfootballfield.cboFieldType.Text = null;
            wdAddfootballfield.ShowDialog();
        }
        public void ClickEditListField(FieldDetailsControl control)
        {
            this.detailsField = control;
            ShowEditField(control.txbIdField.Text);
        }
        public void ClickEditCardField(FieldControl control)
        {

            this.cardField = control;
            ShowEditField(control.txbIdField.Text);
        }
        public void setItemSourceField()
        {
            itemSourceField.Clear();
            foreach (string fieldType in FootballFieldDP.Instance.GetFieldType())
            {
                itemSourceField.Add(fieldType);
            }
        }
        public void ShowEditField(string idField)
        {
            setItemSourceField();
            addfootballfield updateWindow = new addfootballfield();
            updateWindow.Title = "Cập nhật sân bóng";
            FootballField footballField = FootballFieldDP.Instance.GetFootballFieldById(idField);
            updateWindow.txtIdField.Text = idField;
            updateWindow.txtName.Text = footballField.Name;
            updateWindow.txtName.SelectionStart = updateWindow.txtName.Text.Length;
            updateWindow.txtName.SelectionLength = 0;
            updateWindow.cboFieldType.Text = footballField.Type.ToString();
            if (CurrentAccount.Type != 0)
            {
                updateWindow.cboFieldType.IsReadOnly = true;
                updateWindow.txtName.IsReadOnly = true;
                updateWindow.cboFieldType.IsReadOnly = true;
            }

            int statusIndex = 1;
            if (footballField.Status == 1)
            {
                statusIndex = 0;
            }
            updateWindow.cboStatus.SelectedIndex = statusIndex;
            updateWindow.ShowDialog();
            return;
        }
        public void SaveField(addfootballfield parameter)
        {
            //Check giá trị null
            if (string.IsNullOrEmpty(parameter.txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sân", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtName.Text = "";
                parameter.txtName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(parameter.cboFieldType.Text))
            {
                MessageBox.Show("Vui lòng chọn loại sân", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.cboFieldType.Text = "";
                parameter.cboFieldType.Focus();
                return;
            }
            if (string.IsNullOrEmpty(parameter.cboStatus.Text))
            {
                parameter.cboStatus.Text = "";
                parameter.cboStatus.Focus();
                return;
            }

            int status = 1;

            if (parameter.cboStatus.SelectedIndex == 1)
            {
                status = 0;
            }
            //thêm khung giờ
            List<string> fieldTypes = FootballFieldDP.Instance.GetFieldType();

            FootballField newField = new FootballField(int.Parse(parameter.txtIdField.Text.ToString()),
                parameter.txtName.Text.ToString(), int.Parse(parameter.cboFieldType.Text.ToString()), status, "NULL", 0);
            List<FootballField> fields = FootballFieldDP.Instance.ConvertDBToList();
            bool isSuccess1 = false;
            bool isSuccess2 = false;
            if (fields.Count == 0 || fields[fields.Count - 1].IdField < newField.IdField)
            {
                //Kiểm tra sân tồn tại chưa
                if (FootballFieldDP.Instance.isExistFieldName(parameter.txtName.Text))
                {
                    MessageBox.Show("Tên sân đã tồn tại! Vui lòng nhập lại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    parameter.txtName.Text = " ";
                    parameter.txtName.Focus();
                    return;
                }

                //Lưu sân vào database
                if (FootballFieldDP.Instance.AddIntoDB(newField))
                {
                    isSuccess1 = true;
                    MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);

                    //Hiển thị
                    if (home.cboViews.SelectedIndex == 0)
                    {
                        FieldDetailsControl control = new FieldDetailsControl();
                        control.txbFieldType.Text = newField.Name;
                        control.txbIdField.Text = newField.IdField.ToString();
                        control.txbFieldType.Text = "sân " + newField.Type.ToString() + " người";
                        control.txbStatus.Text = newField.Status == 0 ? "Hỏng" : "Tốt";
                        control.txbOrderNum.Text = (home.wpListField.Children.Count + 1).ToString();
                        home.wpListField.Children.Add(control);
                    }
                    else if (home.cboViews.SelectedIndex == 1)
                    {
                        FieldControl control = new FieldControl();
                        control.txbFieldName.Text = newField.Name;
                        control.txbIdField.Text = newField.IdField.ToString();
                        control.txbFieldType.Text = "sân " + newField.Type.ToString() + " người";
                        if (parameter.cboStatus.SelectedIndex == 0)
                        {
                            control.icnError.Visibility = Visibility.Hidden;
                        }
                        home.wpCardField.Children.Add(control);
                    }
                }
                else
                {
                    MessageBox.Show("Thêm thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                //Kiểm tra tên sân có tồn tại chưa
                if (FootballFieldDP.Instance.GetFootballFieldById(parameter.txtIdField.Text).Name != parameter.txtName.Text)
                {
                    if (FootballFieldDP.Instance.isExistFieldName(parameter.txtName.Text))
                    {
                        MessageBox.Show("Tên sân đã tồn tại! Vui lòng nhập lại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        parameter.txtName.Text = " ";
                        parameter.txtName.Focus();
                        return;
                    }
                }

                //Update xuống database
                if (FootballFieldDP.Instance.UpdateField(newField))
                {
                    isSuccess2 = true;
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    //Cập nhật lên display
                    if (home.cboViews.SelectedIndex == 0)
                    {
                        detailsField.txbFieldName.Text = parameter.txtName.Text;
                        detailsField.txbFieldType.Text = parameter.cboFieldType.Text;
                        detailsField.txbStatus.Text = parameter.cboStatus.Text;
                    }
                    else if (home.cboViews.SelectedIndex == 1)
                    {
                        cardField.txbFieldName.Text = parameter.txtName.Text;
                        if (parameter.cboStatus.SelectedIndex == 0)
                        {
                            cardField.icnError.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            cardField.icnError.Visibility = Visibility.Visible;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //Thiết lập khung giờ cho loại sân mới
            if (fieldTypes.Find(x => x == parameter.cboFieldType.Text) == null && (isSuccess1 || isSuccess2))
            {
                int i = TimeFrameDP.Instance.GetIdMax();
                foreach (TimeFrame item in TimeFrameDP.Instance.GetTimeFrame())
                {
                    item.Id = ++i;
                    item.FieldType = newField.Type;
                    item.Price = -1;
                    TimeFrameDP.Instance.AddTimeFrame(item);
                }
                settimeframe newWinDow = new settimeframe();
                DispatcherTimer timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(1)
                };
                timer.Tick += (s, e) =>
                {
                    fieldTypes = FootballFieldDP.Instance.GetFieldType();
                    newWinDow.cboFieldType.SelectedIndex = fieldTypes.IndexOf(newField.Type.ToString());
                    timer.Stop();
                };
                timer.Start();
                newWinDow.ShowDialog();
            }
            parameter.Close();
        }
        public void Hover(FieldControl parameter)
        {
            parameter.grdMask.Visibility = Visibility.Visible;
            if (CurrentAccount.Type == 0)
            {
                parameter.btnDelete.Visibility = Visibility.Hidden;
            }
            parameter.btnEdit.Visibility = Visibility.Visible;
        }
        public void Leave(FieldControl parameter)
        {
            parameter.grdMask.Visibility = Visibility.Hidden;
            if (CurrentAccount.Type == 0)
            {
                parameter.btnDelete.Visibility = Visibility.Hidden;
            }
            parameter.btnEdit.Visibility = Visibility.Hidden;

        }
        public void LoadField(HomeWindow home)
        {
            this.home = home;
            if (CurrentAccount.Type == 1) { home.btnAddField.IsEnabled = false; }
            home.grdListField.Visibility = Visibility.Hidden;
            home.grdCardField.Visibility = Visibility.Visible;
            home.cboViews.Text = "Card";
            setItemSourceField();
            LoadCardFieldToView(home);
        }
        public void LoadCardFieldToView(HomeWindow parameter)
        {
            parameter.wpCardField.Children.Clear();
            foreach (var field in FootballFieldDP.Instance.ConvertDBToList())
            {
                FieldControl child = new FieldControl();
                child.txbFieldName.Text = field.Name;
                child.txbIdField.Text = field.IdField.ToString();
                if (field.Status == 1)
                {
                    child.icnError.Visibility = Visibility.Hidden;
                }
                child.txbFieldType.Text = "sân " + field.Type.ToString() + " người";
                if (CurrentAccount.Type == 2)
                {
                    child.btnDelete.IsEnabled = false;
                    child.btnEdit.IsEnabled = false;
                }
                parameter.wpCardField.Children.Add(child);
            }
        }
    }
}



