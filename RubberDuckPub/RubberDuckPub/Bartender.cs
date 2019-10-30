using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Bartender
    {
        public MainWindow mainWindow { get; set; }
        public Bar bar { get; set; }

        public bool IsWorking { get; set; }

        public Bartender(Bar bar, MainWindow mainWindow)
        {
            this.bar = bar;
            this.mainWindow = mainWindow;

            StartBartender();
        }

        private void StartBartender()
        {
            Task.Run(() =>
            {
                while (bar.IsOpen || bar.TotalNumberGuests > 0)
                {
                    IsWorking = true;
                    CheckIfGuestsAreWaiting();
                }
                GoHome();
            });
        }

        private void CheckIfGuestsAreWaiting()
        {
            if (bar.guestQueue.Count == 0)
            {
                Log(DateTime.Now, "Waiting for guests at the bar.");
                while (bar.guestQueue.Count == 0 && (bar.IsOpen || bar.TotalNumberGuests > 0))
                {

                }
            }
            else
            {
                Log(DateTime.Now, "Going to the shelf.");
                GoToShelf();
            }
        }

        private void GoToShelf()
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
                Log(DateTime.Now, "Picking up a glass from the shelf.");
                bar.cleanGlassesStack.TryPop(out Glasses glass);
                Thread.Sleep(3000);
                bool dequeued = bar.guestQueue.TryDequeue(out Guest dequeuedGuest);
                if (dequeued)
                {
                    ServeBeer(dequeuedGuest);
                }
            }
        }

        private void ServeBeer(Guest dequeuedGuest)
        {
            Log(DateTime.Now, $"Pouring a beer to {dequeuedGuest.Name}.");
            //dequeuedGuest.HasBeer = true;
            bar.guestWaitingForTableQueue.Enqueue(dequeuedGuest);
            dequeuedGuest.HasBeer = true;
            Thread.Sleep(3000);
        }

        private void GoHome()
        {
            Log(DateTime.Now, "Bartender goes home.");
            IsWorking = false;
        }

        private void Log(DateTime timestamp, string activity)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.BartenderListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {activity}"));
        }
    }
}
