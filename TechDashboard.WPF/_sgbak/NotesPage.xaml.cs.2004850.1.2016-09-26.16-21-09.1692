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
    /// <summary>
    /// Interaction logic for NotesPage.xaml
    /// </summary>
    public partial class NotesPage : UserControl
    {
        NotesPageViewModel _vm;
        TextBox _editorNotes;
        Label _labelTitle;
        App_ScheduledAppointment appScheduledAppt;

        public NotesPage(App_WorkTicket workTicket, App_ScheduledAppointment scheduledAppointment)
        {
            _vm = new NotesPageViewModel(workTicket);
            appScheduledAppt = scheduledAppointment;
            InitializeComponent();
            Initialize();
        }

        public NotesPage()
        {
            InitializeComponent();
        }

        protected void Initialize()
        {
           //  Create a label for the notes page
            _labelTitle = new Label();
            _labelTitle.Content = "NOTES";
            _labelTitle.FontWeight = FontWeights.Bold;
            _labelTitle.FontSize = 22;
            _labelTitle.Foreground = new SolidColorBrush(Colors.White);
            _labelTitle.HorizontalAlignment = HorizontalAlignment.Center;
            _labelTitle.VerticalAlignment = VerticalAlignment.Center;

            Grid titleLayout = new Grid()
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB")),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 80
            };
            titleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            titleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            titleLayout.Children.Add(_labelTitle);
            Grid.SetColumn(_labelTitle, 0);
            Grid.SetRow(_labelTitle, 0);

            _editorNotes = new TextBox()
            {
                TextWrapping = TextWrapping.WrapWithOverflow,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                AcceptsReturn = true,
                AcceptsTab = true,
                Height = 300,
                Margin = new Thickness(30,10,30,10),
                Text = (_vm.WorkTicketText.Text == null ? string.Empty : _vm.WorkTicketText.Text)
            };

            Button buttonOK = new Button()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(30, 10, 30, 10),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ECC71"))
            };
            TextBlock buttonOkText = new TextBlock()
            {
                Text = "SAVE",
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = new SolidColorBrush(Colors.White)
            };
            buttonOK.Content = buttonOkText;
            buttonOK.Click += ButtonOK_Clicked;

            Button buttonCancel = new Button()
            {
                Margin = new Thickness(30,10,30,10),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C"))
            };
            TextBlock buttonCancelText = new TextBlock()
            {
                Text = "CANCEL",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White)
            };
            buttonCancel.Content = buttonCancelText;
            buttonCancel.Click += ButtonCancel_Clicked;

            Frame frame = new Frame();
            frame.Content = _editorNotes;

            gridMain.Children.Add(new StackPanel
            {
                Children = {
                    titleLayout,
     //               new Xamarin.Forms.Label { 
					//	Text = "TICKET NOTES",
					//	FontFamily = Device.OnPlatform("OpenSans-Bold", null, null), 
					//	TextColor = Color.FromHex("#7F8C8D")
					//},
					frame,
                    new StackPanel()
                    {
                        Orientation = Orientation.Vertical,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Children =
                        {
                            buttonOK,
                            buttonCancel
                        }
                    }
                }
            });
        }

        private void ButtonCancel_Clicked(object sender, RoutedEventArgs e)
        {
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new TicketDetailsPage(appScheduledAppt);
        }

        private void ButtonOK_Clicked(object sender, RoutedEventArgs e)
        {
            // Update the note text
            _vm.UpdateNotes(_editorNotes.Text);
            ContentControl contentArea = (ContentControl)this.Parent;
            contentArea.Content = new TicketDetailsPage(appScheduledAppt);
        }
    }
}
