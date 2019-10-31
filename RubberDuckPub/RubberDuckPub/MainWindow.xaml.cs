using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace RubberDuckPub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bar bar;
        DispatcherTimer dispatcherTimer;
        TimeSpan timeSpan;

        public MainWindow()
        {
            InitializeComponent();

            List<string> testCases = new List<string>()
            {
                "Standard Settings",
                "20 glasses, 3 chairs",
                "20 chairs, 5 glasses",
                "Guests staying double time",
                "The waiter twice as fast",
                "Bar working hours: 5 minutes",
                "Couples Night",
                "Bus coming"
            };
            testComboBox.ItemsSource = testCases;

            openBarButton.Click += OnOpenBarButtonClicked;
            closeBarButton.Click += OnCloseBarButtonClicked;
            changeSpeedRadioButton.Checked += OnRadioButtonChecked;
        }
        //public ManualResetEvent PauseBartender = new ManualResetEvent(true); //is not really working 

        static List<double> speeds = new List<double>()
        {
            0.25, 0.5, 1, 2, 3, 4
        };
        private void OnRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            SpeedListBox.ItemsSource = speeds;
            SpeedListBox.IsEnabled = true;
            SpeedListBox.Visibility = Visibility.Visible;
        }

        private void OnOpenBarButtonClicked(object sender, RoutedEventArgs e)
        {
            openBarButton.IsEnabled = false;
            closeBarButton.IsEnabled = true;
            BartenderListBox.Items.Clear();
            WaiterListBox.Items.Clear();
            GuestsListBox.Items.Clear();
            barContentListBox.Items.Clear();

            double speed = 1;
            if (SpeedListBox.SelectedItem != null)
            {
                double.TryParse(SpeedListBox.SelectedItem.ToString(), out speed);
            }            

            switch (testComboBox.SelectedItem)
            {
                case "Standard Settings":
                    bar = new Bar(this, speed);
                    break;
                case "20 glasses, 3 chairs":
                    bar = new Bar(this, speed, numberOfGlasses: 20, numberOfChairs: 3);
                    break;
                case "20 chairs, 5 glasses":
                    bar = new Bar(this, speed, numberOfChairs: 20, numberOfGlasses: 5);
                    break;
                case "Guests staying double time":
                    bar = new Bar(this, speed, guestsStayingDouble: true);
                    break;
                case "The waiter twice as fast":
                    bar = new Bar(this, speed, waiterTwiceAsFast: true);
                    break;
                case "Bar working hours: 5 minutes":
                    bar = new Bar(this, speed, openingSeconds: 300);
                    break;
                case "Couples Night":
                    bar = new Bar(this, speed, couplesNight: true, numberOfGuestsAtATime: 2);
                    break;
                case "Bus coming":
                    bar = new Bar(this, speed, busIsComing: true);
                    break;
                default:
                    break;
            }
            UpdateBarStatus(bar);
        }

        private void OnCloseBarButtonClicked(object sender, RoutedEventArgs e)
        {
            closeBarButton.IsEnabled = false;
            openBarButton.IsEnabled = true;
            bar.IsOpen = false;
        }

        public void UpdateBarStatus(Bar bar)
        {
            timeSpan = TimeSpan.FromSeconds(bar.TimeOpenBar);
            dispatcherTimer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                string status = (bar.IsOpen) ? "open" : "closed";
                barStatusTextBox.Text = $"The bar is {status}!";
                if (bar.IsOpen)
                {
                    barStatusTextBox.Text += "\nThe bar is closing in: " + timeSpan.ToString("c");
                }
                else
                {
                    timeSpan = TimeSpan.Zero;
                }
                if (timeSpan == TimeSpan.Zero)
                {
                    dispatcherTimer.Stop();
                    bar.IsOpen = false;                    
                    barStatusTextBox.Text = "The bar is closed!";
                }
                timeSpan = timeSpan.Add(TimeSpan.FromSeconds(-1));

            }, Application.Current.Dispatcher);
            dispatcherTimer.Start();
        }
    }
}
