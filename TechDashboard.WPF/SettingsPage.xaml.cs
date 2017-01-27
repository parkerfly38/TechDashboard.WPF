using Rkl.Erp.Sage.Sage100.TableObjects;
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
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net;

/**************************************************************************************************
 * Page Name    SettingsPage
 * Description: Application Settings
 *-------------------------------------------------------------------------------------------------
 *   Date       By      Description
 * ---------- --------- ---------------------------------------------------------------------------
 * 10/12/2016   DCH     Make sure the url is valid before allowing save;
 *                      If url is valid, enable the technician number and department;
 * 10/26/2016   DCH     Allow cancel/close of window without forcing save or having error occur.
 * 11/22/2016   DCH     On focus of text field, select all text.
 **************************************************************************************************/
 
namespace TechDashboard.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SettingsPage : Window
    {
        // dch rkl 10/26/2016 include parent
        private MainWindow _parent;
        private bool _saved;

        public event EventHandler SettingsSaved;
        public void OnSettingsSaved(object sender, EventArgs e)
        {
            if (SettingsSaved != null)
            {
                SettingsSaved(sender, e);
            }
        }

        AppSettingsPageViewModel _vm;

        // dch rkl 10/26/2016 include parent
        //public SettingsPage()
        public SettingsPage(MainWindow parent)
        {
            // dch rkl 10/26/2016 include parent
            _parent = parent;
            _saved = false;

            InitializeComponent();
            _vm = new AppSettingsPageViewModel();
            this.DataContext = _vm;
            if (_vm.RestServiceUrl == null || _vm.RestServiceUrl.Length <= 0)
            {
                labelDaysAfter_Copy.Visibility = Visibility.Hidden;
                labelDaysAfter_Copy1.Visibility = Visibility.Hidden;
                textUserID_Copy.Visibility = Visibility.Hidden;
                textUserID_Copy1.Visibility = Visibility.Hidden;

            }
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

            // dch rkl 10/26/2016 add cancel button
            btnCancelAppSettings.Click += btnCancelAppSettings_Click;

            //add a modicum of version checking
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            decimal versionNo = Convert.ToDecimal(version.Substring(0, 3));
            lblVersion.Content = "Version " + versionNo.ToString();

            // dch rkl 10/12/2016 Do not enable save button until a valid url is entered
            TestIfValidURL(_vm.RestServiceUrl, _vm.IsUsingHttps);

        }

        // dch rkl 10/28/2016 include empty constructor
        public SettingsPage()
        {
            _saved = false;    

            InitializeComponent();
            _vm = new AppSettingsPageViewModel();
            this.DataContext = _vm;
            if (_vm.RestServiceUrl == null || _vm.RestServiceUrl.Length <= 0)
            {
                labelDaysAfter_Copy.Visibility = Visibility.Hidden;
                labelDaysAfter_Copy1.Visibility = Visibility.Hidden;
                textUserID_Copy.Visibility = Visibility.Hidden;
                textUserID_Copy1.Visibility = Visibility.Hidden;

            }
            btnSaveAppSettings.Click += BtnSaveAppSettings_Click;
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            cbxIsUsingHttps.Checked += CbxIsUsingHttps_Checked;
            cbxIsUsingHttps.Unchecked += CbxIsUsingHttps_Unchecked;
            this.Closing += SettingsPage_Closed;   

            // dch rkl 10/26/2016 add cancel button
            btnCancelAppSettings.Click += btnCancelAppSettings_Click;

            //add a modicum of version checking
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            decimal versionNo = Convert.ToDecimal(version.Substring(0, 3));
            lblVersion.Content = "Version " + versionNo.ToString();

            // dch rkl 10/12/2016 Do not enable save button until a valid url is entered
            TestIfValidURL(_vm.RestServiceUrl, _vm.IsUsingHttps);

            textRESTURL.GotFocus += textbox_GotFocus;        // dch rkl 11/22/2016 select full text on focus
            textURL.GotFocus += textbox_GotFocus;        // dch rkl 11/22/2016 select full text on focus
            textDaysBefore.GotFocus += textbox_GotFocus;        // dch rkl 11/22/2016 select full text on focus
            textDaysAfter.GotFocus += textbox_GotFocus;        // dch rkl 11/22/2016 select full text on focus
            textUserID_Copy.GotFocus += textbox_GotFocus;        // dch rkl 11/22/2016 select full text on focus
            textUserID_Copy1.GotFocus += textbox_GotFocus;        // dch rkl 11/22/2016 select full text on focus
            textDeviceName.GotFocus += textbox_GotFocus;        // dch rkl 11/22/2016 select full text on focus
        }

        // dch rkl 11/22/2016 select all text on focus
        private void textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbx = (TextBox)sender;
            tbx.SelectAll();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SettingsPage_Closed(object sender, EventArgs e)
        {
            // dch rkl 10/26/2016
            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();
            // dch rkl 10/28/2016 if this is the first window displayed, there is no parent, so just close
            if (_parent != null) { _parent.Show(); }
            else { Application.Current.Shutdown(); }
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

        // dch rkl 10/26/2016 add cancel button
        private void btnCancelAppSettings_Click(object sender, RoutedEventArgs e)
        {           
            // dch rkl 10/28/2016 if this is the first window displayed, there is no parent, so just close
            if (_parent != null)
            {
                this.Close();
                _parent.Show();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private async void BtnSaveAppSettings_Click(object sender, RoutedEventArgs e)
        {
            _saved = true;     // dch rkl 10/26/2016

            OnSettingsSaved(sender, e);
            btnSaveAppSettings.IsEnabled = false;
            btnSaveAppSettings.Content = "SAVING...";
            _vm.SaveAppSettings();
            //check for valid technician
            string techno = (_vm.LoggedInTechnicianNo != null) ? _vm.LoggedInTechnicianNo : "";
            string techdeptno = _vm.LoggedInTechnicianDeptNo != null ? _vm.LoggedInTechnicianDeptNo : "";
            JT_Technician technician = App.Database.GetTechnician(_vm.LoggedInTechnicianDeptNo, _vm.LoggedInTechnicianNo);

            // dch rkl 10/12/2016 if the technician is null, try querying directly 
            // TODO pass technician filter in so loop is not necessary
            if (technician == null && techdeptno.Length > 0)
            {
                List<JT_Technician> technicians = App.Database.GetErpData<JT_Technician>(string.Empty, string.Empty);
                if (technicians != null && technicians.Count > 0)
                {
                    foreach (JT_Technician tech in technicians)
                    {
                        if (tech.TechnicianNo == techno && tech.TechnicianDeptNo == techdeptno)
                        {
                            technician = tech;
                            break;
                        }
                    }
                }
            }

            if ((techdeptno.Length > 0 && techno.Length > 0) && technician == null)
            {
                var result = MessageBox.Show("The technician you entered is not valid.  Please check your entries and try again.", "Invalid Technician", MessageBoxButton.OK);

                btnSaveAppSettings.IsEnabled = true;
                btnSaveAppSettings.Content = "SAVE SETTINGS";
                return;
            }

            //bool hasValidSetup = await App.Database.HasValidSetup();
            bool hasValidSetup = App.Database.HasValidSetup();
            if (!hasValidSetup)
            {
                var result = MessageBox.Show("These settings do not appear to work.  Please check your Internet connection or verify your settings.", "SETTINGS VERIFICATION FAILED", MessageBoxButton.OK);

                btnSaveAppSettings.IsEnabled = true;
                btnSaveAppSettings.Content = "SAVE SETTINGS";
                return;
            }
            else
            {
                App.Database.CreateGlobalTables();

                // dch rkl 10/26/2016
                _parent = new WPF.MainWindow();
                //MainWindow mainWindow = new MainWindow();
                //mainWindow.Show();

                this.Close();
            }
        }

        // dch rkl 10/12/2016 validate url
        private void TestIfValidURL(string sUrl, bool bIsHttps)
        {
            // If url is not blank, validate it
            // If valid, enable technician number and department

            bool bValid = false;

            if (sUrl == null) { sUrl = ""; }
            if (bIsHttps == null) { bIsHttps = false; }

            if (sUrl.Length > 0)
            {
                string sPrefix = @"http://";
                if (bIsHttps == true) { sPrefix = "https://"; }
                sUrl = String.Format("{0}{1}", sPrefix, sUrl);

                // set up the proper URL
                if (!sUrl.EndsWith(@"/"))
                {
                    sUrl += @"/";
                }
                sUrl += @"test";

                try
                {
                    HttpWebRequest request = WebRequest.Create(sUrl) as HttpWebRequest;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());
                    var sData = sr.ReadToEnd();
                    bValid = true;
                }
                catch (Exception ex)
                {
                    bValid = false;
                }
            }

            if (bValid)
            {
                labelDaysAfter_Copy.Visibility = Visibility.Visible;
                labelDaysAfter_Copy1.Visibility = Visibility.Visible;
                textUserID_Copy.Visibility = Visibility.Visible;
                textUserID_Copy1.Visibility = Visibility.Visible;
                btnSaveAppSettings.IsEnabled = true;
            }
            else
            {
                labelDaysAfter_Copy.Visibility = Visibility.Hidden;
                labelDaysAfter_Copy1.Visibility = Visibility.Hidden;
                textUserID_Copy.Visibility = Visibility.Hidden;
                textUserID_Copy1.Visibility = Visibility.Hidden;
                btnSaveAppSettings.IsEnabled = false;
            }
        }

        // dch rkl 10/12/2016 when tabbed out of url, validate the url
        private void textRESTURL_LostFocus(object sender, RoutedEventArgs e)
        {
            TestIfValidURL(textRESTURL.Text.Trim(), (bool)cbxIsUsingHttps.IsChecked);
        }
    }
}
