using QlySanBong.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QlySanBong.ViewModel
{
    class RegisterModel: BaseViewModel
    {
        public ICommand closeRegisterWindow { get; set; }

        public RegisterModel()
        {
            closeRegisterWindow = new RelayCommand<Window>((parameter) => true, (parameter) => parameter.Close());
        }
    }
}
