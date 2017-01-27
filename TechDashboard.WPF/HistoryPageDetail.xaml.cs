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

namespace TechDashboard.WPF
{
    /**************************************************************************************************
     * Page Name    HistoryPageDetail
     * Description: History Page Detail
     *-------------------------------------------------------------------------------------------------
     *   Date       By      Description
     * ---------- --------- ---------------------------------------------------------------------------
     * 11/22/2016   DCH     Add filter by parts/labor/all
     * 11/22/2016   DCH     Add ticket number to grid
     **************************************************************************************************/

    /// <summary>
    /// Interaction logic for HistoryPageDetail.xaml
    /// </summary>
    public partial class HistoryPageDetail : UserControl
    {

        SolidColorBrush asbestos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"));
        SolidColorBrush emerald = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ECC71"));
        SolidColorBrush alizarin = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C"));
        SolidColorBrush peterriver = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB"));

        HistoryPageViewModel _vm;

        // dch rkl 11/22/2016 add filter by parts/labor/all
        string _selectedTicketNumber;
        RadioButton _radParts;
        RadioButton _radLabor;
        RadioButton _radAll;
        ListView _historyList;

        public HistoryPageDetail(string selectedTicketNumber)
        {
            // dch rkl 11/22/2016 add filter by parts/labor/all
            _selectedTicketNumber = selectedTicketNumber;
            //_vm = new HistoryPageViewModel(selectedTicketNumber);
            _vm = new HistoryPageViewModel(selectedTicketNumber, "");

            InitializeComponent();
            Initialize();
        }

        class HistoryCell : ListViewItem
        {
            SolidColorBrush asbestos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"));
            SolidColorBrush emerald = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ECC71"));
            SolidColorBrush alizarin = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C"));
            SolidColorBrush peterriver = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB"));

            public HistoryCell()
            {
                Label labelDate = new Label();
                labelDate.Foreground = asbestos;
                labelDate.SetBinding(ContentProperty, "TransactionDate");
                labelDate.VerticalAlignment = VerticalAlignment.Center;

                Label labelTrx = new Label();
                labelTrx.Foreground = asbestos;
                labelTrx.SetBinding(ContentProperty, "Trx");
                labelTrx.VerticalAlignment = VerticalAlignment.Center;

                Label labelServiceTicketNo = new Label();
                labelServiceTicketNo.Foreground = asbestos;
                labelServiceTicketNo.SetBinding(ContentProperty, "ServiceTicketNo");
                labelServiceTicketNo.VerticalAlignment = VerticalAlignment.Center;

                Label labelItemEmployee = new Label();
                labelItemEmployee.Foreground = asbestos;
                labelItemEmployee.SetBinding(ContentProperty, "ItemEmployee");
                labelItemEmployee.VerticalAlignment = VerticalAlignment.Center;

                Label labelDescription = new Label();
                labelDescription.Foreground = asbestos;
                labelDescription.SetBinding(ContentProperty, "Description");
                labelDescription.VerticalAlignment = VerticalAlignment.Center;

                Label labelQuantity = new Label();
                labelQuantity.Foreground = asbestos;
                labelQuantity.SetBinding(ContentProperty, "Quantity");
                labelQuantity.VerticalAlignment = VerticalAlignment.Center;

                StackPanel rowLayout = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Children = {
                        labelDate,
                        labelTrx,
                        labelServiceTicketNo,
                        labelItemEmployee,
                        labelDescription,
                        labelQuantity
                    }
                };
                this.Content = rowLayout;
            }

        }

        protected void Initialize()
        {
            // dch rkl 11/22/2016 move ticket number to grid BEGIN
            Label labelServiceTicketTitle = new Label()
            {
                Content = "Service Ticket",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };
            Label labelServiceTicket = new Label()
            {
                Foreground = asbestos
            };
            // dch rkl 11/22/2016 move ticket number to grid END

            Label labelItemCodeTitle = new Label()
            {
                Content = "Item Code",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            Label labelItemCode = new Label()
            {
                Foreground = asbestos
            };
            
             

            Label labelItemCodeDesc = new Label()
            {
                Foreground = asbestos
            };


            Label mfgSerNoTitle = new Label()
            {
                Content = "Mfg Ser No",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            Label mfgSerNo = new Label()
            {
                //FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };
           
            Label mfgSerNoDesc = new Label()
            {
                Foreground = asbestos
            };

            Label intSerNoTitle = new Label()
            {
                Content = "Int Ser No",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };
            Label intSerNo = new Label()
            {
                Foreground = asbestos
            };

           
            Label modelNoTitle = new Label()
            {
                Content = "Model No",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };
            Label modelNo = new Label()
            {
                Foreground = asbestos
            };
            //if (_vm.History.Count > 0)
            //{
            labelItemCode.Content = _vm.WorkTicket.DtlRepairItemCode; //_vm.History[0].ItemCode;
            labelItemCodeDesc.Content = _vm.Item.ItemCodeDesc; //_vm.History[0].ItemDesc;

            // dch rkl 11/22/2016 move ticket number to grid 
            labelServiceTicket.Content = _vm.TicketNumber;

            // dch rkl 10/18/2016 Make sure there is at least 1 history item before trying to assign the value.
            // this was throwing a null exception
            if (_vm.History.Count > 0)
            {
                mfgSerNo.Content = _vm.History[0].MfgSerialNo;
                mfgSerNoDesc.Content = _vm.History[0].EADesc;
                intSerNo.Content = _vm.History[0].IntSerialNo;
                modelNo.Content = _vm.History[0].ModelNo;
            }

            //}
            //history 
            // dch rkl 11/22/2016 add filter by parts/labor/all
            //ListView historyList = new ListView()
            _historyList = new ListView()
            {
                ItemsSource = _vm.History,
                ItemTemplate = (DataTemplate)this.Resources["HistoryDataTemplate"]
                , Height = 475      // dch rkl 11/22/2016 this forces scrollbars   
            };

            Grid topGrid = new Grid();
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });     // dch rkl 11/22/2016 Add Row for Ticket
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Pixel)});     // dch rkl 11/22/2016 Add filtering
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });     // dch rkl 11/22/2016 Add filtering
            topGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Pixel) });     // dch rkl 11/22/2016 Add filtering
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });     // dch rkl 11/22/2016 Add cancel and filter buttons
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200, GridUnitType.Pixel) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200, GridUnitType.Pixel) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200, GridUnitType.Pixel) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200, GridUnitType.Pixel) });
            
            // dch rkl 11/22/2016 Add Row for Ticket
            topGrid.Children.Add(labelServiceTicketTitle);
            Grid.SetColumn(labelServiceTicketTitle, 0);
            Grid.SetRow(labelServiceTicketTitle, 0);
            topGrid.Children.Add(labelServiceTicket);
            Grid.SetColumn(labelServiceTicket, 1);
            Grid.SetRow(labelServiceTicket, 0);
            Grid.SetColumnSpan(labelServiceTicket, 3);

            topGrid.Children.Add(labelItemCodeTitle);
            Grid.SetColumn(labelItemCodeTitle, 0);
            Grid.SetRow(labelItemCodeTitle, 1);
            topGrid.Children.Add(labelItemCode);
            Grid.SetColumn(labelItemCode, 1);
            Grid.SetRow(labelItemCode, 1);
            topGrid.Children.Add(labelItemCodeDesc);
            Grid.SetColumn(labelItemCodeDesc, 2);
            Grid.SetRow(labelItemCodeDesc, 1);
            Grid.SetColumnSpan(labelItemCodeDesc, 2);

            topGrid.Children.Add(mfgSerNoTitle);
            Grid.SetColumn(mfgSerNoTitle, 0);
            Grid.SetRow(mfgSerNoTitle, 2);
            topGrid.Children.Add(mfgSerNo);
            Grid.SetColumn(mfgSerNo, 1);
            Grid.SetRow(mfgSerNo, 2);
            topGrid.Children.Add(mfgSerNoDesc);
            Grid.SetColumn(mfgSerNoDesc, 2);
            Grid.SetRow(mfgSerNoDesc, 2);
            Grid.SetColumnSpan(mfgSerNoDesc, 2);

            topGrid.Children.Add(intSerNoTitle);
            Grid.SetColumn(intSerNoTitle, 0);
            Grid.SetRow(intSerNoTitle, 3);
            topGrid.Children.Add(intSerNo);
            Grid.SetColumn(intSerNo, 1);
            Grid.SetRow(intSerNo, 3);
            topGrid.Children.Add(modelNoTitle);
            Grid.SetColumn(modelNoTitle, 2);
            Grid.SetRow(modelNoTitle, 3);
            topGrid.Children.Add(modelNo);
            Grid.SetColumn(modelNo, 3);
            Grid.SetRow(modelNo, 3);

            // dch rkl 11/22/2016 add filtering radio buttons BEGIN
            Label lblSpace = new Label();

            _radParts = new RadioButton();
            _radParts.Content = "Parts Only";
            _radParts.FontWeight = FontWeights.Bold;
            _radParts.Checked += new RoutedEventHandler(radPLB_Checked);

            _radLabor = new RadioButton();
            _radLabor.Content = "Labor Only";
            _radLabor.FontWeight = FontWeights.Bold;
            _radLabor.Checked += new RoutedEventHandler(radPLB_Checked);

            _radAll = new RadioButton();
            _radAll.Content = "Both";
            _radAll.FontWeight = FontWeights.Bold;
            _radAll.IsChecked = true;
            _radAll.Checked += new RoutedEventHandler(radPLB_Checked);

            topGrid.Children.Add(lblSpace);
            Grid.SetColumn(lblSpace, 0);
            Grid.SetRow(lblSpace, 5);
            topGrid.Children.Add(_radParts);
            Grid.SetColumn(_radParts, 1);
            Grid.SetRow(_radParts, 5);
            topGrid.Children.Add(_radLabor);
            Grid.SetColumn(_radLabor, 2);
            Grid.SetRow(_radLabor, 5);
            topGrid.Children.Add(_radAll);
            Grid.SetColumn(_radAll, 3);
            Grid.SetRow(_radAll, 5);
            // dch rkl 11/22/2016 add filtering radio buttons END

            // dch rkl 11/22/2016 add cancel button BEGIN
            Button buttonCancel = new Button()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C")),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(30, 5, 30, 5),
                Height = 40
            };
            TextBlock cancelText = new TextBlock()
            {
                Text = "CANCEL",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White)
            };
            buttonCancel.Content = cancelText;
            buttonCancel.Click += buttonCancel_Clicked;

            topGrid.Children.Add(buttonCancel);
            Grid.SetColumn(buttonCancel, 0);
            Grid.SetRow(buttonCancel, 7);
            Grid.SetColumnSpan(buttonCancel, 4);
            // dch rkl 11/22/2016 add filter and cancel buttons END

            gridMain.Children.Add(new StackPanel
            {
                Children = {
                    topGrid,
                    _historyList
                },
            });

        }

        // dch rkl 11/22/2016 add cancel button
        private void buttonCancel_Clicked(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new HistoryPage();
        }
        
        // dch rkl 11/22/2016 filter the list for parts/labor/both BEGIN
        private void FilterList()
        {
            string sPLA = "";
            if ((bool)_radParts.IsChecked) { sPLA = "P"; }
            else if ((bool)_radLabor.IsChecked) { sPLA = "L"; }

            _vm = new HistoryPageViewModel(_selectedTicketNumber, sPLA);
            _historyList.ItemsSource = _vm.History;
        }

        private void radPLB_Checked(object sender, RoutedEventArgs e)
        {
            // Filter List
            FilterList();
        }
        // dch rkl 11/22/2016 filter the list for parts/labor/both END
    }
}

