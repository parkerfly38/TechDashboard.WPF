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
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is decimal)
                return value.ToString();
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal dec;
            if (decimal.TryParse(value as string, out dec))
                return dec;
            return value;
        }
    }
    /// <summary>
    /// Interaction logic for PartsEditPage.xaml
    /// </summary>
    public partial class PartsEditPage : UserControl
    {
        public enum PageMode
        {
            Add,
            Edit
        };

        PartsEditPageViewModel _vm;
        App_ScheduledAppointment _scheduledAppointment;

        PageMode _pageMode;
        Label _labelTitle;

        Label _labelPartNumber;
        Label _labelPartDescription;
        ComboBox _pickerSerialNumber;
        ComboBox _pickerWarehouse;
        TextBox _entryQuantity;
        TextBox _entryUnitOfMeasure;
        TextBox _entryUnitCost;
        TextBox _entryUnitPrice;
        Label _labelExtensionPrice;
        Button _buttonAddPart;
        Button _buttonCancel;
        TextBox _entryComments;
        CheckBox _switchIsChargeable;
        CheckBox _switchIsPrintable;
        CheckBox _switchIsPurchased;
        CheckBox _switchIsOverhead;

        public PartsEditPage(App_WorkTicket workTicket, CI_Item part, PageMode pageMode, App_ScheduledAppointment scheduledAppointment)
        {
            _pageMode = pageMode;
            _vm = new PartsEditPageViewModel(workTicket, part);
            _scheduledAppointment = scheduledAppointment;
            InitializeComponent();

            SetPageLayout();
        }

        public PartsEditPage(App_WorkTicket workTicket, App_RepairPart part, PageMode pageMode, App_ScheduledAppointment scheduledAppointment)
        {
            _pageMode = pageMode;
            _vm = new PartsEditPageViewModel(workTicket, part);
            _scheduledAppointment = scheduledAppointment;
            InitializeComponent();

            SetPageLayout();
        }
        protected void SetPageLayout()
        {
           //  Create a label for the technician list
            _labelTitle = new Label();

            // Set the page title.
            switch (_pageMode)
            {
                case PageMode.Add:
                    _labelTitle.Content = "ADD PART";
                    break;
                case PageMode.Edit:
                    _labelTitle.Content = "EDIT PART";
                    break;
                default:
                    _labelTitle.Content = "ADD/EDIT PART";
                    break;
            }

            gridMain.DataContext = _vm.PartToEdit;

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

            SolidColorBrush asbestos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7f8C8d"));

            _labelPartNumber = new Label();
            _labelPartNumber.SetBinding(ContentProperty, "PartItemCode");
            _labelPartNumber.FontWeight = FontWeights.Bold;
            _labelPartNumber.Foreground = asbestos;
            _labelPartNumber.HorizontalAlignment = HorizontalAlignment.Left;

            _labelPartDescription = new Label();
            _labelPartDescription.SetBinding(ContentProperty, "PartItemCodeDescription");
            _labelPartDescription.FontWeight = FontWeights.Bold;
            _labelPartDescription.Foreground = asbestos;
            _labelPartDescription.HorizontalAlignment = HorizontalAlignment.Left;

            Label labelserial = new Label()
            {
                Content = "MFG SERIAL:",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };
            _pickerSerialNumber = new ComboBox();

            foreach (string serialNumber in _vm.GetMfgSerialNumbersForPart())
            {
                _pickerSerialNumber.Items.Add(serialNumber);
            }
            if ((_pickerSerialNumber.Items == null) || (_pickerSerialNumber.Items.Count == 0))
            {
                _pickerSerialNumber.Visibility = Visibility.Hidden;
            }
            else
            {
                _pickerSerialNumber.Visibility = Visibility.Visible;
                _pickerSerialNumber.SelectedIndex = 0;
            }
            _pickerSerialNumber.HorizontalAlignment = HorizontalAlignment.Left;

            Label labelWarehouse = new Label()
            {
                Content = "WAREHOUSE:",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };
            _pickerWarehouse = new ComboBox();
            _pickerWarehouse.Width = 100;
            _pickerWarehouse.HorizontalAlignment = HorizontalAlignment.Left;
            foreach (string warehouse in _vm.WarehouseList)
            {
                _pickerWarehouse.Items.Add(warehouse);
            }
            try { _pickerWarehouse.SelectedIndex = _pickerWarehouse.Items.IndexOf(_vm.PartToEdit.Warehouse); } catch { }
            //_pickerWarehouse.SelectedIndex = 0;

            Label labelQuantity = new Label()
            {
                Content = "QTY:",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            _entryQuantity = new TextBox();


            _entryQuantity.SetBinding(ContentProperty, "Quantity");
            _entryQuantity.TextChanged += EntryQuantity_TextChanged;

            _entryQuantity.Foreground = asbestos;
            _entryQuantity.Text = "1";

            Label labelUOM = new Label()
            {
                Content = "U/M",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            _entryUnitOfMeasure = new TextBox();
            _entryUnitOfMeasure.SetBinding(ContentProperty, "UnitOfMeasure");
            _entryUnitOfMeasure.Text = _vm.PartToEdit.UnitOfMeasure;
            _entryUnitOfMeasure.Foreground = asbestos;
            _entryUnitOfMeasure.Style = (Style)this.Resources["styleUOM"];

            Label labelUnitCost = new Label()
            {
                Content = "COST",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            _entryUnitCost = new TextBox();
            _entryUnitCost.Text = _vm.PartToEdit.UnitCost.ToString();
            _entryUnitCost.IsEnabled = false;
            _entryUnitCost.Foreground = asbestos;

            Label labelPrice = new Label()
            {
                Content = "PRICE",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            _entryUnitPrice = new TextBox();
            //_entryUnitPrice.SetBinding(ContentProperty,"UnitPrice");
            _entryUnitPrice.Text = _vm.PartToEdit.UnitPrice.ToString();
            _entryUnitPrice.Foreground = asbestos;

            _labelExtensionPrice = new Label();
            _labelExtensionPrice.Content = (_vm.PartToEdit.UnitPrice * _vm.PartToEdit.Quantity).ToString();
            _labelExtensionPrice.Foreground = asbestos;

            Label labelComments = new Label()
            {
                Content = "COMMENTS",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            _entryComments = new TextBox();
            _entryComments.TextWrapping = TextWrapping.WrapWithOverflow;
            _entryComments.SetBinding(ContentProperty, "Comment");
            _entryComments.Foreground = asbestos;
            _entryComments.Style = (Style)this.Resources["styleComments"];

            _switchIsChargeable = new CheckBox();
            
            
            _switchIsPrintable = new CheckBox();

            _switchIsPurchased = new CheckBox();

            _switchIsOverhead = new CheckBox();

            if (_pageMode == PageMode.Edit)
            {
                _switchIsChargeable.SetBinding(CheckBox.IsCheckedProperty, "IsChargeable");
                _switchIsPrintable.SetBinding(CheckBox.IsCheckedProperty, "IsPrintable");
                _switchIsPurchased.SetBinding(CheckBox.IsCheckedProperty, "IsPurchased");
                _switchIsOverhead.SetBinding(CheckBox.IsCheckedProperty, "IsOverhead");
            }

            Grid topGrid = new Grid();
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150, GridUnitType.Pixel) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            topGrid.Children.Add(_labelPartNumber);
            Grid.SetColumn(_labelPartNumber, 0);
            Grid.SetRow(_labelPartNumber, 0);
            topGrid.Children.Add(_labelPartDescription);
            Grid.SetColumn(_labelPartDescription, 1);
            Grid.SetRow(_labelPartDescription, 0);

            topGrid.Children.Add(labelserial);
            Grid.SetColumn(labelserial, 0);
            Grid.SetRow(labelserial, 1);
            topGrid.Children.Add(_pickerSerialNumber);
            Grid.SetColumn(_pickerSerialNumber, 1);
            Grid.SetRow(_pickerSerialNumber, 1);

            topGrid.Children.Add(labelWarehouse);
            Grid.SetColumn(labelWarehouse, 0);
            Grid.SetRow(labelWarehouse, 2);
            topGrid.Children.Add(_pickerWarehouse);
            Grid.SetColumn(_pickerWarehouse, 1);
            Grid.SetRow(_pickerWarehouse, 2);

            Grid amtsGrid = new Grid();
            amtsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            amtsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            amtsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            amtsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            amtsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150, GridUnitType.Pixel) });
            amtsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Pixel) });
            amtsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Pixel) });

            amtsGrid.Children.Add(labelQuantity);
            Grid.SetColumn(labelQuantity, 0);
            Grid.SetRow(labelQuantity, 0);
            amtsGrid.Children.Add(_entryQuantity);
            Grid.SetColumn(_entryQuantity, 1);
            Grid.SetRow(_entryQuantity, 0);
            amtsGrid.Children.Add(_entryUnitOfMeasure);
            Grid.SetColumn(_entryUnitOfMeasure, 2);
            Grid.SetRow(_entryUnitOfMeasure, 0);

            amtsGrid.Children.Add(labelUnitCost);
            Grid.SetColumn(labelUnitCost, 0);
            Grid.SetRow(labelUnitCost, 1);
            amtsGrid.Children.Add(_entryUnitCost);
            Grid.SetColumn(_entryUnitCost, 1);
            Grid.SetRow(_entryUnitCost, 1);

            amtsGrid.Children.Add(labelPrice);
            Grid.SetColumn(labelPrice, 0);
            Grid.SetRow(labelPrice, 2);
            amtsGrid.Children.Add(_entryUnitPrice);
            Grid.SetColumn(_entryUnitPrice, 1);
            Grid.SetRow(_entryUnitPrice, 2);

            Label labelExtPrice = new Label()
            {
                Content = "EXT PRICE:",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            amtsGrid.Children.Add(labelExtPrice);
            Grid.SetColumn(labelExtPrice, 0);
            Grid.SetRow(labelExtPrice, 3);
            amtsGrid.Children.Add(_labelExtensionPrice);
            Grid.SetColumn(_labelExtensionPrice, 1);
            Grid.SetRow(_labelExtensionPrice, 3);

            _buttonAddPart = new Button();
            TextBlock buttonAddPartText = new TextBlock();
            switch (_pageMode)
            {
                case PageMode.Add:
                    buttonAddPartText.Text = "ADD";
                    break;
                case PageMode.Edit:
                    buttonAddPartText.Text = "UPDATE";
                    break;
            }
            _buttonAddPart.Click += ButtonAddPart_Click;

            buttonAddPartText.FontWeight = FontWeights.Bold;
            _buttonAddPart.Foreground = new SolidColorBrush(Colors.White);
            _buttonAddPart.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ECC71"));
            _buttonAddPart.HorizontalAlignment = HorizontalAlignment.Stretch;
            _buttonAddPart.Margin = new Thickness(30, 10, 30, 10);
            _buttonAddPart.Content = buttonAddPartText;

            _buttonCancel = new Button();
            TextBlock buttonCancelText = new TextBlock();
            buttonCancelText.Text = "CANCEL";
            buttonCancelText.Foreground = new SolidColorBrush(Colors.White);
            buttonCancelText.FontWeight = FontWeights.Bold;
            _buttonCancel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C"));
            _buttonCancel.Click += ButtonCancel_Click;
            _buttonCancel.HorizontalAlignment = HorizontalAlignment.Stretch;
            _buttonCancel.Content = buttonCancelText;
            _buttonCancel.Margin = new Thickness(30, 10, 30, 10);

            gridMain.Children.Add(new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Children =
                {
                    titleLayout,
                    topGrid,
                    amtsGrid,
                    _entryComments,
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Children =
                        {
                            new Label { Content = "CHARGE:", FontWeight = FontWeights.Bold, Foreground = asbestos },
                            _switchIsChargeable
                        }
                    },
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Children =
                        {
                            new Label { Content = "PRINT:", FontWeight = FontWeights.Bold, Foreground = asbestos },
                            _switchIsPrintable
                        }
                    },
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Children =
                        {
                            new Label { Content = "PURCHASE:", FontWeight = FontWeights.Bold, Foreground = asbestos },
                            _switchIsPurchased
                        }
                    },
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Children =
                        {
                            new Label { Content = "OVERHEAD:", FontWeight = FontWeights.Bold, Foreground = asbestos },
                            _switchIsOverhead
                        }
                    },
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Children =
                        {
                            _buttonAddPart,
                            _buttonCancel
                        }
                    },
                }
            });
        }
        
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            if (_pageMode == PageMode.Add)
            {
                contentArea.Content = new PartsListPage(_vm.WorkTicket, _scheduledAppointment);
            }
           
        }

        private void ButtonAddPart_Click(object sender, RoutedEventArgs e)
        {
            if (_entryUnitOfMeasure.Text.Length <= 0)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("You must select a unit of measure.", "Unit of Measure", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                    return;
            }

            _vm.PartToEdit.Warehouse = (string)_pickerWarehouse.Items[_pickerWarehouse.SelectedIndex];
            _vm.PartToEdit.Quantity = Convert.ToDouble(_entryQuantity.Text);
            _vm.PartToEdit.UnitPrice = Convert.ToDouble(_entryUnitPrice.Text);
            _vm.PartToEdit.UnitOfMeasure = _entryUnitOfMeasure.Text;
            _vm.PartToEdit.IsChargeable = (bool)_switchIsChargeable.IsChecked;
            _vm.PartToEdit.IsPrintable = (bool)_switchIsPrintable.IsChecked;
            _vm.PartToEdit.IsPurchased = (bool)_switchIsPurchased.IsChecked;
            _vm.PartToEdit.IsOverhead = (bool)_switchIsOverhead.IsChecked;
            _vm.PartToEdit.Comment = _entryComments.Text;

            // puke
            switch (_pageMode)
            {
                case PageMode.Add:
                    _vm.AddPartToPartsList();
                    //await Navigation.PopAsync();
                    break;
                case PageMode.Edit:
                    _vm.UpdatePartOnPartsList();
                    break;
            }
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new PartsListPage(_vm.WorkTicket, _scheduledAppointment);
        }

        private void EntryQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_labelExtensionPrice == null)
                return;
            double n;
            bool isNumeric = double.TryParse(_entryQuantity.Text, out n);
            if (!isNumeric)
            {
                n = 1;
                _entryQuantity.Text = "1";
            }
            _labelExtensionPrice.Content = "$" + (n * _vm.PartToEdit.UnitPrice).ToString();
        }
    }
}
