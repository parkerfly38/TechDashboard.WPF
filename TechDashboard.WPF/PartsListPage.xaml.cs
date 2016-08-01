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
    /// Interaction logic for PartsListPage.xaml
    /// </summary>
    public partial class PartsListPage : UserControl
    {
        PartsListPageViewModel _vm;

        Label _labelTitle;
        ListView _listViewParts;
        App_ScheduledAppointment _scheduledAppointment;

        public PartsListPage(App_WorkTicket workTicket, App_ScheduledAppointment scheduledAppointment)
        {
            _vm = new PartsListPageViewModel(workTicket);
            _scheduledAppointment = scheduledAppointment;
            InitializeComponent();
            SetPageDisplay();
        }

        protected void SetPageDisplay()
        {

            // Set the page title.
            //Title = "Parts List";

            SolidColorBrush asbestos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7f8C8d"));
            gridMain.DataContext = _vm.ObservablePartsList;

            //  Create a label for the technician list
            _labelTitle = new Label();
            _labelTitle.Content = "PARTS LIST";
            _labelTitle.FontWeight = FontWeights.Bold;
            _labelTitle.FontSize = 22;
            _labelTitle.Foreground = new SolidColorBrush(Colors.White);
            _labelTitle.HorizontalAlignment = HorizontalAlignment.Center;
            _labelTitle.VerticalAlignment = VerticalAlignment.Center;

            Button btnBack = new Button();

            StackPanel spBackButton = new StackPanel();
            Image imBackButton = new Image();
            imBackButton.Source = new BitmapImage(new Uri("Resources/arrow-111-64.png", UriKind.Relative));
            imBackButton.Height = 32;
            imBackButton.Width = 32;
            spBackButton.Children.Add(imBackButton);
            btnBack.Content = spBackButton;
            btnBack.HorizontalAlignment = HorizontalAlignment.Left;
            btnBack.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3498DB"));
            btnBack.BorderBrush = null;
            btnBack.Width = 32;

            btnBack.Click += BtnBack_Click;

            Grid titleLayout = new Grid()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3498DB")),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 80
            };
            titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            titleLayout.Children.Add(_labelTitle);
            Grid.SetColumn(_labelTitle, 0);
            Grid.SetRow(_labelTitle, 0);
            titleLayout.Children.Add(btnBack);
            Grid.SetColumn(btnBack, 0);
            Grid.SetRow(btnBack, 1);

            _listViewParts = new ListView()
            {
                ItemsSource = _vm.PartsList,
                ItemTemplate = (DataTemplate)this.Resources["PartsListDataTemplate"]
            };

            Button buttonAddEditPart = new Button()
            {
                Content = "ADD NEW PART",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ECC71")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ECC71"))
            };

            buttonAddEditPart.Click += ButtonAddEditPart_Clicked;

            //_listViewParts.ItemTapped += ListViewWorkTickets_ItemTapped;  // puke... only do if clocked in on ticket
            _listViewParts.SelectionChanged += _listViewParts_SelectionChanged;


            Content = new StackPanel
            {
                Children = {
                    titleLayout,
                    _listViewParts,
                    buttonAddEditPart
                }
            };
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new TicketDetailsPage(_scheduledAppointment);
        }

        private void _listViewParts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App_RepairPart part = (App_RepairPart)_listViewParts.SelectedItem; //e.Item as App_RepairPart;
            CI_Item partAsItem = App.Database.GetItemFromDB(part.PartItemCode);

            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new PartsEditPage(_vm.WorkTicket, part, PartsEditPage.PageMode.Edit, _scheduledAppointment);
        }

        private void ButtonAddEditPart_Clicked(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new PartsAddPage(_vm.WorkTicket, _scheduledAppointment);
        }
    }
}
