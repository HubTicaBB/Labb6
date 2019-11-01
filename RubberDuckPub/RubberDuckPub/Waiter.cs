using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RubberDuckPub
{
    public class Waiter
    {
        public MainWindow mainWindow { get; set; }
        public Bar bar { get; set; }
        public double TimeToPickUpGlasses { get; set; } = 10000;
        public double TimeToDoDishes { get; set; } = 15000;
        public int numberOfPickedUpGlasses = 0;
        public bool IsWorking { get; set; }
        public bool TwiceAsFast { get; set; }

        public Waiter(Bar bar, MainWindow mainWindow, bool twiceAsFast)
        {
            this.bar = bar;
            this.mainWindow = mainWindow;

            TwiceAsFast = twiceAsFast;
            if (TwiceAsFast)
            {
                TimeToPickUpGlasses /= 2;
                TimeToDoDishes /= 2;
            }

            StartWaiter();
        }

        private void StartWaiter()
        {
            Task.Run(() =>
            {
                while (bar.IsOpen || bar.dirtyGlasses.Count > 0 || bar.TotalNumberGuests > 0)
                {
                    IsWorking = true;
                    CheckIfGlassesAreEmpty();
                }
                GoHome();
            });
        }

        private void CheckIfGlassesAreEmpty()
        {
            int glassesToClean = bar.dirtyGlasses.Count;
            if (glassesToClean > 0)
            {
                Log(DateTime.Now, $"Picking up {glassesToClean} empty glasses.");
                Thread.Sleep((int)(TimeToPickUpGlasses / mainWindow.CurrentSpeed()));
                DoDishes(glassesToClean);
            }
        }

        private void DoDishes(int glassesToClean)
        {
            Log(DateTime.Now, $"Doing dishes.");
            Thread.Sleep((int)(TimeToDoDishes / mainWindow.CurrentSpeed()));
            Glasses[] removedGlasses = new Glasses[glassesToClean];
            bar.dirtyGlasses.TryPopRange(removedGlasses, 0, glassesToClean);
            PutGlassBack(removedGlasses);
        }

        private void PutGlassBack(Glasses[] removedGlasses)
        {
            Log(DateTime.Now, $"Putting clean glasses in the shelf.");
            bar.cleanGlasses.PushRange(removedGlasses);
            Thread.Sleep(1000); // making sure that bar content has enough time to update after putting the last glass back
        }

        private void GoHome()
        {
            if (bar.TotalNumberGuests == 0)
            {
                Log(DateTime.Now, "Waiter goes home.");
                IsWorking = false;
                MessageBoxResult answer = mainWindow.Dispatcher.Invoke(() => 
                                          MessageBox.Show($"{mainWindow.testComboBox.SelectedItem.ToString().Substring(0,7)}\n\n" +
                                                          $"\"{mainWindow.testComboBox.SelectedItem.ToString().Substring(11)}\"\n\n" +
                                                          $"has been successfully completed. All the guests and staff went home.\n\n" +
                                                          "  - Press 'YES' to continue\n" +
                                                          "  - Press 'NO' to close the application", 
                                                          "Done", MessageBoxButton.YesNo, MessageBoxImage.Information));

                if (answer == MessageBoxResult.No)
                {
                    mainWindow.Dispatcher.Invoke(() => Application.Current.Shutdown());
                }
                else
                {
                    mainWindow.Dispatcher.Invoke(() => mainWindow.openBarButton.IsEnabled = true);
                    mainWindow.Dispatcher.Invoke(() => mainWindow.closeBarButton.IsEnabled = false);
                    mainWindow.Dispatcher.Invoke(() => mainWindow.testComboBox.IsEnabled = true);
                    mainWindow.Dispatcher.Invoke(() => mainWindow.changeSpeedRadioButton.IsEnabled = true);
                    mainWindow.Dispatcher.Invoke(() => mainWindow.SpeedListBox.SelectedItem = null);
                    mainWindow.Dispatcher.Invoke(() => mainWindow.SpeedListBox.Visibility = Visibility.Hidden);
                    mainWindow.Dispatcher.Invoke(() => mainWindow.SpeedCheckBox.IsChecked = false);
                }          
            }
        }

        private void Log(DateTime timestamp, string activity)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.WaiterListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {activity}"));
        }
    }
}
