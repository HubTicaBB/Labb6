using System;
using System.Collections.Generic;
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
                "Case 1:    The bar has 8 glasses and 9 chairs.",
                "Case 2:    The bar has 20 glasses and 3 chairs.",
                "Case 3:    The bar has 5 glasses and 20 chairs.",
                "Case 4:    The guests are staying double time in the bar.",
                "Case 5:    The waiter is picking up glasses and doing dishes twice as fast.",
                "Case 6:    The bar is open for 5 minutes.",
                "Case 7:    Couples Night (The guests are coming inside the bar in couples).",
                "Case 8:    A bus with 15 guests is coming at the bar."
            };
            testComboBox.ItemsSource = testCases;

            openBarButton.Click += OnOpenBarButtonClicked;
            closeBarButton.Click += OnCloseBarButtonClicked;
            changeSpeedRadioButton.Checked += OnRadioButtonChecked;
        }

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
            changeSpeedRadioButton.IsEnabled = false;
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
                case "Case 1:    The bar has 8 glasses and 9 chairs.":
                    bar = new Bar(this, speed);
                    break;
                case "Case 2:    The bar has 20 glasses and 3 chairs.":
                    bar = new Bar(this, speed, numberOfGlasses: 20, numberOfChairs: 3);
                    break;
                case "Case 3:    The bar has 5 glasses and 20 chairs.":
                    bar = new Bar(this, speed, numberOfChairs: 20, numberOfGlasses: 5);
                    break;
                case "Case 4:    The guests are staying double time in the bar.":
                    bar = new Bar(this, speed, guestsStayingDouble: true);
                    break;
                case "Case 5:    The waiter is picking up glasses and doing dishes twice as fast.":
                    bar = new Bar(this, speed, waiterTwiceAsFast: true);
                    break;
                case "Case 6:    The bar is open for 5 minutes.":
                    bar = new Bar(this, speed, openingSeconds: 300);
                    break;
                case "Case 7:    Couples Night (The guests are coming inside the bar in couples).":
                    bar = new Bar(this, speed, couplesNight: true, numberOfGuestsAtATime: 2);
                    break;
                case "Case 8:    A bus with 15 guests is coming at the bar.":
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
                    closeBarButton.IsEnabled = false;
                    changeSpeedRadioButton.IsEnabled = false;
                    barStatusTextBox.Text = "The bar is closed!";
                }
                timeSpan = timeSpan.Add(TimeSpan.FromSeconds(-1));

            }, Application.Current.Dispatcher);
            dispatcherTimer.Start();
        }
    }
}
