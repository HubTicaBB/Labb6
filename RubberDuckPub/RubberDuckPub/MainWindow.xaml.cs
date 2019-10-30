using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
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
        }
        //public ManualResetEvent PauseBartender = new ManualResetEvent(true); //is not really working 

        private void OnOpenBarButtonClicked(object sender, RoutedEventArgs e)
        {
            openBarButton.IsEnabled = false;
            closeBarButton.IsEnabled = true;
            switch (testComboBox.SelectedItem)
            {
                case "Standard Settings":
                    bar = new Bar(this);
                    UpdateBarStatus(bar);
                    break;
                case "20 glasses, 3 chairs":
                    bar = new Bar(this, numberOfGlasses: 20, numberOfChairs: 3);
                    UpdateBarStatus(bar);
                    break;
                case "20 chairs, 5 glasses":
                    bar = new Bar(this, numberOfChairs: 20, numberOfGlasses: 5);
                    UpdateBarStatus(bar);
                    break;
                case "Guests staying double time":
                    bar = new Bar(this, guestsStayingDouble: true);
                    UpdateBarStatus(bar);
                    break;
                case "The waiter twice as fast":
                    bar = new Bar(this, waiterTwiceAsFast: true);
                    UpdateBarStatus(bar);
                    break;
                case "Bar working hours: 5 minutes":
                    bar = new Bar(this, openingSeconds: 300);
                    UpdateBarStatus(bar);
                    break;
                case "Couples Night":
                    bar = new Bar(this, numberOfGuestsAtATime: 2);
                    UpdateBarStatus(bar);
                    break;
                case "Bus coming":
                    bar = new Bar(this, bouncerHalfAsSlow: true, numberOfGuestsAtATime: 15);
                    UpdateBarStatus(bar);
                    break;
                default:
                    break;
            }
        }

        private void OnCloseBarButtonClicked(object sender, RoutedEventArgs e)
        {
            closeBarButton.IsEnabled = false;
            openBarButton.IsEnabled = true;
            Thread.Sleep(100);
            bar.IsOpen = false;
        }

        public void UpdateBarStatus(Bar bar)
        {
            timeSpan = TimeSpan.FromSeconds(bar.TimeOpenBar);
            dispatcherTimer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                string status = (bar.IsOpen) ? "open" : "closed";
                barStatusTextBox.Text = $"The bar is {status}!";
                if (bar.IsOpen) barStatusTextBox.Text += "\nThe bar is closing in: " + timeSpan.ToString("c");
                else timeSpan = TimeSpan.Zero;
                if (timeSpan == TimeSpan.Zero)
                {
                    dispatcherTimer.Stop();
                    bar.IsOpen = false;
                    barStatusTextBox.Text = "The bar is closed";
                }
                timeSpan = timeSpan.Add(TimeSpan.FromSeconds(-1));

            }, Application.Current.Dispatcher);
            dispatcherTimer.Start();
        }
    }
}
