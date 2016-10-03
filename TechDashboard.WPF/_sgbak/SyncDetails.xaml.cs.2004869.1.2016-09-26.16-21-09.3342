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
using System.Windows.Shapes;
using TechDashboard.ViewModels;

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for SyncDetails.xaml
    /// </summary>
    public partial class SyncDetails : Window
    {
        SyncDetailViewModel _vm;

        public SyncDetails()
        {
            InitializeComponent();
            _vm = new SyncDetailViewModel();

            dataGrid.ItemsSource = _vm.transactionImportDetails;
            dataGrid.AutoGenerateColumns = true;
        }
    }
}
