using QlySanBong.data_provier;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace QlySanBong.ViewModel
{
    class ReportViewModel:BaseViewModel
    {
        public ICommand LoadReportCommad { get; set; }

        public ReportViewModel(HomeWindow homeWindow)
        {
            LoadReportWindow(homeWindow);
        }
        public ReportViewModel()
        {
            LoadReportCommad = new RelayCommand<HomeWindow>((parameter) => true, (parameter) => LoadReportWindow(parameter));
        }
        private void LoadReportWindow(HomeWindow parameter)
        {
            long total = 0;
            foreach (var totalImport in StockReceiptDP.Instance.ConvertDBToList())
            {
                if(totalImport.DateTimeStockReceipt.Month == DateTime.Now.Month)
                     total += totalImport.Total;
            }
            int number = 0;
            foreach (var totalFielded in FieldInfoDP.Instance.ConvertDBToList())
            {
                if(totalFielded.EndingTime.Month == DateTime.Now.Month )
                    number++;
            }
            parameter.tbTotalMoneyImport.Text = string.Format("{0:N0}", total) +"VNĐ";
            parameter.tbNumberofPitchIsPlaced.Text = number.ToString();
        }
    }
}
