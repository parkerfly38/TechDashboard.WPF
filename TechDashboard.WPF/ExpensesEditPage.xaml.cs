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
 * Page Name    ExpenseEditPage
 * Description: Application Settings
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/12/2016   DCH     Make sure all labels are in lower case, without colon at the end;
 *                      The description label was not appearing above the description textbox;
 *                      Display the service ticket number at the top of the page;
 * 11/21/2016   DCH     Category can be blank
 * 11/22/2016   DCH     Select all text in textbox on Focus
 **************************************************************************************************/

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for ExpensesEditPage.xaml
    /// </summary>
    public partial class ExpensesEditPage : UserControl
    {
        ExpensesEditPageViewModel _vm;

        DatePicker _datePickerExpenseDate;
        ComboBox _pickerCategory;
        ComboBox _pickerChargeCode;
        TextBox _entryQuantity;
        TextBox _entryUnitOfMeasure;
        TextBox _entryUnitCost;
        TextBox _entryTotalCost;
        TextBox _entryUnitPrice;
        TextBox _entryMarkupPercentage;
        TextBox _entryTotalPrice;
        CheckBox _switchIsReimbursable;
        CheckBox _switchIsChargeCustomer;
        TextBox _editorDescription;
        Button _buttonAddEditExpense;
        Button _buttonCancel;
        Button _buttonDelete;         // dch rkl 10/14/2016 When updating, add option to delete expense, only if it is in the JT_TransactionImportDetail table
        Label _labelTitle;

        // dch rkl 10/12/2016
        string workticket = "";

        public ExpensesEditPage(App_Expense expense)
        {
            //_expense = expense;

            // dch rkl 10/12/2016 
            workticket = expense.WorkTicket.FormattedTicketNumber;

            _vm = new ExpensesEditPageViewModel(expense);
            InitializeComponent();
            Initialize();
        }

        public ExpensesEditPage(App_WorkTicket workTicket)
        {
            // dch rkl 10/12/2016 
            workticket = workTicket.FormattedTicketNumber;

            //_expense = new App_Expense();
            _vm = new ExpensesEditPageViewModel(workTicket);
            InitializeComponent();
            Initialize();
        }

        protected void Initialize()
        {
            // Set the page title.
            //Title = "Add Expense";
            SolidColorBrush asbestos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7f8C8d"));
            //_labelHeading = new Xamarin.Forms.Label();

            //  Create a label for the expense page
            _labelTitle = new Label();
            if (_vm.ExpenseId > 0)
            {
                _labelTitle.Content = "EDIT EXPENSE";
            }
            else
            {
                _labelTitle.Content = "ADD EXPENSE";
            }

            _labelTitle.FontWeight = FontWeights.Bold;
            _labelTitle.FontSize = 22;
            _labelTitle.Foreground = new SolidColorBrush(Colors.White);
            _labelTitle.HorizontalAlignment = HorizontalAlignment.Center;
            _labelTitle.VerticalAlignment = VerticalAlignment.Center;

            Grid titleLayout = new Grid()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498db")),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 80
            };
            titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            titleLayout.Children.Add(_labelTitle);
            Grid.SetColumn(_labelTitle, 0);
            Grid.SetRow(_labelTitle, 0);

            _datePickerExpenseDate = new DatePicker();
            _datePickerExpenseDate.SelectedDate = _vm.ExpenseDate;
            _datePickerExpenseDate.Width = 100;     // dch rkl 11/22/2016

            Label labelCategory = new Label()
            {
                Content = "CATEGORY",
                Foreground = asbestos,
                FontWeight = FontWeights.Bold
            };
            _pickerCategory = new ComboBox { };

            // dch rkl 10/14/2016 Show Code + Description in Expense Category Dropdown BEGIN
            List<JT_MiscellaneousCodes> lsMiscCodes = App.Database.GetExpenseCategoriesWithDesc();
            foreach (JT_MiscellaneousCodes code in lsMiscCodes)
            {
                _pickerCategory.Items.Add(string.Format("{0} - {1}", code.MiscellaneousCode, code.Description));
            }
            //foreach (string s in _vm.ExpenseCategories)
            //{
            //    _pickerCategory.Items.Add(s);
            //}
            // dch rkl 10/14/2016 Show Code + Description in Expense Category Dropdown END
            //_pickerCategory.BindingContext = _vm.ExpenseCategory; 
            _pickerCategory.SelectionChanged += _pickerCategory_SelectionChanged;

            Label labelChargeCode = new Label()
            {
                Content = "CHARGE CODE",
                Foreground = asbestos,
                FontWeight = FontWeights.Bold
            };
            _pickerChargeCode = new ComboBox() { };

            // dch rkl 10/14/2016 Show Code + Description in Charge Code Dropdown BEGIN
            List<string> lsChgCodes = App.Database.GetExpenseChargeCodesWithDesc();
            foreach (string chgCode in lsChgCodes)
            {
                _pickerChargeCode.Items.Add(chgCode);
            }
            //foreach (string s in _vm.ExpenseChargeCodes)
            //{
            //    _pickerChargeCode.Items.Add(s);
            //}
            //_pickerChargeCode.BindingContext = _vm.ExpenseChargeCode;
            _pickerChargeCode.SelectionChanged += _pickerChargeCode_SelectionChanged;

            //         _labelHeading.Text = "ADD EXPENSE";
            //_labelHeading.FontFamily = Device.OnPlatform("OpenSans-Bold", null, null);
            //_labelHeading.TextColor = asbestos;

            Label labelQuantity = new Label()
            {
                Content = "QTY",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };
            _entryQuantity = new TextBox();
            _entryQuantity.TextChanged += EntryQuantity_TextChanged;
            _entryQuantity.GotFocus += textbox_GotFocus;        // dch rkl 11/22/2016 select full text on focus

            Label labelUoM = new Label()
            {
                Content = "U/M",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            _entryUnitOfMeasure = new TextBox();
            _entryUnitOfMeasure.MaxLength = 4;
            _entryUnitOfMeasure.GotFocus += textbox_GotFocus;        // dch rkl 11/22/2016 select full text on focus

            Label labelUnitCost = new Label()
            {
                Content = "Unit Cost",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            _entryUnitCost = new TextBox();
            _entryUnitCost.TextChanged += EntryUnitCost_TextChanged;
            _entryUnitCost.GotFocus += textbox_GotFocus;        // dch rkl 11/22/2016 select full text on focus

            _entryTotalCost = new TextBox();
            _entryTotalCost.Text = "0.00";
            _entryTotalCost.IsEnabled = false;

            _entryUnitPrice = new TextBox();
            _entryUnitPrice.Text = "0.00";
            _entryUnitPrice.TextChanged += EntryUnitPrice_TextChanged;
            _entryUnitPrice.GotFocus += textbox_GotFocus;        // dch rkl 11/22/2016 select full text on focus

            _entryTotalPrice = new TextBox();
            _entryTotalPrice.Text = "";
            _entryTotalPrice.IsEnabled = false;

            _entryMarkupPercentage = new TextBox();
            _entryMarkupPercentage.Text = "0.00";
            _entryMarkupPercentage.TextChanged += EntryMarkupPercentage_TextChanged;
            _entryMarkupPercentage.GotFocus += textbox_GotFocus;        // dch rkl 11/22/2016 select full text on focus

            _switchIsReimbursable = new CheckBox();
            //_switchIsReimbursable.IsToggled = _vm.ExpenseIsReimbursable;

            _switchIsChargeCustomer = new CheckBox();


            _editorDescription = new TextBox();
            _editorDescription.AcceptsReturn = true;
            _editorDescription.TextWrapping = TextWrapping.WrapWithOverflow;
            //_editorDescription.Text = _vm.ExpenseBillingDescription;

            _buttonAddEditExpense = new Button();
            _buttonAddEditExpense.Click += ButtonAddEditExpense_Click;
            _buttonAddEditExpense.FontWeight = FontWeights.Bold;
            TextBlock addEditExpenseText = new TextBlock();
            addEditExpenseText.Text = "ADD";
            _buttonAddEditExpense.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ECC71"));
            addEditExpenseText.Foreground = new SolidColorBrush(Colors.White);
            _buttonAddEditExpense.HorizontalAlignment = HorizontalAlignment.Stretch;
            _buttonAddEditExpense.Margin = new Thickness(10);
            _buttonAddEditExpense.Content = addEditExpenseText;
            _buttonAddEditExpense.Height = 40;   // dch rkl 11/22/2016
            _buttonAddEditExpense.Width = 175;   // dch rkl 11/22/2016

            _buttonCancel = new Button();
            _buttonCancel.Click += ButtonCancel_Click;
            TextBlock buttonCancelText = new TextBlock();
            buttonCancelText.Text = "CANCEL";
            buttonCancelText.FontWeight = FontWeights.Bold;
            buttonCancelText.Foreground = new SolidColorBrush(Colors.White);
            _buttonCancel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C"));
            _buttonCancel.HorizontalAlignment = HorizontalAlignment.Stretch;
            _buttonCancel.Margin = new Thickness(10);
            _buttonCancel.Content = buttonCancelText;
            _buttonCancel.Height = 40;   // dch rkl 11/22/2016
            _buttonCancel.Width = 175;   // dch rkl 11/22/2016

            // dch rkl 10/14/2016 When updating, add option to delete expense, only if it is in the JT_TransactionImportDetail table
            _buttonDelete = new Button();
            _buttonDelete.Click += ButtonDelete_Click;
            TextBlock buttonDeleteText = new TextBlock();
            buttonDeleteText.Text = "DELETE";
            buttonDeleteText.FontWeight = FontWeights.Bold;
            buttonDeleteText.Foreground = new SolidColorBrush(Colors.White);
            _buttonDelete.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C"));
            _buttonDelete.HorizontalAlignment = HorizontalAlignment.Stretch;
            _buttonDelete.Margin = new Thickness(10);
            _buttonDelete.Content = buttonDeleteText;
            _buttonDelete.Height = 40;   // dch rkl 11/22/2016
            _buttonDelete.Width = 175;   // dch rkl 11/22/2016
            if (_vm.ExpenseId == 0) { _buttonDelete.Visibility = Visibility.Hidden;}

            if (_vm.ExpenseId > 0)
            {
                addEditExpenseText.Text = "UPDATE";

                for (int i = 0; i < _pickerCategory.Items.Count; i++)
                {
                    // dch rkl 10/14/2016 Show Code + Description in Expense Category Dropdown BEGIN
                    string pickerCategory = "";
                    try { pickerCategory = _pickerCategory.Items[i].ToString().Split(' ')[0].ToString().Trim(); }
                    catch (Exception ex) { }
                    //if ((string)_pickerCategory.Items[i] == _vm.ExpenseCategory)
                    if (pickerCategory == _vm.ExpenseCategory)
                    // dch rkl 10/14/2016 Show Code + Description in Expense Category Dropdown END
                    {
                        _pickerCategory.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < _pickerChargeCode.Items.Count; i++)
                {
                    // dch rkl 10/14/2016 Show Code + Description in Charge Code Dropdown BEGIN
                    string pickerChargeCode = "";
                    try { pickerChargeCode = _pickerChargeCode.Items[i].ToString().Split(' ')[0].ToString().Trim(); }
                    catch (Exception ex) { }
                    //if ((string)_pickerChargeCode.Items[i] == _vm.ExpenseChargeCode)
                    if (pickerChargeCode == _vm.ExpenseChargeCode)
                    // dch rkl 10/14/2016 Show Code + Description in Charge Code Dropdown END
                    {
                        _pickerChargeCode.SelectedIndex = i;
                        break;
                    }
                }

                _entryQuantity.Text = _vm.ExpenseQuantity.ToString();
                _entryUnitOfMeasure.Text = _vm.ExpenseUnitOfMeasure;
                _entryUnitCost.Text = _vm.ExpenseUnitCost.ToString();
                _entryUnitPrice.Text = _vm.ExpenseUnitPrice.ToString();
                _switchIsReimbursable.IsChecked = _vm.ExpenseIsReimbursable;
                _editorDescription.Text = _vm.ExpenseBillingDescription;
            }

            Grid topGrid = new Grid();
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // dch rkl 10/12/2016 service ticket row 0
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // date 1
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // expense category 2
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // charge code 3
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // quantity and U/M 4
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // unit cost and total cost 5 
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // unit price and total price 6
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // markup 7
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // reimburse employee 8
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // charge customer 9
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });     // dch rkl 10/12/1016 billing description label 10 
            topGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) }); // description 11
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200, GridUnitType.Pixel) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Pixel) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150, GridUnitType.Pixel) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Pixel) });

            // dch rkl 10/12/2016 Make all labels lower case instead of upper case

            // dch rkl 10/12/2016 display Service Ticket at top. BEGIN
            Label labelSvcTicket = new Label()
            {
                Content = "Service Ticket",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };
            Label labelSvcTicketData = new Label()
            {
                Content = workticket,
                Foreground = asbestos
            };
            topGrid.Children.Add(labelSvcTicket);
            Grid.SetColumn(labelSvcTicket, 0);
            Grid.SetRow(labelSvcTicket, 0);
            topGrid.Children.Add(labelSvcTicketData);
            Grid.SetColumn(labelSvcTicketData, 1);
            Grid.SetRow(labelSvcTicketData, 0);
            Grid.SetColumnSpan(labelSvcTicketData, 3);
            // dch rkl 10/12/2016 display Service Ticket at top. END

            Label labelDate = new Label()
            {
                Content = "Date",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            topGrid.Children.Add(labelDate);
            Grid.SetColumn(labelDate, 0);
            Grid.SetRow(labelDate, 1);
            topGrid.Children.Add(_datePickerExpenseDate);
            Grid.SetColumn(_datePickerExpenseDate, 1);
            Grid.SetRow(_datePickerExpenseDate, 1);
            Grid.SetColumnSpan(_datePickerExpenseDate, 3);

            Label labelExpenseCat = new Label()
            {
                Content = "Expense Category",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            topGrid.Children.Add(labelExpenseCat);
            Grid.SetColumn(labelExpenseCat, 0);
            Grid.SetRow(labelExpenseCat, 2);
            topGrid.Children.Add(_pickerCategory);
            Grid.SetColumn(_pickerCategory, 1);
            Grid.SetRow(_pickerCategory, 2);
            Grid.SetColumnSpan(_pickerCategory, 3);

            Label labelChargeCodeTitle = new Label()
            {
                Content = "Charge Code",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            topGrid.Children.Add(labelChargeCodeTitle);
            Grid.SetColumn(labelChargeCodeTitle, 0);
            Grid.SetRow(labelChargeCodeTitle, 3);
            topGrid.Children.Add(_pickerChargeCode);
            Grid.SetColumn(_pickerChargeCode, 1);
            Grid.SetRow(_pickerChargeCode, 3);
            Grid.SetColumnSpan(_pickerChargeCode, 3);

            Label labelQuantityTitle = new Label()
            {
                Content = "Quantity",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            topGrid.Children.Add(labelQuantityTitle);
            Grid.SetColumn(labelQuantityTitle, 0);
            Grid.SetRow(labelQuantityTitle, 4);
            topGrid.Children.Add(_entryQuantity);
            Grid.SetColumn(_entryQuantity, 1);
            Grid.SetRow(_entryQuantity, 4);
            topGrid.Children.Add(labelUoM);
            Grid.SetColumn(labelUoM, 2);
            Grid.SetRow(labelUoM, 4);
            topGrid.Children.Add(_entryUnitOfMeasure);
            Grid.SetColumn(_entryUnitOfMeasure, 3);
            Grid.SetRow(_entryUnitOfMeasure, 4);

            topGrid.Children.Add(labelUnitCost);
            Grid.SetColumn(labelUnitCost, 0);
            Grid.SetRow(labelUnitCost, 5);
            topGrid.Children.Add(_entryUnitCost);
            Grid.SetColumn(_entryUnitCost, 1);
            Grid.SetRow(_entryUnitCost, 5);

            Label labelTotalCost = new Label()
            {
                Content = "Total Cost",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            topGrid.Children.Add(labelTotalCost);
            Grid.SetColumn(labelTotalCost, 2);
            Grid.SetRow(labelTotalCost, 5);
            topGrid.Children.Add(_entryTotalCost);
            Grid.SetColumn(_entryTotalCost, 3);
            Grid.SetRow(_entryTotalCost, 5);

            Label labelUnitPrice = new Label()
            {
                Content = "Unit Price",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            topGrid.Children.Add(labelUnitPrice);
            Grid.SetColumn(labelUnitPrice, 0);
            Grid.SetRow(labelUnitPrice, 6);
            topGrid.Children.Add(_entryUnitPrice);
            Grid.SetColumn(_entryUnitPrice, 1);
            Grid.SetRow(_entryUnitPrice, 6);

            Label labelTotalPrice = new Label()
            {
                Content = "Billing Amount",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            topGrid.Children.Add(labelTotalPrice);
            Grid.SetColumn(labelTotalPrice, 2);
            Grid.SetRow(labelTotalPrice, 6);
            topGrid.Children.Add(_entryTotalPrice);
            Grid.SetColumn(_entryTotalPrice, 3);
            Grid.SetRow(_entryTotalPrice, 6);

            Label labelMarkup = new Label()
            {
                Content = "Markup",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            topGrid.Children.Add(labelMarkup);
            Grid.SetColumn(labelMarkup, 0);
            Grid.SetRow(labelMarkup, 7);
            topGrid.Children.Add(_entryMarkupPercentage);
            Grid.SetColumn(_entryMarkupPercentage, 1);
            Grid.SetRow(_entryMarkupPercentage, 7);

            Label labelMarkUpPercent = new Label()
            {
                Content = "%",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };
            topGrid.Children.Add(labelMarkUpPercent);
            Grid.SetColumn(labelMarkUpPercent, 2);
            Grid.SetRow(labelMarkUpPercent, 7);

            Label labelReimbursableHdg = new Label
            {
                Content = "Reimburse Employee",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };
            topGrid.Children.Add(labelReimbursableHdg);
            Grid.SetColumn(labelReimbursableHdg, 0);
            Grid.SetRow(labelReimbursableHdg, 8);
            Grid.SetColumnSpan(labelReimbursableHdg, 2);
            topGrid.Children.Add(_switchIsReimbursable);
            Grid.SetColumn(_switchIsReimbursable, 2);
            Grid.SetRow(_switchIsReimbursable, 8);

            Label labelChargeCustomer = new Label
            {
                Content = "Charge Customer",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            topGrid.Children.Add(labelChargeCustomer);
            Grid.SetColumn(labelChargeCustomer, 0);
            Grid.SetRow(labelChargeCustomer, 9);
            Grid.SetColumnSpan(labelChargeCustomer, 2);
            topGrid.Children.Add(_switchIsChargeCustomer);
            Grid.SetColumn(_switchIsChargeCustomer, 2);
            Grid.SetRow(_switchIsChargeCustomer, 9);

            Label labelDescription = new Label
            {
                Content = "Billing Description",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };
            topGrid.Children.Add(labelDescription);
            Grid.SetColumn(labelDescription, 0);
            Grid.SetRow(labelDescription, 10);
            Grid.SetColumnSpan(labelDescription, 4);

            topGrid.Children.Add(_editorDescription);
            Grid.SetColumn(_editorDescription, 0);
            Grid.SetRow(_editorDescription, 11);
            Grid.SetColumnSpan(_editorDescription, 4);

            gridMain.Children.Add(new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Children =
                {
                    titleLayout,
                    topGrid,
                    new StackPanel
                    {
                        Margin = new Thickness(30),
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Children =
                        {
                            _buttonAddEditExpense,
                            _buttonCancel,
                            _buttonDelete           // dch rkl 10/14/2016
                        }
                    }

                }
            });
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new ExpensesListPage();
        }

        // dch rkl 10/14/2016 Delete - Remove Transaction from JT_TransactionImportDetail Table
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            // Delete Expense
            _vm.DeleteExpenseItem();

            // Return to Expense List Page
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new ExpensesListPage();
        }

        private void ButtonAddEditExpense_Click(object sender, RoutedEventArgs e)
        {
            double quantity;
            double unitPrice;
            double unitCost;

            // dch rkl 11/21/2016 Per Jeanne, do not require a category
            //if (_pickerCategory.SelectedIndex < 0)
            //{
            //    //await DisplayAlert("Category", "Please enter a valid Category.", "OK");
            //    var result = MessageBox.Show("Please entery a valid category.", "Category", MessageBoxButton.OK);
            //    return;
            //}

            try
            {
                quantity = double.Parse(_entryQuantity.Text);
            }
            catch
            {
                //await DisplayAlert("Quantity", "Please enter a valid Quantity.", "OK");
                var result = MessageBox.Show("Please enter a valid quantity.", "Quantity", MessageBoxButton.OK);
                return;
            }
            try
            {
                unitPrice = double.Parse(_entryUnitPrice.Text);
            }
            catch
            {
                //await DisplayAlert("Unit Price", "Please enter a valid Unit Price.", "OK");
                var result = MessageBox.Show("Please enter a valid unit price.", "Unit Price", MessageBoxButton.OK);
                return;
            }
            try
            {
                unitCost = double.Parse(_entryUnitCost.Text);
            }
            catch
            {
                //await DisplayAlert("Unit Cost", "Please enter a valid Unit Cost.", "OK");
                var result = MessageBox.Show("Please enter a valid unit cost.", "Unit Cost", MessageBoxButton.OK);
                return;
            }


            _vm.ExpenseDate = (DateTime)_datePickerExpenseDate.SelectedDate;

            // dch rkl 10/14/2016 Show Code + Description in Expense Category Dropdown BEGIN
            string pickerCategory = "";
            try { pickerCategory = _pickerCategory.Items[_pickerCategory.SelectedIndex].ToString().Split(' ')[0].ToString().Trim(); }
            catch (Exception ex) { }
            //_vm.ExpenseCategory = ((string)_pickerCategory.Items[_pickerCategory.SelectedIndex]);
            _vm.ExpenseCategory = pickerCategory;
            // dch rkl 10/14/2016 Show Code + Description in Expense Category Dropdown END

            // dch rkl 10/14/2016 Show Code + Description in Charge Code Dropdown BEGIN
            string pickerChargeCode = "";
            try { pickerChargeCode = _pickerChargeCode.Items[_pickerChargeCode.SelectedIndex].ToString().Split(' ')[0].ToString().Trim(); }
            catch (Exception ex) { }
            //_vm.ExpenseChargeCode = ((string)_pickerChargeCode.Items[_pickerChargeCode.SelectedIndex]);
            _vm.ExpenseChargeCode = pickerChargeCode;
            // dch rkl 10/14/2016 Show Code + Description in Charge Code Dropdown END

            _vm.ExpenseQuantity = quantity;
            _vm.ExpenseUnitOfMeasure = _entryUnitOfMeasure.Text;
            _vm.ExpenseUnitCost = unitCost;
            _vm.ExpenseUnitPrice = unitPrice;
            _vm.ExpenseIsReimbursable = (bool)_switchIsReimbursable.IsChecked;
            _vm.ExpenseIsChargeableToCustomer = (bool)_switchIsChargeCustomer.IsChecked;
            _vm.ExpenseBillingDescription = _editorDescription.Text;

            _vm.SaveExpenseItem();

            var endresult = MessageBox.Show("Expense saved.", "Expense", MessageBoxButton.OK);
        }

        private void UpdateTotalPrice()
        {
            double quantity = 0;
            double unitPrice = 0;
            double markupPercentage = 0;
            double totalPrice = 0;

            try
            {
                quantity = double.Parse(_entryQuantity.Text);
            }
            catch
            {
                quantity = 0;
            }
            try
            {
                unitPrice = double.Parse(_entryUnitPrice.Text);
            }
            catch
            {
                unitPrice = 0;
            }
            try
            {
                markupPercentage = double.Parse(_entryMarkupPercentage.Text) / 100.0;
                if (markupPercentage == 0)
                {
                    markupPercentage = 1.0;
                }
                else
                {
                    markupPercentage += 1.0;
                }
            }
            catch
            {
                markupPercentage = 1.0;
            }

            totalPrice = (quantity * unitPrice * markupPercentage);
            _entryTotalPrice.Text = totalPrice.ToString("#.00");
        }

        private void UpdateTotalCost()
        {
            double quantity = 0;
            double unitCost = 0;
            double totalCost = 0;
            try
            {
                quantity = double.Parse(_entryQuantity.Text);
            }
            catch
            {
                quantity = 0;
            }
            try
            {
                unitCost = double.Parse(_entryUnitCost.Text);
            }
            catch
            {
                unitCost = 0;
            }

            totalCost = quantity * unitCost;
            _entryTotalCost.Text = totalCost.ToString("#.00");
        }
        private void EntryMarkupPercentage_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTotalPrice();
        }

        private void EntryUnitPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTotalPrice();
        }

        private void EntryUnitCost_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTotalCost();
        }

        private void EntryQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTotalCost();
            UpdateTotalPrice();
        }

        // dch rkl 11/22/2016 select all text on focus
        private void textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbx = (TextBox)sender;
            tbx.SelectAll();
        }

        private void _pickerChargeCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // dch rkl 10/14/2016 Show Code + Description in Charge Code Dropdown BEGIN
            string pickerChargeCode = "";
            try { pickerChargeCode = _pickerChargeCode.Items[_pickerChargeCode.SelectedIndex].ToString().Split(' ')[0].ToString().Trim(); }
            catch (Exception ex) { }
            //_vm.ExpenseChargeCode = (string)_pickerChargeCode.Items[_pickerChargeCode.SelectedIndex];
            _vm.ExpenseChargeCode = pickerChargeCode;

            // update other display items
            _entryUnitOfMeasure.Text = _vm.ExpenseUnitOfMeasure;
            if (_vm.ExpenseUnitPrice > 0)
            {
                _entryUnitCost.Text = _vm.ExpenseUnitPrice.ToString();
                _entryUnitPrice.Text = _vm.ExpenseUnitPrice.ToString();
            }
            else
            {
                _entryUnitCost.Text = string.Empty;
                _entryUnitPrice.Text = string.Empty;
            }
        }

        private void _pickerCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // dch rkl 10/14/2016 Show Code + Description in Expense Category Dropdown BEGIN
            string pickerCategory = "";
            try { pickerCategory = _pickerCategory.Items[_pickerCategory.SelectedIndex].ToString().Split(' ')[0].ToString().Trim(); }
            catch (Exception ex) { }
            //_vm.ExpenseCategory = (string)_pickerCategory.Items[_pickerCategory.SelectedIndex];
            _vm.ExpenseCategory = pickerCategory;
            // dch rkl 10/14/2016 Show Code + Description in Expense Category Dropdown END

            // scroll charge code
            for (int i = 0; i < _pickerChargeCode.Items.Count; i++)
            {
                // dch rkl 10/14/2016 Show Code + Description in Charge Code Dropdown BEGIN
                string pickerChargeCode = "";
                try { pickerChargeCode = _pickerChargeCode.Items[i].ToString().Split(' ')[0].ToString().Trim(); }
                catch (Exception ex) { }
                //if ((string)_pickerChargeCode.Items[i] == _vm.ExpenseChargeCode)
                if (pickerChargeCode == _vm.ExpenseChargeCode)
                // dch rkl 10/14/2016 Show Code + Description in Charge Code Dropdown END
                {
                    _pickerChargeCode.SelectedIndex = i;
                    break;
                }
            }
            _entryUnitOfMeasure.Text = _vm.ExpenseUnitOfMeasure;
            _entryUnitPrice.Text = _vm.ExpenseUnitPrice.ToString();
        }
    }
}
