using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Bartender
    {
        public Bartender(Bar bar, MainWindow mainWindow, Bouncer bouncer)
        {

            Task.Run(() =>
            {
                while (bar.IsOpen)
                {
                    CheckIfGuestsAreWaiting(bar, mainWindow, bouncer);
                }
                if (bar.IsOpen == false)
                {
                    GoHome(bar, mainWindow);
                }
            });

        }

        private void CheckIfGuestsAreWaiting(Bar bar, MainWindow mainWindow, Bouncer bouncer)
        {
            if (bar.guestQueue.Count == 0)
            {

                Log(DateTime.Now, "Waiting for guests at the bar.", mainWindow);
                int waitForGuest = bouncer.seconds;
                Thread.Sleep(waitForGuest * 1000);  //
            }
            else
            {
                Guest dequeuedGuest;
                Log(DateTime.Now, "Going to the shelf.", mainWindow);
                bar.guestQueue.TryDequeue(out dequeuedGuest);
                GoToShelf(bar, dequeuedGuest, mainWindow);
            }
        }

        private void GoToShelf(Bar bar, Guest dequeuedGuest, MainWindow mainWindow)
        {
            if (bar.cleanGlassesStack.Count > 0)
            {
                Glasses removedGlass;
                Log(DateTime.Now, "Picking up a glass from the shelf.", mainWindow);
                bar.cleanGlassesStack.TryPop(out removedGlass);
                mainWindow.Dispatcher.Invoke(() => bar.BarContentInfo(mainWindow, bar.cleanGlassesStack.Count, bar.emptyChairs.Count));
                Thread.Sleep(3000);
                ServeBeer(bar, dequeuedGuest, mainWindow);
            }
        }
        private void ServeBeer(Bar bar, Guest dequeuedGuest, MainWindow mainWindow)
        {
            Log(DateTime.Now, $"Pouring a beer to {dequeuedGuest.Name}.", mainWindow);
            Thread.Sleep(3000);
            bar.guestWaitingForTableQueue.Enqueue(dequeuedGuest);

        }
        private void GoHome(Bar bar, MainWindow mainWindow)
        {
            if (bar.TotalNumberGuests == 0)
            {
                Log(DateTime.Now, "Goes home.", mainWindow);
            }
        }
        private void Log(DateTime timestamp, string activity, MainWindow mainWindow)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.BartenderListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {activity}"));
        }

    }
}
