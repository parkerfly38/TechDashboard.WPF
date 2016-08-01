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

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : UserControl
    {
        SolidColorBrush asbestos = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F8C8D"));
        SolidColorBrush emerald = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ECC71"));
        SolidColorBrush alizarin = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C"));
        SolidColorBrush peterriver = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB"));

        ComboBox _pickerWorkTicket;

        public HistoryPage()
        {
            InitializeComponent();
            StackPanel pageLayout = new StackPanel();

            _pickerWorkTicket = new ComboBox();
            foreach (App_ScheduledAppointment appt in App.Database.GetScheduledAppointments())
            {
                if (!_pickerWorkTicket.Items.Contains(appt.ServiceTicketNumber))
                {
                    _pickerWorkTicket.Items.Add(appt.ServiceTicketNumber);
                }
            }
            //_pickerWorkTicket.SelectedIndexChanged += _pickerWorkTicket_SelectedIndexChanged;
            Button buttonWorkTicket = new Button()
            {
                Background = emerald
            };
            TextBlock workTicketText = new TextBlock()
            {
                Text = "OPEN TICKET HISTORY",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White)
            };
            buttonWorkTicket.Content = workTicketText;
            buttonWorkTicket.Click += ButtonWorkTicket_Clicked;

            pageLayout = new StackPanel
            {
                Children = {
                        new StackPanel {
                            Background = peterriver,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Stretch,
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
                        new StackPanel {
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Top,
                            Children = {
                                _pickerWorkTicket,
                                new Label() {
                                    Content = "PLEASE SELECT A TICKET FIRST",
                                    FontWeight = FontWeights.Bold,
                                    Foreground = alizarin,
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center
                                },
                                buttonWorkTicket
                            }
                        }
                    }
            };

            if (App.Database.GetCurrentWorkTicket() != null)
            {
                _pickerWorkTicket.SelectedIndex = _pickerWorkTicket.Items.IndexOf(App.Database.GetCurrentWorkTicket().FormattedTicketNumber);
            }

            Content = pageLayout;

        }

        private void ButtonWorkTicket_Clicked(object sender, RoutedEventArgs e)
        {
            string selectedTicketNumber = (string)_pickerWorkTicket.Items[_pickerWorkTicket.SelectedIndex];
            //await Navigation.PushAsync(new HistoryPageDetail(selectedTicketNumber));
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new HistoryPageDetail(selectedTicketNumber);
        }
    }
}
