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

/**************************************************************************************************
 * Page Name    HistoryPage
 * Description: History Page
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/26/2016   DCH     Standardize page font sizes, colors and buttons and alignment of data, labels
 * 11/22/2016   DCH     Move page label to code
 **************************************************************************************************/

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
                App_WorkTicket _workticket = App.Database.GetWorkTicket2(appt.ServiceTicketNumber);
                if (_workticket != null)
                {
                    if ((_workticket.DtlRepairItemCode != null && _workticket.DtlMfgSerialNo != null) )//|| (_workticket.DtlRepairItemCode.Length > 0 && _workticket.DtlMfgSerialNo.Length > 0))
                    {
                        List<JT_WorkTicket> workTickets = App.Database.GetWorkTickets(_workticket.DtlRepairItemCode, _workticket.DtlMfgSerialNo);
                        if (!_pickerWorkTicket.Items.Contains(appt.ServiceTicketNumber) && workTickets.Count > 0)
                        {
                            _pickerWorkTicket.Items.Add(appt.ServiceTicketNumber);
                        }
                    }
                }
            }
                

            //get workticket and remove if it's got no worktickets


            //_pickerWorkTicket.SelectedIndexChanged += _pickerWorkTicket_SelectedIndexChanged;
            Button buttonWorkTicket = new Button()
            {
                Background = emerald
            };
            TextBlock workTicketText = new TextBlock()
            {
                Text = "OPEN SERVICE TICKET HISTORY",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White)
            };
            buttonWorkTicket.Content = workTicketText;
            buttonWorkTicket.Click += ButtonWorkTicket_Clicked;

            // dch rkl 10/26/2016
            buttonWorkTicket.Margin = new Thickness(30, 5, 30, 0);
            buttonWorkTicket.Height = 40;
            
            // put it all together on the page
            gridMain.Children.Add(new StackPanel()
            {
                Children = {
                        new StackPanel {
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Top,
                            Children = {
                                _pickerWorkTicket,
                                new Label() {
                                    Content = "SELECT A SERVICE TICKET",
                                    FontWeight = FontWeights.Bold,
                                    Foreground = alizarin,
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center
                                },
                                buttonWorkTicket
                            }
                        }
                    }
            });

            if (App.Database.GetCurrentWorkTicket() != null)
            {
                _pickerWorkTicket.SelectedIndex = _pickerWorkTicket.Items.IndexOf(App.Database.GetCurrentWorkTicket().FormattedTicketNumber);
            }

        }

        private void ButtonWorkTicket_Clicked(object sender, RoutedEventArgs e)
        {
            // dch rkl 10/05/2016 make sure a ticket was selected
            if (_pickerWorkTicket.SelectedIndex > -1)
            {
                string selectedTicketNumber = (string)_pickerWorkTicket.Items[_pickerWorkTicket.SelectedIndex];
                //await Navigation.PushAsync(new HistoryPageDetail(selectedTicketNumber));
                ContentControl contentArea = (ContentControl)this.Parent;
                contentArea.Content = new HistoryPageDetail(selectedTicketNumber);
            }
        }
    }
}
