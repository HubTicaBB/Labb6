using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Bartender
    {
        public Bartender(Bar bar, MainWindow mainWindow)
        {
            Task.Run(() =>
            {
                while (bar.IsOpen)
                {
                    CheckIfGuestsAreWaiting(bar, mainWindow);
                }
                //GoHome() after all lists with guests are empty 

            });

        }

        private void CheckIfGuestsAreWaiting(Bar bar, MainWindow mainWindow)
        {
            if (bar.guestQueue.Count == 0)
            {
                Log(DateTime.Now, "Bartender is waiting for guests at the bar.", mainWindow);
            }
            else
            {
                Guest dequeuedGuest;
                Log(DateTime.Now, "Bartender is going to the shelf.", mainWindow);
                bar.guestQueue.TryDequeue(out dequeuedGuest);

                GoToShelf(bar, dequeuedGuest, mainWindow);
            }
        }

        private void GoToShelf(Bar bar, Guest dequeuedGuest, MainWindow mainWindow)
        {
            if (bar.cleanGlassesStack.Count > 0)
            {
                Log(DateTime.Now, "Bartender is picking up a glass from the shelf.", mainWindow);
                Thread.Sleep(3000);
                ServeBeer(bar, dequeuedGuest, mainWindow);
            }
        }

        private void ServeBeer(Bar bar, Guest dequeuedGuest, MainWindow mainWindow)
        {
            Log(DateTime.Now, $"Bartender is pouring a beer to {dequeuedGuest.Name}.", mainWindow); // add name of guest
            Thread.Sleep(3000);
        }

        private void GoHome()
        {
            // 
        }
        private void Log(DateTime timestamp, string activity, MainWindow mainWindow)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.BartenderListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {activity}"));
        }

    }
}
