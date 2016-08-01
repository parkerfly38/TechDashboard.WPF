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
        
        public HistoryPageDetail(string selectedTicketNumber)
        {
            _vm = new HistoryPageViewModel(selectedTicketNumber);
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

            Label labelItemCodeTitle = new Label()
            {
                Content = "ITEM CODE",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            Label labelItemCode = new Label()
            {
                Content = _vm.History[0].ItemCode,
                Foreground = asbestos
            };

            Label labelItemCodeDesc = new Label()
            {
                Content = _vm.History[0].ItemDesc,
                Foreground = asbestos
            };

            Label mfgSerNoTitle = new Label()
            {
                Content = "MFG SER NO",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            Label mfgSerNo = new Label()
            {
                Content = _vm.History[0].MfgSerialNo,
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };

            Label mfgSerNoDesc = new Label()
            {
                Content = _vm.History[0].EADesc,
                Foreground = asbestos
            };

            Label intSerNoTitle = new Label()
            {
                Content = "INT SER NO",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };
            Label intSerNo = new Label()
            {
                Content = _vm.History[0].IntSerialNo,
                Foreground = asbestos
            };
            Label modelNoTitle = new Label()
            {
                Content = "MODEL NO",
                FontWeight = FontWeights.Bold,
                Foreground = asbestos
            };
            Label modelNo = new Label()
            {
                Content = _vm.History[0].ModelNo,
                Foreground = asbestos
            };

            //history 
            ListView historyList = new ListView()
            {
                ItemsSource = _vm.History,
                ItemTemplate = new DataTemplate(typeof(HistoryCell)) //,
                /*Header = new Xamarin.Forms.Label
                {
                    Text = "Details",
                    FontFamily = Device.OnPlatform("OpenSans-Bold", null, null),
                    TextColor = Color.FromHex("#FFFFFF"),
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    BackgroundColor = peterriver
                }*/
            };

            Grid topGrid = new Grid();
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200, GridUnitType.Pixel) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200, GridUnitType.Pixel) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200, GridUnitType.Pixel) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200, GridUnitType.Pixel) });

            topGrid.Children.Add(labelItemCodeTitle);
            Grid.SetColumn(labelItemCodeTitle, 0);
            Grid.SetRow(labelItemCodeTitle, 0);
            topGrid.Children.Add(labelItemCode);
            Grid.SetColumn(labelItemCode, 1);
            Grid.SetRow(labelItemCode, 0);
            topGrid.Children.Add(labelItemCodeDesc);
            Grid.SetColumn(labelItemCodeDesc, 2);
            Grid.SetRow(labelItemCodeDesc, 0);
            Grid.SetColumnSpan(labelItemCodeDesc, 2);

            topGrid.Children.Add(mfgSerNoTitle);
            Grid.SetColumn(mfgSerNoTitle, 0);
            Grid.SetRow(mfgSerNoTitle, 1);
            topGrid.Children.Add(mfgSerNo);
            Grid.SetColumn(mfgSerNo, 1);
            Grid.SetRow(mfgSerNo, 1);
            topGrid.Children.Add(mfgSerNoDesc);
            Grid.SetColumn(mfgSerNoDesc, 2);
            Grid.SetRow(mfgSerNoDesc, 1);
            Grid.SetColumnSpan(mfgSerNoDesc, 2);

            topGrid.Children.Add(intSerNoTitle);
            Grid.SetColumn(intSerNoTitle, 0);
            Grid.SetRow(intSerNoTitle, 2);
            topGrid.Children.Add(intSerNo);
            Grid.SetColumn(intSerNo, 1);
            Grid.SetRow(intSerNo, 2);
            topGrid.Children.Add(modelNoTitle);
            Grid.SetColumn(modelNoTitle, 2);
            Grid.SetRow(modelNoTitle, 2);
            topGrid.Children.Add(modelNo);
            Grid.SetColumn(modelNo, 3);
            Grid.SetRow(modelNo, 2);

            gridMain.Children.Add(new StackPanel
            {
                Children = {
                    new StackPanel {
                        Background = peterriver,
                        Height = 80,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Top,
                        Children = {
                            new Label {
                                Content = "HISTORY",
                                FontWeight = FontWeights.Bold,
                                Foreground = new SolidColorBrush(Colors.White),
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center
                            }
                        }
                    },
                    topGrid,
                    historyList
                }
            });

        }
    }
}
