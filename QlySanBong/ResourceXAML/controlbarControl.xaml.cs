using QlySanBong.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QlySanBong.ResourceXAML
{
    /// <summary>
    /// Interaction logic for controlbarControl.xaml
    /// </summary>
    public partial class controlbarControl : UserControl
    {
        public ControlBarViewModel ViewModel { get; set; }

        public controlbarControl()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new ControlBarViewModel();
        }
    }
}
