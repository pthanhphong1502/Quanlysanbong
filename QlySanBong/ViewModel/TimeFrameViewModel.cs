using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QlySanBong.View;
using QlySanBong.ResourceXAML;
using System.Windows;
using QlySanBong.Model;
using QlySanBong.data_provier;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.Generic;
using System;

namespace QlySanBong.ViewModel
{
    class TimeFrameViewModel : BaseViewModel
    {
        public ICommand SelectionChangedFieldType { get; set; } //Chọn loại sân
        public ICommand LoadedCommand { get; set; } //Thay đổi thời gian mở của, đóng cửa
        public ICommand SetCommand { get; set; } //Thiết lập khung giờ
        public ICommand SaveCommand { get; set; } //Lưu 
        public ICommand DeleteTimeFrameCommand { get; set; } //Xóa khung giờ
        public ICommand TextChangedCommand { get; set; } //Thay đổi giá của khung giờ
        public ICommand SeparateThousandsCommand { get; set; } //Chuyển sang dạng 0,000,000
        public ICommand ShowWindowAddTimeFrame { get; set; }
        private ObservableCollection<string> itemSourceFieldType = new ObservableCollection<string>();
        public ObservableCollection<string> ItemSourceFieldType { get => itemSourceFieldType; set { itemSourceFieldType = value; OnPropertyChanged(); } }

        private bool isChanged; // kiểm tra có sự thay đổi nào không
        public bool IsChanged { get => isChanged; set => isChanged = value; }
        private string price;
        public string Price { get => price; set => price = value; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
        public string TimePerMatch { get; set; }

        private settimeframe setTimeWd;
        public settimeframe SetTimeWd { get => setTimeWd; set => setTimeWd = value; }

        public List<TimeFrame> tmpTimeFrames;// Lưu những thứ chưa được lưu dưới DB => khi bấm nút Lưu =>Lưu DB
        public TimeFrameViewModel()
        {
            LoadedCommand = new RelayCommand<settimeframe>(parameter => true, parameter => Load(parameter));
            TextChangedCommand = new RelayCommand<PeriodControl>(parameter => true, parameter => TextChanged(parameter));
            SeparateThousandsCommand = new RelayCommand<TextBox>(parameter => true, parameter => SeparateThousands(parameter));
            DeleteTimeFrameCommand = new RelayCommand<PeriodControl>(parameter => true, parameter => DeleteTimeFrame(parameter));
            SelectionChangedFieldType = new RelayCommand<settimeframe>(parameter => true, parameter => ChangedFieldType(parameter));
            SaveCommand = new RelayCommand<settimeframe>(parameter => true, parameter => SaveData(parameter));
            SetCommand = new RelayCommand<settimeframe>(parameter => true, parameter => GenerateTimeFrame(parameter));
        }

        public void GenerateTimeFrame(settimeframe wdSetTime)
        {
            if (string.IsNullOrEmpty(wdSetTime.tpkOpenTime.Text))
            {
                wdSetTime.tpkOpenTime.Focus();
                return;
            }
            if (string.IsNullOrEmpty(wdSetTime.tpkCloseTime.Text))
            {
                wdSetTime.tpkCloseTime.Focus();
                return;
            }
            if (string.IsNullOrEmpty(wdSetTime.cboTimePerMatch.Text))
            {
                wdSetTime.cboTimePerMatch.Focus();
                wdSetTime.cboTimePerMatch.Text = "";
                return;
            }
            if (CovertToMinute(wdSetTime.tpkOpenTime.Text) > CovertToMinute(wdSetTime.tpkCloseTime.Text))
            {
                wdSetTime.cboTimePerMatch.SelectedItem = null;
                wdSetTime.tpkOpenTime.Text = null;
                wdSetTime.tpkCloseTime.Text = null;
                MessageBox.Show("Giờ bắt đầu nhỏ hơn giờ kết thúc!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            this.tmpTimeFrames.Clear();
            wdSetTime.stkTime.Children.Clear();
            this.isChanged = true;
            //Chia khung giờ
            int openTime = CovertToMinute(wdSetTime.tpkOpenTime.Text);
            int closeTime = CovertToMinute(wdSetTime.tpkCloseTime.Text);
            int step;
            step = 30 * (wdSetTime.cboTimePerMatch.SelectedIndex + 2);
            for (int i = openTime; i <= closeTime - step; i += step)
            {
                string str1 = (i % 60).ToString();
                if (i % 60 < 10)
                {
                    str1 = "0" + str1;
                }
                string str2 = ((i + step) % 60).ToString();
                if ((i + step) % 60 < 10)
                {
                    str2 = "0" + str2;
                }
                string strStartTime = ((i / 60 < 10) ? "0" : "") + (i / 60).ToString() + ":" + str1;
                string strEndTime = (((i + step) / 60) < 10 ? "0" : "") + ((i + step) / 60).ToString() + ":" + str2;
                PeriodControl control = new PeriodControl();
                control.txtStartTime.Text = strStartTime;
                control.txtEndTime.Text = strEndTime;
                control.txbId.Text = (tmpTimeFrames.Count + 1).ToString();
                wdSetTime.stkTime.Children.Add(control);

                //Add vào 1 list tạm => mục đích để hiển thị thông tin đang sửa(chưa lưu xuống DB)
                foreach (string item in FootballFieldDP.Instance.GetFieldType())
                {
                    TimeFrame time = new TimeFrame(tmpTimeFrames.Count + 1, strStartTime, strEndTime, int.Parse(item), -1);
                    this.tmpTimeFrames.Add(time);
                }
            }
        }
        public void CloseWindow(object sender, CancelEventArgs e)
        {
            TimeFrame time = tmpTimeFrames.Find(x => x.Price == -1);
            if (isChanged || time != null)
            {
                MessageBoxResult result = MessageBox.Show("Bạn có muốn lưu hay không?", "Thông báo", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    if (IsPriceNull(SetTimeWd))
                    {
                        e.Cancel = true;
                        return;
                    }
                    SaveData(SetTimeWd);
                    if (!isChanged)
                    {
                        e.Cancel = false;
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    if (TimeFrameDP.Instance.CheckPriceIsNull())
                    {
                        e.Cancel = true;
                        MessageBox.Show("Khung giờ của loại sân mới chưa được thiết lập!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
        public int CovertToMinute(string time)
        {
            string[] tmp = time.Split(':');
            return int.Parse(tmp[0]) * 60 + int.Parse(tmp[1]);
        }
        public void SaveData(settimeframe wdSetTime)
        {
            //Lưu
            if (!IsPriceNull(wdSetTime) && isChanged)
            {
                TimeFrameDP.Instance.ClearData();
                bool isSuccess = true;
                for (int i = 0; i < tmpTimeFrames.Count; i++)
                {
                    TimeFrame newTime = tmpTimeFrames[i];
                    if (!TimeFrameDP.Instance.AddTimeFrame(newTime))
                    {
                        isSuccess = false;
                        break;
                    }
                }
                if (isSuccess)
                {
                    MessageBox.Show("Đã lưu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    isChanged = false;
                    wdSetTime.Close();
                }
                else
                {
                    MessageBox.Show("Lưu lỗi!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        //Kiểm tra giá của các khung giờ có null hay không
        public bool IsPriceNull(settimeframe wdSetTime)
        {
            var listTemp = tmpTimeFrames.FindAll(x => x.Price == -1).OrderBy(x => x.FieldType).ToList();
            if (listTemp.Count() != 0)
            {
                int i = 0;
                if (listTemp[0].FieldType.ToString() == wdSetTime.cboFieldType.Text.Split(' ')[1])
                {
                    MessageBox.Show("Vui lòng nhập giá!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                foreach (string fieldType in FootballFieldDP.Instance.GetFieldType())
                {
                    if (listTemp[0].FieldType.ToString() == fieldType)
                    {
                        wdSetTime.cboFieldType.SelectedIndex = i;
                        return true;
                    }
                    i++;
                }
            }
            return false;
        }
        public void DeleteTimeFrame(PeriodControl control)
        {
            MessageBoxResult result = MessageBox.Show("Sẽ xóa những loại sân có cùng khung giờ này. Bạn có muốn xóa?", 
                "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.tmpTimeFrames.RemoveAll(x => x.StartTime == control.txtStartTime.Text && x.EndTime == control.txtEndTime.Text);
                this.setTimeWd.stkTime.Children.Remove(control);
                if (tmpTimeFrames.Count != 0)
                {
                    setTimeWd.tpkOpenTime.Text = tmpTimeFrames[0].StartTime;
                    setTimeWd.tpkCloseTime.Text = tmpTimeFrames[tmpTimeFrames.Count - 1].EndTime;
                }
                this.isChanged = true;
            }
        }
        public void TextChanged(PeriodControl control)
        {
            if (string.IsNullOrEmpty(control.txtPrice.Text))
            {
                this.tmpTimeFrames.Where(x => x.Id.ToString() == control.txbId.Text).ToList().ForEach(x => x.Price = -1);
            }
            else
            {
                this.tmpTimeFrames.Where(x => x.Id.ToString() == control.txbId.Text).ToList().ForEach(x => x.Price = ConvertToNumber(control.txtPrice.Text));
            }
            this.isChanged = true;
        }
        public void Load(settimeframe wdSetTime)
        {
            this.IsChanged = false;
            this.setTimeWd = wdSetTime;
            setItemSourceFieldType();
            tmpTimeFrames = TimeFrameDP.Instance.ConvertDBToList();
            wdSetTime.cboFieldType.SelectedIndex = 0;
            if (tmpTimeFrames.Count != 0)
            {
                wdSetTime.tpkOpenTime.SelectedTime = DateTime.Parse(tmpTimeFrames[0].StartTime);
                wdSetTime.tpkCloseTime.SelectedTime = DateTime.Parse(tmpTimeFrames[tmpTimeFrames.Count - 1].EndTime);
            }
            wdSetTime.cboTimePerMatch.Text = null;
            ChangedFieldType(wdSetTime);
        }
        public void setItemSourceFieldType()
        {
            this.itemSourceFieldType.Clear();
            foreach (var fieldType in FootballFieldDP.Instance.GetFieldType())
            {
                itemSourceFieldType.Add("Sân " + fieldType + " người");
            }
        }
        public void ChangedFieldType(settimeframe wdSetTime)
        {
            if (wdSetTime.cboFieldType.SelectedItem == null)
                return;
            wdSetTime.stkTime.Children.Clear();
            string tmp = wdSetTime.cboFieldType.SelectedValue.ToString();
            string fieldType = tmp.Split(' ')[1];
            foreach (TimeFrame time in this.tmpTimeFrames.FindAll(x => x.FieldType.ToString() == fieldType))
            {
                PeriodControl control = new PeriodControl();
                control.txbId.Text = time.Id.ToString();
                control.txtStartTime.Text = time.StartTime;
                control.txtEndTime.Text = time.EndTime;
                if (time.Price != -1)
                {
                    control.txtPrice.Text = string.Format("{0:N0}", time.Price);
                }
                control.txtPrice.SelectionStart = control.txtPrice.Text.Length;
                control.txtPrice.SelectionLength = 0;
                wdSetTime.stkTime.Children.Add(control);
            }
        }
    }
}


