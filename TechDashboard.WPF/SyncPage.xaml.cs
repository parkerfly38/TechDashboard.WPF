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
                Content = "REFRESH APP DATA",
                Foreground = asbestos,
                FontWeight = FontWeights.Bold
            };

            TextBlock labelUpdateWarning = new TextBlock()
            {
                Text = "This will refresh the data on your application, using the time frame that can be adjusted in Settings.  It requires either mobile data or WiFi connectivity.",
                TextWrapping = TextWrapping.Wrap,
                Foreground = asbestos,
                FontWeight = FontWeights.Bold
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
            buttonUpdateData.Content = "UPDATE APP DATA";
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
                Text = "This will send data from Tech Dashboard to JobOps.  This operations requires either mobile data or WiFi connections.",
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
                Content = "records awaiting sync.",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Red)
            };

            Label labelLastSyncDate = new Label()
            {
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Red),
                Margin = new Thickness(30, 10, 30, 10)
            };
            labelLastSyncDate.Content = _vm.LastSyncDate;

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
            buttonSendData.Content = "SYNC DATA";
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
                        VerticalAlignment = VerticalAlignment.Center
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
            _vm.syncWithServer();
        }

        void ButtonUpdateData_Clicked(object sender, EventArgs e)
        {
            JT_Technician currentTechnician = App.Database.GetCurrentTechnicianFromDb();
            App.Database.CreateGlobalTables();
            App.Database.SaveTechnicianAsCurrent(currentTechnician);
            App.Database.CreateDependentTables(currentTechnician);
            //App.Database.CreateDependentTables(currentTechnician);
        }
    }
}