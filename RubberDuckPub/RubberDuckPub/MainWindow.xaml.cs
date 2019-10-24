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

namespace RubberDuckPub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

            testButton.Click += OnTestButtonClicked;
        }

        private void OnTestButtonClicked(object sender, RoutedEventArgs e)
        {
            // Bar bar;
            switch (testComboBox.SelectedItem)
            {
                case "Standard Settings":
                    // bar = new Bar(this); constructor använder default värdena på properties
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
    }
}
