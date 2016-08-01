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
using TechDashboard.ViewModels;
using TechDashboard.Models;

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for TechnicianPage.xaml
    /// </summary>
    public partial class TechnicianPage : UserControl
    {
        TechnicianPageViewModel _vm;
        Label _labelTitle;
        Label _labelTechnicianNo;
        Label _labelFirstName;
        Label _labelLastName;
        ComboBox _pickerTechnicianStatus;
        Button _buttonOK;

        public TechnicianPage()
        {

            InitializeComponent();

            //  Create a label for the technician list
            _labelTitle = new Label();
            _labelTitle.Content = "TECHNICIAN DETAILS";
            _labelTitle.FontWeight = FontWeights.Bold;
            _labelTitle.FontSize = 22;
            _labelTitle.Foreground = new SolidColorBrush(Colors.White);
            _labelTitle.HorizontalAlignment = HorizontalAlignment.Center;
            _labelTitle.VerticalAlignment = VerticalAlignment.Center;

            Grid titleLayout = new Grid()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB")),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 80
            };
            titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            titleLayout.Children.Add(_labelTitle);
            Grid.SetColumn(_labelTitle, 0);
            Grid.SetRow(_labelTitle, 0);

            _vm = new TechnicianPageViewModel();

            SolidColorBrush asbestos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7f8c8d"));
            this.DataContext = _vm.Technician;

            _labelTechnicianNo = new Label();
            _labelTechnicianNo.Foreground = asbestos;
            _labelTechnicianNo.SetBinding(ContentProperty, "FormattedTechnicianNumber");

            _labelFirstName = new Label();
            _labelFirstName.Foreground = asbestos;
            _labelFirstName.SetBinding(ContentProperty, "FirstName");

            _labelLastName = new Label();
            _labelLastName.Foreground = asbestos;
            _labelLastName.SetBinding(ContentProperty, "LastName");

            _pickerTechnicianStatus = new ComboBox { ItemsSource = _vm.TechnicianStatusList };
            _pickerTechnicianStatus.DisplayMemberPath =  "StatusDescription";
            SetPickerTechnicianStatus();

            _buttonOK = new Button();
            _buttonOK.Content = "OK";
            _buttonOK.Height = 50;
            _buttonOK.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB"));
            _buttonOK.Foreground = new SolidColorBrush(Colors.White);
            _buttonOK.Click += ButtonOK_Clicked;

            gridMain.Children.Add(new StackPanel
            {

                Children = {
                    titleLayout,
     //               new Xamarin.Forms.Label 
					//{ Text = "TECHNICIAN DETAILS", FontFamily = Device.OnPlatform("OpenSans-Bold", null, null), TextColor = asbestos },
                    _labelTechnicianNo,
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,

                        Children =
                        {
                            _labelFirstName,
                            new Label { Content = ", ", Foreground = asbestos },
                            _labelLastName,
                        }
                    },
                    _pickerTechnicianStatus,
                    _buttonOK
                }
            });
        }

        protected async void ButtonOK_Clicked(object sender, EventArgs e)
        {
            JT_TechnicianStatus selectedStatus = _pickerTechnicianStatus.SelectedItem as JT_TechnicianStatus;
            if (selectedStatus.StatusCode != _vm.Technician.CurrentStatus)
            {
                // update the status code
                _vm.UpdateTechnicianStatus(selectedStatus);
            }

            ContentControl contentArea = (ContentControl)this.Parent;
            
            if (App.Database.GetCurrentWorkTicket() == null)
            {
                contentArea.Content = null;
            }
            else
            {
                contentArea.Content = new TicketDetailsPage();
            }
        }

        protected void SetPickerTechnicianStatus()
        {
            // dch rkl 05/15/2016 before checking the technician status, make sure the technician is not null BEGIN
            if (_vm.Technician != null)
            {
                // dch rkl 05/15/2016 before checking the technician status, make sure the technician is not null END
                foreach (JT_TechnicianStatus status in _pickerTechnicianStatus.ItemsSource)
                {
                    if (status.StatusCode == _vm.Technician.CurrentStatus)
                    {
                        _pickerTechnicianStatus.SelectedItem = status;
                        return;
                    }
                }
                // dch rkl 05/15/2016 before checking the technician status, make sure the technician is not null BEGIN
            }
            else
            {
                // Go to select technician page
                ContentControl contentArea = (ContentControl)this.Parent;
                contentArea.Content = new TechnicianListPage();
            }
            // dch rkl 05/15/2016 before checking the technician status, make sure the technician is not null END
        }
    }
}
