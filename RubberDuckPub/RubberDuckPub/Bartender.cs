using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Bartender
    {
        public Bartender(Bar bar, MainWindow mainWindow, Bouncer bouncer)
        {
            //bar.mainWindow.PauseBartender.WaitOne(Timeout.Infinite);
            Task.Run(() =>
            {
                while (bar.IsOpen)
                {
                    CheckIfGuestsAreWaiting(bar, mainWindow, bouncer);
                }
                //GoHome() after all lists with guests are empty 

            });

        }

        private void CheckIfGuestsAreWaiting(Bar bar, MainWindow mainWindow, Bouncer bouncer)
        {
            if (bar.guestQueue.Count == 0)
            {

                Log(DateTime.Now, "Bartender is waiting for guests at the bar.", mainWindow);
                int waitForGuest = bouncer.seconds;
                Thread.Sleep(waitForGuest * 1000);  //
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
                Glasses removedGlass;
                Log(DateTime.Now, "Bartender is picking up a glass from the shelf.", mainWindow);
                bar.cleanGlassesStack.TryPop(out removedGlass);
                mainWindow.Dispatcher.Invoke(() => bar.BarContentInfo(mainWindow, mainWindow.GuestsListBox.Items.Count, bar.cleanGlassesStack.Count, bar.emptyChairs.Count));
                ServeBeer(bar, dequeuedGuest, mainWindow, removedGlass);
            }
        }

        private void ServeBeer(Bar bar, Guest dequeuedGuest, MainWindow mainWindow, Glasses removedGlass)
        {
            Log(DateTime.Now, $"Bartender is pouring a beer to {dequeuedGuest.Name}.", mainWindow);
            bar.dirtyGlassesStack.Push(removedGlass);
            Thread.Sleep(3000);
            bar.guestWaitingForTableQueue.Enqueue(dequeuedGuest);
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
