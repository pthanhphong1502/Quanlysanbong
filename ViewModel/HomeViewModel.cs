using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using QlySanBong.View;
using QlySanBong.ResourceXAML;
using System.Windows.Media;
using QlySanBong.Model;
using QlySanBong.data_provier;
using System.Linq;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Threading;
using Microsoft.Win32;
using System.Data.OleDb;

namespace QlySanBong.ViewModel
{
    class HomeViewModel : BaseViewModel
    {
        public ICommand LogOutCommand { get; set; }
        public ICommand SwitchTabCommand { get; set; }

        public ICommand E_LoadCommand { get; set; }
        public ICommand E_AddCommand { get; set; }
        public ICommand E_SetSalaryCommand { get; set; }
        public ICommand E_CalculateSalaryCommand { get; set; }

        public ICommand GetUidCommand { get; set; }

        public ICommand S_SaveBtnFieldInfoCommand { get; set; }
        public ICommand S_SaveFieldInfoCommand { get; set; }
        public ICommand S_EnableBtnSavePassCommand { get; set; }
        public ICommand S_SaveNewPasswordCommand { get; set; }
        public ICommand OpenCheckAttendanceWindowCommand { get; set; }
        public StackPanel Stack { get => stack; set => stack = value; }

        private StackPanel stack = new StackPanel();
        private string uid;
        public HomeViewModel()
        {
            LogOutCommand = new RelayCommand<Window>((parameter) => true, (parameter) => parameter.Close());
            GetUidCommand = new RelayCommand<Button>((parameter) => true, (parameter) => uid = parameter.Uid);
        }
    }
}
