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

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for SmsPage.xaml
    /// </summary>
    public partial class SmsPage : UserControl
    {
        public SmsPage()
        {
            InitializeComponent();
            Button buttonSendSms = new Button() { Content = "Send SMS" };
            buttonSendSms.Click += ButtonSendSms_Click;

            gridMain.Children.Add(new StackPanel
            {
                Children = {
                    new Label { Content = "SMS PAGE COMING SOON" },
                    buttonSendSms
                }
            });
        }

        protected void ButtonSendSms_Click(object sender, EventArgs e)
        {
            //var result =  MessageBox.Show("Not yet implemented.");
        }
    }
}
