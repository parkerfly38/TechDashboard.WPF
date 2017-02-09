using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using TechDashboard.Data;
using TechDashboard.Models;
using TechDashboard.ViewModels;

/**************************************************************************************************
 * Page Name    MainWindow
 * Description: Main Window
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/26/2016   DCH     Pass the current main window object to settings page, which allows "cancel"
 *                      from settings page without saving.
 * 12/01/2016   DCH     Correct misspelling of GetApplicationSettings
 * 01/16/2017   DCH     Make sure the main window isn't a tab stop, and doesn't get selected.
 * 01/20/2017   DCH     Do not allow logoff if there is no data connection.
 * 01/20/2017   DCH     If logging off, and Transactions are Pending to Sync, force them to sync first
 * 02/03/2017   DCH     Need more positions for version.
 **************************************************************************************************/
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
            //btnSMS.Click += BtnSMS_Click;
            btnExit.Click += BtnExit_Click;

            App_Settings appSettings = App.Database.GetApplicationSettings();

            // dch rkl 12/08/2016 Get Application Version and Compare to Current DB Version
            // If the versions do not match, they need to do a sync and refresh of their data
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string Buildversion = fvi.FileVersion;
            string dbVersion = appSettings.DbVersion.Substring(0, 5);  // dch rkl 02/03/2017 Need more positions for version
            if (Buildversion != dbVersion)
            {
                bool bHasDataConnection = App.Database.HasDataConnection();
                if (bHasDataConnection == false)
                {
                    string sMsg = string.Format("WARNING: Your local database version is {0}, and your \napplication version is {1}. An internet connection is required \nto refresh your database schema, and is not currently present. Please log into the application when an internet connection \nis available so the sync and upgrade can be completed.", dbVersion, Buildversion);
                    var result = MessageBox.Show(sMsg, "Database Update Required", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    Application.Current.Shutdown();
                    return;
                }
                else
                {
                    string sMsg = string.Format("WARNING: Your local database version is {0}, and your \napplication version is {1}. A database Sync is required at this time. \nClick OK to continue with the upgrade, or Cancel to exit.\n \nIf OK is selected, any pending transactions will be sent to JobOps prior to sync.", dbVersion, Buildversion);
                    var result = MessageBox.Show(sMsg, "Database Update Required", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    if (result == MessageBoxResult.Cancel)
                    {
                        Application.Current.Shutdown();
                        return;
                    }
                    else
                    {
                        // Send any JT_TransactionImportDetail records back to JobOps
                        TransactionSync();

                        // Update local Data
                        App.Database.CreateGlobalTables();

                        appSettings.DbVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                        App.Database.SaveAppSettings(appSettings);
                    }
                }
            }

            if (appSettings.LoggedInTechnicianNo != null)
            {
                if (appSettings.LoggedInTechnicianNo.Length > 0 && appSettings.LoggedInTechnicianDeptNo.Length > 0)
                {
                    JT_Technician technician = App.Database.GetTechnician(appSettings.LoggedInTechnicianDeptNo, appSettings.LoggedInTechnicianNo);
                    App.Database.SaveTechnicianAsCurrent(technician);
                    if (App.Database.HasDataConnection())
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
            contentArea.IsTabStop = false;      // dch rkl 01/16/2017 Make sure the main window isn't a tab stop, and doesn't get selected.
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
                //btnSMS.Visibility = Visibility.Collapsed;
                btnSync.Visibility = Visibility.Collapsed;
            }
        }

        // dch rkl 12/08/2016 Sync Data Function for JT_TransactionImportDetail
        private void TransactionSync()
        {
            Cursor prev = Mouse.OverrideCursor;
            this.Cursor = Cursors.Wait;

            try
            {
                TechDashboard.Data.RestClient restClient = new Data.RestClient(App.Database.GetApplicationSettings().IsUsingHttps, App.Database.GetApplicationSettings().RestServiceUrl);

                List<JT_TransactionImportDetail> transactionImportDetails = App.Database.GetCurrentExport();

                foreach (JT_TransactionImportDetail transaction in transactionImportDetails)
                {
                    // If Lot/Serial Nbr Data, sync back to JobOps with multiple rows
                    bool updateWorked = true;
                    if (transaction.LotSerialNo == null || transaction.LotSerialNo.Trim().Length == 0)
                    {
                        // dch rkl 12/09/2016 This now returns a results object
                        //updateWorked = restClient.InsertTransactionImportDetailRecordSync(transaction);
                        updateWorked = restClient.InsertTransactionImportDetailRecordSync(transaction).Success;
                    }
                    else
                    {
                        // Split into LotSerNo/Qty strings
                        string[] lotSerQty = transaction.LotSerialNo.Split('|');
                        double qty = 0;

                        foreach (string lsq in lotSerQty)
                        {
                            // Split each LotSerNo/Qty string into LotSerNo and Qty
                            string[] sqty = lsq.Split('~');
                            if (sqty.GetUpperBound(0) > 0)
                            {
                                double.TryParse(sqty[1], out qty);
                                if (qty > 0)
                                {
                                    transaction.QuantityUsed = qty;
                                    transaction.LotSerialNo = sqty[0];
                                    // dch rkl 12/09/2016 This now returns a results object
                                    //bool updateWorkedLS = restClient.InsertTransactionImportDetailRecordSync(transaction);
                                    bool updateWorkedLS = restClient.InsertTransactionImportDetailRecordSync(transaction).Success;
                                    if (updateWorkedLS == false)
                                    {
                                        updateWorked = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (updateWorked)
                    {
                        App.Database.DeleteExportRow(transaction);
                    }
                }
            }
            catch (Exception ex)
            {
                // dch rkl 02/03/2017 Log Error
                ErrorReporting errorReporting = new ErrorReporting();
                errorReporting.sendException(ex, "TechDashboard.MainWindow.TransactionSync");
            }
            finally
            {
                this.Cursor = prev;
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            // dch rkl 01/20/2017 Do not allow logoff if there is no data connection
            bool bDoLogoff = true;
            bool bHasDataConnection = App.Database.HasDataConnection();
            if (bHasDataConnection == false)
            {
                string sMsg = "WARNING: No data connection exists; logoff cannot be done at this time.";
                MessageBoxResult result = MessageBox.Show(sMsg, "Cannot Log Off", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                bDoLogoff = false;
            }

            else
            {
                // dch rkl 01/20/2017 If Transactions are Pending to Sync, force them to sync first
                List<JT_TransactionImportDetail> transactionImportDetails = App.Database.GetCurrentExport();
                if (transactionImportDetails.Count > 0)
                {
                    string sMsg = "WARNING: Pending transactions exist, and\n a sync is required before logging off.\n\nClick OK to proceed with Sync, or \nCancel to cancel logoff.";
                    MessageBoxResult result = MessageBox.Show(sMsg, "Sync is Required", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    if (result == MessageBoxResult.OK)
                    {
                        TransactionSync();
                    }
                    else
                    {
                        bDoLogoff = false;
                    }
                }
            }

            //Application.Current.Shutdown();
            if (bDoLogoff)
            {
                App_Settings appSettings = App.Database.GetApplicationSettings();
                appSettings.LoggedInTechnicianDeptNo = "";
                appSettings.LoggedInTechnicianNo = "";
                App.Database.SaveAppSettings(appSettings);
                contentArea.Content = new TechnicianListPage();
            }
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
            // dch rkl 10/26/2016 pass parent
            //SettingsPage settingsPage = new SettingsPage();
            SettingsPage settingsPage = new SettingsPage(this);

            settingsPage.Show();
            this.Hide();
        }
    }
}
