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
using TechDashboard.Models;
using TechDashboard.ViewModels;

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for PartsAddPage.xaml
    /// </summary>
    public partial class PartsAddPage : UserControl
    {
        PartsAddPageViewModel _vm;
        ListView itemsList;
        TextBox searchBarText;
        //Button searchBarButton;
        Label labelTitle;
        App_ScheduledAppointment _scheduledAppointment;

        public PartsAddPage(App_WorkTicket workTicket, App_ScheduledAppointment scheduledAppointment)
        {
            _vm = new PartsAddPageViewModel(workTicket);
            _vm.FilterItemList("ZXZXZXZXZX128391");
            _scheduledAppointment = scheduledAppointment;
            InitializeComponent();

            labelTitle = new Label();
            labelTitle.Content = "SELECT PART";
            labelTitle.FontWeight = FontWeights.Bold;
            labelTitle.FontSize = 22;
            labelTitle.Foreground = new SolidColorBrush(Colors.White);
            labelTitle.HorizontalAlignment = HorizontalAlignment.Center;
            labelTitle.VerticalAlignment = VerticalAlignment.Center;

            Grid titleLayout = new Grid()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498db")),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 80
            };
            titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            titleLayout.Children.Add(labelTitle);
            Grid.SetColumn(labelTitle, 0);
            Grid.SetRow(labelTitle, 0);

            // create a list to hold our items
            itemsList = new ListView()
            {
                ItemsSource = _vm.ItemList,
                ItemTemplate = (DataTemplate)this.Resources["PartsListDataTemplate"]
            };
            itemsList.MaxHeight = 300;
            itemsList.SelectionChanged += ItemsList_SelectionChanged; ;
            searchBarText = new TextBox();
            searchBarText.HorizontalAlignment = HorizontalAlignment.Stretch;
            searchBarText.Width = 200;
            searchBarText.Margin = new Thickness(10);
            // put a search bar on the page to filter the items list
            //_searchBarItems = new SearchBar();
            //_searchBarItems.Placeholder = "Search Item Code or Desc";
            //_searchBarItems.TextChanged += SearchBarItems_TextChanged;
            //_searchBarItems.SearchCommand = new Command(() => { _vm.FilterItemList(_searchBarItems.Text); });
            //searchBarText.TextChanged += SearchBarText_TextChanged;
            Button buttonSearch = new Button()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB")),
                Foreground = new SolidColorBrush(Colors.White),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB")),
                Content = "SEARCH",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(10)
            };
            buttonSearch.Click += ButtonSearch_Click;

            // create a "cancel" button to go back
            Button buttonAddPart = new Button()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C")),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(30, 10, 30, 10)
            };
            TextBlock buttonAddPartText = new TextBlock()
            {
                Text = "CANCEL",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White),
            };
            buttonAddPart.Content = buttonAddPartText;
            buttonAddPart.Click += buttonAddPart_Click;

            // put it all together on the page
            gridMain.Children.Add(new StackPanel()
            {
                Children = {
                    titleLayout,
                    new StackPanel()
                    {
                        Orientation = Orientation.Horizontal,
                        Children =
                        {
                            searchBarText,
                            buttonSearch
                        }
                    },
                    itemsList,
                    buttonAddPart
                },
                CanVerticallyScroll = true
            });
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (searchBarText.Text == null || searchBarText.Text.Length == 0)
            {
                _vm.FilterItemList(null);
            }
            else
            {
                _vm.FilterItemList(searchBarText.Text);
            }
        }

        private void buttonAddPart_Click(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new TicketDetailsPage(_scheduledAppointment);
        }

        private void ItemsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App_RepairPart part = new App_RepairPart((App_Item)itemsList.Items[itemsList.SelectedIndex], _vm.WorkTicket);
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new PartsEditPage(_vm.WorkTicket, part, PartsEditPage.PageMode.Add, _scheduledAppointment);
        }
    }
}
