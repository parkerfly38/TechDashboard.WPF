using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using TechDashboard.Models;
using Newtonsoft.Json;
using System.Net;
using RestSharp;
using TechDashboard.ViewModels;

/**************************************************************************************************
 * Page Name    SyncPage
 * Description: Sync Page
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/26/2016   DCH     Standardize page font sizes, colors and buttons and alignment of data, labels.
 * 11/02/2016   DCH     Display message if sync fails; display message when sync is completed.
 * 12/02/2016   DCH     Informational message text should not be bold.
 * 01/20/2017   DCH     After database is refreshed, reset current technician based on the latest values pulled from JobOps    
 * 01/24/2017   BK      Change language and such because it's so unclear now...not
 **************************************************************************************************/

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for SyncPage.xaml
    /// </summary>
    public partial class SyncPage : UserControl
    {
        SyncPageViewModel _vm;

        SolidColorBrush asbestos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"));
        SolidColorBrush emerald = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ECC71"));
        SolidColorBrush alizarin = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C"));
        SolidColorBrush peterriver = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB"));

        public SyncPage()
        {
            InitializeComponent();
            Initialize();
        }

        protected void Initialize()
        {
            _vm = new SyncPageViewModel();
            this.DataContext = _vm;
            Label labelUpdateAppData = new Label()
            {
                Content = "REFRESH TECHNICIAN DASHBOARD DATA",
                Foreground = asbestos,
                FontWeight = FontWeights.Bold
            };

            TextBlock labelUpdateWarning = new TextBlock()
            {
                Text = "Select the Technician Dashboard Data button to refresh the Technician Dashboard data.  Access SETTINGS to specifiy a time range.  The refresh process requires a mobile data or WiFi connection.",
                TextWrapping = TextWrapping.Wrap,
                Foreground = asbestos,
                //FontWeight = FontWeights.Bold   dch rkl 12/02/2016 this should not be bold
            };

            Button buttonUpdateData = new Button()
            {
                Foreground = new SolidColorBrush(Colors.White),
                Background = emerald,
                BorderBrush = emerald,
                FontWeight = FontWeights.Bold,
                Height = 50,
                Margin = new Thickness(0,20,0,20)
            };
            //later, add check for connectivity and make this presentation conditional
            buttonUpdateData.Content = "REFRESH TECHNCIAN DASHBOARD DATA";
            buttonUpdateData.Click += ButtonUpdateData_Clicked;

            Rectangle separator = new Rectangle()
            {
                Height = 1,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Stroke = asbestos,
                Margin = new Thickness(30,20,30,0)
            };

            StackPanel refreshLayout = new StackPanel
            {
                Margin = new Thickness(30,10,30,0),
                Children = {
                    labelUpdateAppData,
                    labelUpdateWarning,
                    buttonUpdateData
                }
            };

            Label labelSendDataTitle = new Label()
            {
                Content = "UPLOAD DATA",
                Foreground = asbestos,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(30,10,30,0)
            };

            TextBlock labelSendWarning = new TextBlock()
            {
                Text = "Select the UPLOAD DATA button to send data from Technician Dashboard to JobOps/Sage 100c Manufacturing.  The sync process requires a mobile data or WiFi connection.",
                TextWrapping = TextWrapping.Wrap,
                Foreground = asbestos,
                Margin = new Thickness(30, 10, 30, 0)
            };

            Label labelSendCount = new Label()
            {
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Red)
            };
            labelSendCount.SetBinding(ContentProperty, "UpdateCount");

            Label labelSendContent = new Label()
            {
                Content = "records awaiting upload.",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Red)
            };

            Label labelLastSyncDate = new Label()
            {
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Red),
                Margin = new Thickness(30, 10, 30, 10)
            };
            labelLastSyncDate.SetBinding(Label.ContentProperty, "LastSyncDate");

            Button buttonSendData = new Button()
            {
                Foreground = new SolidColorBrush(Colors.White),
                Background = emerald,
                BorderBrush = emerald,
                FontWeight = FontWeights.Bold,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(30, 10, 30, 10)
            };
            // same as bove, add check for connectivity and make this enabled conditional
            buttonSendData.Content = "UPLOAD DATA";
            buttonSendData.Click += ButtonSendData_Clicked;

            Button buttonViewData = new Button()
            {
                Foreground = new SolidColorBrush(Colors.White),
                Background = alizarin,
                BorderBrush = alizarin,
                FontWeight = FontWeights.Bold,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(30, 10, 30, 10)
            };
            buttonViewData.Content = "PREVIEW DATA";
            buttonViewData.Click += ButtonViewData_Click;

            StackPanel titleLayout = new StackPanel
            {
                Background = peterriver,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Children = {
                    new Label {
                        Content = "SYNC",
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Colors.White),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 18       // dch rkl 10/26/2016
                    }
                }
            };

            gridMains.Children.Add(new StackPanel
            {
                Children = {
                    titleLayout,
                    refreshLayout,
                    separator,
                    labelSendDataTitle,
                    labelSendWarning,
                    new StackPanel()
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(30,10,30,0),
                        Children =
                        {
                            labelSendCount,
                            labelSendContent
                        }
        
                    },
                    labelLastSyncDate,
                    buttonViewData,
                    buttonSendData
                }
            });
        }

        private void ButtonViewData_Click(object sender, RoutedEventArgs e)
        {
            SyncDetails syncDetails = new SyncDetails();
            syncDetails.Show();            
        }

        void ButtonSendData_Clicked(object sender, EventArgs e)
        {
            Button buttonSendData = (Button)sender;
            buttonSendData.IsEnabled = false;
            buttonSendData.Content = "SENDING DATA...";
            
            // dch rkl 11/02/2016 add try/catch to catch and display message if the sync fails
            try
            {

                if (!App.Database.HasDataConnection())
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("No data connection presently, please check and try again.", "Connectivity Issue", MessageBoxButton.OK);
                    if (result == MessageBoxResult.OK)
                        return;
                }
                else
                {
                    // dch rkl 12/09/2016 return number failed and number successful
                    int iSyncSuccess = 0;
                    int iSyncFailed = 0;
                    //_vm.syncWithServer();
                    _vm.syncWithServer(ref iSyncSuccess, ref iSyncFailed);
                    string syncMessage ="";
                    if (iSyncFailed > 0 && iSyncSuccess > 0)
                    {
                        syncMessage = string.Format("{0} transactions synced successfully; {1} transactions failed during sync.", iSyncSuccess, iSyncFailed);
                    }
                    else if (iSyncSuccess > 0 && iSyncFailed == 0)
                    {
                        syncMessage = string.Format("{0} transactions synced successfully.", iSyncSuccess);
                    }
                    else if (iSyncSuccess == 0 && iSyncFailed > 0)
                    {
                        syncMessage = string.Format("{0} transactions failed during sync.", iSyncFailed);
                    }
                    else
                    {
                        syncMessage = "Sync completed successfully.";
                    }

                    // dch rkl 11/02/2016 display message when sync has completed
                    //MessageBoxResult result = System.Windows.MessageBox.Show("Sync has completed successfully.", "Sync Completed", MessageBoxButton.OK);
                    MessageBoxResult result = System.Windows.MessageBox.Show(syncMessage, "Sync Completed", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(String.Format("Sync failed with the following error: {0}",ex.ToString()), 
                    "Sync Failed", MessageBoxButton.OK);
            }

            buttonSendData.IsEnabled = true;
            buttonSendData.Content = "SYNC DATA";
        }

        void ButtonUpdateData_Clicked(object sender, EventArgs e)
        {
            JT_Technician currentTechnician = App.Database.GetCurrentTechnicianFromDb();
            if (!App.Database.HasDataConnection())
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("No data connection presently, please check and try again.", "Connectivity Issue", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }
            else {
                App.Database.CreateGlobalTables(); 
                // dch rkl 01/20/2017 After database is refreshed, reset current technician based on the latest values pulled from JobOps               
                currentTechnician = App.Database.GetTechnician(currentTechnician.TechnicianDeptNo, currentTechnician.TechnicianNo);
                App.Database.SaveTechnicianAsCurrent(currentTechnician);
                App.Database.CreateDependentTables(currentTechnician);
            }
            //App.Database.CreateDependentTables(currentTechnician);
        }
    }
}