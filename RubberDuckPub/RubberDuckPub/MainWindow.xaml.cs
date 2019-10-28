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
                "The waiter picks upp glasses in half the time",
                "Bar working hours: 5 minutes",
                "Couples Night",
                "Bus comming"
            };
            testComboBox.ItemsSource = testCases;

            openBarButton.Click += OnOpenBarButtonClicked;
        }
        //public ManualResetEvent PauseBartender = new ManualResetEvent(true); //is not really working 
        private void OnOpenBarButtonClicked(object sender, RoutedEventArgs e)
        {

            Bar bar;
            switch (testComboBox.SelectedItem)
            {
                case "Standard Settings":
                    bar = new Bar(this);
                    CountDown(bar);
                    break;
                case "20 glasses, 3 chairs":
                    // bar = new Bar(this, 20, 3);
                    // constructor overload:
                    // public Bar(MainWindow mainWindow, int numberOfGlasses, int numberOfChairs)
                    // {
                    //      NumberOfGlasses = numberOfGlasses;
                    //      NumberOfChairs = numberOfChairs;
                    // }
                    break;
                case "20 chairs, 5 glasses":
                    // bar = new Bar(this, 5, 20);  // samma konstruktorn som i caset ovan
                    break;
                case "Guests staying double time":
                    // bar = new Bar(this);
                    break;
                case "The waiter picks upp glasses in half the time":
                    // class Waiter { int PickUpTime = 10000 by default }
                    // bar = new Bar(this, 5000)
                    // {
                    //      // gör allt som vanligt, fast skicka 5000 med Waiter konstruktorn
                    //      Waiter waiter = new Waiter(this, 5000)
                    //      public Waiter(Bar bar, MainWindow mainwindow)
                    // }
                    //
                    break;
                case "Bar working hours: 5 minutes":
                    // bar = new Bar(this);
                    break;
                case "Couples Night":
                    // bar = new Bar(this);
                    break;
                case "Bus comming":
                    // bar = new Bar(this);
                    break;
                default:
                    break;
            }


        }
        public void CountDown(Bar bar)
        {
            timeSpan = TimeSpan.FromSeconds(bar.TimeOpenBar);
            dispatcherTimer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate

            {
                barStatusTextBox.Text = "The bar is open!\nThe bar is closing in: " + timeSpan.ToString("c");
                if (timeSpan == TimeSpan.Zero)
                    dispatcherTimer.Stop();
                timeSpan = timeSpan.Add(TimeSpan.FromSeconds(-1));

            }, Application.Current.Dispatcher);
            dispatcherTimer.Start();
        }


    }
}
