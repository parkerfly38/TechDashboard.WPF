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

/**************************************************************************************************
 * Page Name    ExpenseListPage
 * Description: Application Settings
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/12/2016   DCH     Change the description to a label instead of a textbox, since this is not
 *                      an input/edit screen
 * 10/26/2016   DCH     Standardize page font sizes, colors and buttons and alignment of data, labels
 **************************************************************************************************/

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for ExpenseListPage.xaml
    /// </summary>
    public partial class ExpensesListPage : UserControl
    {
        ExpensesListPageViewModel _vm;
        Label _labelTitle;
        ListView _listViewExpenses;
        ComboBox _pickerScheduledAppointment;

        public ExpensesListPage()
        {
            _vm = new ExpensesListPageViewModel();
            InitializeComponent();
            SetPageDisplay();
        }

        public ExpensesListPage(App_WorkTicket workTicket)
        {
            _vm = new ExpensesListPageViewModel(workTicket);
            InitializeComponent();
            SetPageDisplay();
        }
        
        public void SetPageDisplay()
        {
            // Set the page title.
            //Title = "Expenses";
            SolidColorBrush asbestos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7f8C8d"));

            //  Create a label for the expenses list
            _labelTitle = new Label();
            _labelTitle.Content = "EXPENSES";
            _labelTitle.FontWeight = FontWeights.Bold;
            _labelTitle.FontSize = 18;
            _labelTitle.Foreground = new SolidColorBrush(Colors.White);
            _labelTitle.HorizontalAlignment = HorizontalAlignment.Center;
            _labelTitle.VerticalAlignment = VerticalAlignment.Center;

            Grid titleLayout = new Grid()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498db")), //Color.FromHex("#2980b9"),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 80
            };
            titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            titleLayout.Children.Add(_labelTitle);
            Grid.SetColumn(_labelTitle, 0);
            Grid.SetRow(_labelTitle, 0);

            _pickerScheduledAppointment = new ComboBox();
            //_pickerScheduledAppointment.Title = "WORK TICKET";
            _pickerScheduledAppointment.ItemsSource = _vm.GetScheduledAppointments();
            //_pickerScheduledAppointment.SetBinding(BindablePicker.DisplayPropertyProperty, "FormattedTicketNumber");
            _pickerScheduledAppointment.DisplayMemberPath = "ServiceTicketNumber";
            _pickerScheduledAppointment.SelectionChanged += PickerScheduledAppointmentt_SelectedIndexChanged;
            for (int i = 0; i < _pickerScheduledAppointment.Items.Count; i++)
            {
                if (_vm.WorkTicket != null && (((App_ScheduledAppointment)_pickerScheduledAppointment.Items[i]).ServiceTicketNumber == _vm.WorkTicket.FormattedTicketNumber))
                {
                    _pickerScheduledAppointment.SelectedIndex = i;
                    break;
                }
            }

            // Create our screen objects

            Button buttonAddExpense = new Button();

            TextBlock addExpenseText = new TextBlock()
            {
                Text = "ADD",
                Foreground = new SolidColorBrush(Colors.White),
                FontWeight = FontWeights.Bold
            };
            buttonAddExpense.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2EcC71"));
            buttonAddExpense.HorizontalAlignment = HorizontalAlignment.Stretch;
            buttonAddExpense.Margin = new Thickness(10);
            buttonAddExpense.Content = addExpenseText;
            buttonAddExpense.Click += ButtonAddEditExpense_Clicked;

            // dch rkl 10/26/2016
            buttonAddExpense.Margin = new Thickness(30, 5, 30, 0);
            buttonAddExpense.Height = 40;

            // Create a template to display each technician in the list
            //var dataTemplateExpense = new DataTemplate(typeof(ExpenseDataCell));

            // Create the actual list
            _listViewExpenses = new ListView()
            {
                //HasUnevenRows = true,
                ItemsSource = _vm.ExpensesList,
                ItemTemplate = (DataTemplate)this.Resources["ExpenseDataTemplate"]
            };
            _listViewExpenses.DataContext = _vm.ExpensesList;
            _listViewExpenses.SelectionChanged += _listViewExpenses_SelectionChanged; ;


            gridMain.Children.Add(new StackPanel()
            {
                //Padding = 30,
                Children = {
                    titleLayout,
                    _pickerScheduledAppointment,
                    _listViewExpenses,
                    buttonAddExpense
                }
            });
        }

        private void _listViewExpenses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App_Expense expense = (App_Expense)_listViewExpenses.SelectedItem;

            //_listViewExpenses.SelectedItem = null;

            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new ExpensesEditPage(expense);
        }

        protected void PickerScheduledAppointmentt_SelectedIndexChanged(object sender, EventArgs e)
        {
            App_ScheduledAppointment selectedAppointment = null;

            // I've found that the picker object will remove duplicate entries when displaying on-screen data, 
            //  so I need to do a little more work to find the proper object selected.  Will revisit code for 
            //  future improvements.
            for (int i = 0; i < _pickerScheduledAppointment.Items.Count; i++)
            {
                selectedAppointment = _pickerScheduledAppointment.Items[i] as App_ScheduledAppointment;

                if (selectedAppointment.ServiceTicketNumber == ((App_ScheduledAppointment)_pickerScheduledAppointment.Items[_pickerScheduledAppointment.SelectedIndex]).ServiceTicketNumber)
                {
                    _vm.SetWorkTicket(selectedAppointment.ServiceTicketNumber);
                    break;
                }
            }
        }

        private void ButtonAddEditExpense_Clicked(object sender, EventArgs e)
        {
            // Make sure that some sort of work ticket was selected!  If not, we can't move 
            //  forward.
            if (_pickerScheduledAppointment.SelectedIndex < 0)
            {
                //await DisplayAlert("Select Work Ticket", "Please select a work ticket for your expenses.", "OK");
                var result = MessageBox.Show("Please select a service ticket for your expenses.", "Select Service Ticket", MessageBoxButton.OK);
                return;
            }
            // Let's add a new expense item
            //await Navigation.PushAsync(new ExpensesEditPage(_vm.WorkTicket));
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new ExpensesEditPage(_vm.WorkTicket);
        }

    }
}
