using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using TechDashboard.ViewModels;

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for TechnicianListPage.xaml
    /// </summary>
    public partial class TechnicianListPage : UserControl
    {
        Progress progressWindow = new Progress();
        BackgroundWorker bw = new BackgroundWorker();
        TechnicianListPageViewModel _vm;

        public static readonly RoutedEvent SignedInEvent = EventManager.RegisterRoutedEvent("SigneInEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TechnicianListPage));

        public event RoutedEventHandler SignedIn
        {
            add { AddHandler(SignedInEvent, value); }
            remove { RemoveHandler(SignedInEvent, value); }
        }

        void RaiseSignedInEvent()
        {
            ContentControl cc = (ContentControl)this.Parent;
            DockPanel dp = (DockPanel)cc.Parent;
            Grid grid = (Grid)dp.Parent;
            MainWindow mainWindow = (MainWindow)grid.Parent;
            mainWindow.IsEnabled = true;
            RoutedEventArgs newEventArgs = new RoutedEventArgs(TechnicianListPage.SignedInEvent);
            RaiseEvent(newEventArgs);
        }

        public TechnicianListPage()
        {
            InitializeComponent();
            _vm = new TechnicianListPageViewModel();
            gridTechnician.ItemsSource = _vm.TechnicianList;
            gridTechnician.MouseDoubleClick += GridTechnician_MouseDown;
            bw.WorkerReportsProgress = true;
            bw.DoWork += Bw_DoWork;
            bw.ProgressChanged += Bw_ProgressChanged;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressWindow.Close();
            RaiseSignedInEvent();
        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressWindow.pbStatus.Value = e.ProgressPercentage;
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Models.App_Technician appTechnician = (Models.App_Technician)e.Argument;
            _vm.SignIn(appTechnician);
        }

        private async void GridTechnician_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //bool hasValidSetup = await App.Database.HasValidSetup();
            bool hasValidSetup = App.Database.HasValidSetup();
            if (!hasValidSetup)
            {
                var result = MessageBox.Show("Your connection settings appear to be invalid.  Check your Internet Connection or verify your settings.", "CANNOT CONNECT TO SERVER", MessageBoxButton.OK);
                return;
            }
            progressWindow.Show();
            ContentControl cc = (ContentControl)this.Parent;
            DockPanel dp = (DockPanel)cc.Parent;
            Grid grid = (Grid)dp.Parent;
            MainWindow mainWindow = (MainWindow)grid.Parent;
            mainWindow.btnTechnician.Visibility = Visibility.Visible;
            mainWindow.btnSchedule.Visibility = Visibility.Visible;
            mainWindow.btnHistory.Visibility = Visibility.Visible;
            mainWindow.btnMiscTime.Visibility = Visibility.Visible;
            mainWindow.btnExpenses.Visibility = Visibility.Visible;
            //mainWindow.btnSMS.Visibility = Visibility.Visible;
            mainWindow.btnSync.Visibility = Visibility.Visible;
            mainWindow.Hide();
            Models.App_Technician appTechnician = (Models.App_Technician)gridTechnician.SelectedItem;
           
            
            bw.RunWorkerAsync(appTechnician);
        }
    }
}
