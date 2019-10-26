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
                Log(DateTime.Now, "Waiting for guests at the bar.", mainWindow);
                Thread.Sleep(10000);  //
            }
            else
            {                
                Log(DateTime.Now, "Going to the shelf.", mainWindow); 
                GoToShelf(bar, mainWindow);
            }
        }

        private void GoToShelf(Bar bar, MainWindow mainWindow)
        {
            if (bar.cleanGlassesStack.Count > 0)
            {                
                Log(DateTime.Now, "Picking up a glass from the shelf.", mainWindow);
                Glasses glass;
                if (bar.cleanGlassesStack.TryPop(out glass))
                {
                    Thread.Sleep(3000);
                    Guest dequeuedGuest;
                    if (bar.guestQueue.TryDequeue(out dequeuedGuest))
                    {
                        ServeBeer(glass, bar, dequeuedGuest, mainWindow);
                    }
                }                                     
            }
        }

        private void ServeBeer(Glasses glass, Bar bar, Guest dequeuedGuest, MainWindow mainWindow)
        {
            bar.glassesInUse.Add(glass);
            Log(DateTime.Now, $"Pouring a beer to {dequeuedGuest.Name}.", mainWindow);
            bar.waitingToBeSeated.Enqueue(dequeuedGuest);
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
