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

/**************************************************************************************************
 * Page Name    TechnicianPage
 * Description: Technician Details
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/26/2016   DCH     Move all controls to the .xaml page instead of generating with code
 * 10/26/2016   DCH     Standardize page font sizes, colors and buttons and alignment of data, labels.
 ***************************************************************************************************/
namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for TechnicianPage.xaml
    /// </summary>
    public partial class TechnicianPage : UserControl
    {
        TechnicianPageViewModel _vm;
        //Label _labelTitle;
        //Label _labelTechnicianNo;
        //Label _labelFirstName;
        //Label _labelLastName;
        //ComboBox _pickerTechnicianStatus;
        //Button _buttonOK;

        public TechnicianPage()
        {

            InitializeComponent();

            //  Create a label for the technician list
            //_labelTitle = new Label();
            //_labelTitle.Content = "TECHNICIAN DETAILS";
            //_labelTitle.FontWeight = FontWeights.Bold;
            //_labelTitle.FontSize = 18;
            //_labelTitle.Foreground = new SolidColorBrush(Colors.White);
            //_labelTitle.HorizontalAlignment = HorizontalAlignment.Center;
            //_labelTitle.VerticalAlignment = VerticalAlignment.Center;

            //Grid titleLayout = new Grid()
            //{
            //    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB")),
            //    //HorizontalAlignment = HorizontalAlignment.Stretch,
            //    Height = 100
            //};
            //titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            //titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //titleLayout.Children.Add(_labelTitle);
            //Grid.SetColumn(_labelTitle, 0);
            //Grid.SetRow(_labelTitle, 0);

            _vm = new TechnicianPageViewModel();

            SolidColorBrush asbestos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7f8c8d"));
            this.DataContext = _vm.Technician;

            lblTechDeptNumber.Content = _vm.Technician.FormattedTechnicianNumber;
            lblTechnicianName.Content = String.Format("{0} {1}", _vm.Technician.FirstName, _vm.Technician.LastName);

            pkrTechnicianStatus.ItemsSource = _vm.TechnicianStatusList;

            //_labelTechnicianNo = new Label();
            //_labelTechnicianNo.Foreground = asbestos;
            //_labelTechnicianNo.SetBinding(ContentProperty, "FormattedTechnicianNumber");

            //_labelFirstName = new Label();
            //_labelFirstName.Foreground = asbestos;
            //_labelFirstName.SetBinding(ContentProperty, "FirstName");

            //_labelLastName = new Label();
            //_labelLastName.Foreground = asbestos;
            //_labelLastName.SetBinding(ContentProperty, "LastName");

            //_pickerTechnicianStatus = new ComboBox { ItemsSource = _vm.TechnicianStatusList };
            //_pickerTechnicianStatus.DisplayMemberPath =  "StatusDescription";
            SetPickerTechnicianStatus();

            //_buttonOK = new Button();
            //_buttonOK.Content = "OK";
            //_buttonOK.Height = 50;
            //_buttonOK.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB"));
            //_buttonOK.Foreground = new SolidColorBrush(Colors.White);
            //_buttonOK.Click += ButtonOK_Clicked;

            //gridMain.Children.Add(new StackPanel
            //{ 
            //    Margin = new Thickness(30, 10, 30, 10),
            //    Children = {
                    //titleLayout,
                    //_labelTechnicianNo,
                    //new StackPanel
                    //{
                    //    Orientation = Orientation.Horizontal,
                    //    Children =
                    //    {
                    //        _labelFirstName,
                    //        new Label { Content = ", ", Foreground = asbestos },
                    //        _labelLastName,
                    //    }
                    //},
                    //_pickerTechnicianStatus,
                    //_buttonOK
            //    }
            //});
        }

        protected async void ButtonOK_Clicked(object sender, EventArgs e)
        {
            JT_TechnicianStatus selectedStatus = pkrTechnicianStatus.SelectedItem as JT_TechnicianStatus;
            if (selectedStatus.StatusCode != _vm.Technician.CurrentStatus && App.Database.GetCurrentWorkTicket() != null)
            {
                // update the status code
                _vm.UpdateTechnicianStatus(selectedStatus);
            }

            ContentControl contentArea = (ContentControl)this.Parent;
            
            if (App.Database.GetCurrentWorkTicket() == null)
            {
                contentArea.Content = new SchedulePage();
            }
            else
            {
                contentArea.Content = new TicketDetailsPage(App.Database.GetScheduledAppointment());
            }
        }

        protected void SetPickerTechnicianStatus()
        {
            // dch rkl 05/15/2016 before checking the technician status, make sure the technician is not null BEGIN
            if (_vm.Technician != null)
            {
                // dch rkl 05/15/2016 before checking the technician status, make sure the technician is not null END
                foreach (JT_TechnicianStatus status in pkrTechnicianStatus.ItemsSource)
                {
                    if (status.StatusCode == _vm.Technician.CurrentStatus)
                    {
                        pkrTechnicianStatus.SelectedItem = status;
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
