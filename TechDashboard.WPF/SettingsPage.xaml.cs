using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using TechDashboard.Data;
using TechDashboard.ViewModels;

namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SettingsPage : Window
    {
        public event EventHandler SettingsSaved;
        public void OnSettingsSaved(object sender, EventArgs e)
        {
            if (SettingsSaved != null)
            {
                SettingsSaved(sender, e);
            }
        }

        AppSettingsPageViewModel _vm;

        public SettingsPage()
        {
            InitializeComponent();
            _vm = new AppSettingsPageViewModel();
            this.DataContext = _vm;
            //textURL.SetBinding(TextBox.TextProperty, "SDataUrl");
            //textUserID.SetBinding(TextBox.TextProperty, "SDataUserId");
            //textDaysAfter.SetBinding(TextBox.TextProperty, "ScheduleDaysAfter");
            //textDaysBefore.SetBinding(TextBox.TextProperty, "ScheduleDaysBefore");
            btnSaveAppSettings.Click += BtnSaveAppSettings_Click;
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            cbxIsUsingHttps.Checked += CbxIsUsingHttps_Checked;
            cbxIsUsingHttps.Unchecked += CbxIsUsingHttps_Unchecked;
            //cbxIsUsingHttps.SetBinding(CheckBox.IsCheckedProperty, "IsUsingHttps");
            //labelHttpText.Content = (_vm.IsUsingHttps ? @"https://" : @"http://");
            this.Closing += SettingsPage_Closed;

            //add a modicum of version checking
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            decimal versionNo = Convert.ToDecimal(version.Substring(0, 3));
            lblVersion.Content = "Version " + versionNo.ToString();
        }

        private void SettingsPage_Closed(object sender, EventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void CbxIsUsingHttps_Unchecked(object sender, RoutedEventArgs e)
        {
            labelHttpText.Content = @"http://";
        }

        private void CbxIsUsingHttps_Checked(object sender, RoutedEventArgs e)
        {
            labelHttpText.Content = @"https://";
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _vm.SDataPassword = passwordBox.Password;
        }

        private async void BtnSaveAppSettings_Click(object sender, RoutedEventArgs e)
        {
            OnSettingsSaved(sender, e);
            _vm.SaveAppSettings();
            bool hasValidSetup = await App.Database.HasValidSetup();
            if (!hasValidSetup)
            {
                var result = MessageBox.Show("These settings do not appear to work.  Please check your Internet connection or verify your settings.", "SETTINGS VERIFICATION FAILED", MessageBoxButton.OK);
            }
            else
            {
                App.Database.CreateGlobalTables();
                this.Close();
            }
        }
    }
}
