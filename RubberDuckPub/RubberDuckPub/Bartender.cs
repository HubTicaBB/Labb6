using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Bartender
    {
        public bool IsWorking { get; set; }

        public Bartender(Bar bar, MainWindow mainWindow, Bouncer bouncer)
        {
            //bar.mainWindow.PauseBartender.WaitOne(Timeout.Infinite);
            Task.Run(() =>
            {
                while (bar.IsOpen || bar.TotalNumberGuests > 0)
                {
                    IsWorking = true;
                    CheckIfGuestsAreWaiting(bar, mainWindow, bouncer);                    
                }
                GoHome(mainWindow);
            });

        }

        private void CheckIfGuestsAreWaiting(Bar bar, MainWindow mainWindow, Bouncer bouncer)
        {
            if (bar.guestQueue.Count == 0)
            {
                Log(DateTime.Now, "Waiting for guests at the bar.", mainWindow);
                
                //int waitForGuest = bouncer.seconds;
                //Thread.Sleep(waitForGuest * 1000);  //
            }
            while (bar.guestQueue.Count == 0)
            {
                if (bar.TotalNumberGuests == 0 && !bar.IsOpen)
                    return;
                if (bar.guestQueue.Count > 0 && bar.cleanGlassesStack.Count > 0)
                    break;               
            }
            Log(DateTime.Now, "Going to the shelf.", mainWindow);
            GoToShelf(bar, /*dequeuedGuest,*/ mainWindow);
        }

        private void GoToShelf(Bar bar, /*Guest dequeuedGuest,*/ MainWindow mainWindow)
        {
            while (bar.cleanGlassesStack.Count == 0)
            {
                if (bar.TotalNumberGuests == 0 && !bar.IsOpen)
                {
                    return;
                }
            }
            if (bar.cleanGlassesStack.Count > 0 && bar.guestQueue.Count > 0)
            {
                Log(DateTime.Now, "Picking up a glass from the shelf.", mainWindow);
                Glasses glass;
                bar.cleanGlassesStack.TryPop(out glass /*removedGlass*/);
                Thread.Sleep(3000);
                Guest dequeuedGuest;
                bar.guestQueue.TryDequeue(out dequeuedGuest);
                ////mainWindow.Dispatcher.Invoke(() => bar.BarContentInfo(mainWindow, bar.cleanGlassesStack.Count, bar.emptyChairs.Count));
                ServeBeer(bar, dequeuedGuest, mainWindow);            
             }
        }

        private void ServeBeer(Bar bar, Guest dequeuedGuest, MainWindow mainWindow)
        {
            if (dequeuedGuest != null)
            {
                Log(DateTime.Now, $"Pouring a beer to {dequeuedGuest.Name}.", mainWindow);
                bar.guestWaitingForTableQueue.Enqueue(dequeuedGuest);
                Thread.Sleep(3000);
            }            
        }

        private void GoHome(MainWindow mainWindow)
        {
            Log(DateTime.Now, "Bartender goes home.", mainWindow);
            IsWorking = false;
        }

        private void Log(DateTime timestamp, string activity, MainWindow mainWindow)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.BartenderListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {activity}"));
        }

    }
}
