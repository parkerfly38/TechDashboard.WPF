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
using TechDashboard.Models;

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btnSettings.Click += BtnSettings_Click;
            btnExpenses.Click += BtnExpenses_Click;
            btnSchedule.Click += BtnSchedule_Click;
            btnHistory.Click += BtnHistory_Click;
            btnMiscTime.Click += BtnMiscTime_Click;
            btnSync.Click += BtnSync_Click;
            btnTechnician.Click += BtnTechnician_Click;
            btnSMS.Click += BtnSMS_Click;
            btnExit.Click += BtnExit_Click;

            App_Settings appSettings = App.Database.GetApplicatioinSettings();
            if (appSettings.LoggedInTechnicianNo != null)
            {
                if (appSettings.LoggedInTechnicianNo.Length > 0 && appSettings.LoggedInTechnicianDeptNo.Length > 0)
                {
                    JT_Technician technician = App.Database.GetTechnician(appSettings.LoggedInTechnicianDeptNo, appSettings.LoggedInTechnicianNo);
                    App.Database.SaveTechnicianAsCurrent(technician);
                    App.Database.CreateDependentTables(technician);
                    contentArea.Content = new SchedulePage();
                } else
                {
                    contentArea.Content = new TechnicianListPage();
                }
            }
            else {
                contentArea.Content = new TechnicianListPage();
            }
            AddHandler(TechnicianListPage.SignedInEvent, new RoutedEventHandler(SignedInEventHandlerMethod));
            AddHandler(SchedulePage.SelectedTicketEvent, new RoutedEventHandler(SelectedTicketEventHandlerMethod));
            this.Closed += MainWindow_Closed;
            if (App.Database.GetCurrentTechnicianFromDb() == null)
            {
                btnTechnician.Visibility = Visibility.Collapsed;
                btnSchedule.Visibility = Visibility.Collapsed;
                btnHistory.Visibility = Visibility.Collapsed;
                btnMiscTime.Visibility = Visibility.Collapsed;
                btnExpenses.Visibility = Visibility.Collapsed;
                btnSMS.Visibility = Visibility.Collapsed;
                btnSync.Visibility = Visibility.Collapsed;
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
            App_Settings appSettings = App.Database.GetApplicatioinSettings();
            appSettings.LoggedInTechnicianDeptNo = "";
            appSettings.LoggedInTechnicianNo = "";
            App.Database.SaveAppSettings(appSettings);
            contentArea.Content = new TechnicianListPage();
        }

        private void BtnSMS_Click(object sender, RoutedEventArgs e)
        {
            contentArea.Content = new SmsPage();
        }

        private void BtnTechnician_Click(object sender, RoutedEventArgs e)
        {
            contentArea.Content = new TechnicianPage();
        }

        private void BtnSync_Click(object sender, RoutedEventArgs e)
        {
            contentArea.Content = new SyncPage();
        }

        private void BtnMiscTime_Click(object sender, RoutedEventArgs e)
        {
            contentArea.Content = new MiscellaneousTimePage();
        }

        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            contentArea.Content = new HistoryPage();
        }

        private void BtnSchedule_Click(object sender, RoutedEventArgs e)
        {
            contentArea.Content = new SchedulePage();
        }

        private void SignedInEventHandlerMethod(object sender, RoutedEventArgs e)
        {
            contentArea.Content = new SchedulePage();
            this.Show();
        }

        private void SelectedTicketEventHandlerMethod(object sender, RoutedEventArgs e)
        {
            DataGrid ticketSource = (DataGrid)e.OriginalSource;
            App_ScheduledAppointment scheduledAppointment = (App_ScheduledAppointment)ticketSource.SelectedItem;
            contentArea.Content = new TicketDetailsPage(scheduledAppointment);
        }

        private void BtnExpenses_Click(object sender, RoutedEventArgs e)
        {
            contentArea.Content = new ExpensesListPage();
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage settingsPage = new SettingsPage();
            settingsPage.Show();
            this.Hide();
        }
    }
}
