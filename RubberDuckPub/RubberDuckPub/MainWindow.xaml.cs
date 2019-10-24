using System.Collections.Generic;
using System.Windows;

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
        }

    }
}
