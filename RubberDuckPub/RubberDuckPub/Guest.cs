using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Guest
    {
        public string Name { get; set; }
        public int TimeToGoToTable { get; set; } = 4000;
        public bool IsInBar { get; set; } = true;

        public Guest(string name, Bar bar, MainWindow mainWindow)
        {
            Name = name;
            Task.Run(() =>
            {
                while (bar.IsOpen && IsInBar)
                {
                    if (bar.guestWaitingForTableQueue.Count > 0)
                    {
                        Log(DateTime.Now, $"{this.Name} is searching for an available seat.", mainWindow);
                        SearchForEmptyChair(bar, mainWindow);
                    }
                }
            });
        }

        private void SearchForEmptyChair(Bar bar, MainWindow mainWindow)
        {
            if (bar.emptyChairs.Count > 0)
            {

                Thread.Sleep(TimeToGoToTable);
                Log(DateTime.Now, $"{this.Name} is sitting at the table.", mainWindow);
                bar.emptyChairs.TryPop(out Chairs removedChair);
                mainWindow.Dispatcher.Invoke(() => bar.BarContentInfo(mainWindow, bar.cleanGlassesStack.Count, bar.emptyChairs.Count));
                DrinkBeer(bar, mainWindow, removedChair);
            }
        }

        private void DrinkBeer(Bar bar, MainWindow mainWindow, Chairs removedChair)
        {
            Random r = new Random();
            Log(DateTime.Now, $"{this.Name} is drinking beer", mainWindow);
            int secondsToDrinkBeer = r.Next(10, 21);
            Thread.Sleep(secondsToDrinkBeer * 1000);
            GoHome(bar, mainWindow, removedChair);
        }

        private void GoHome(Bar bar, MainWindow mainWindow, Chairs removedChair)
        {
            Guest servedGuest;
            Log(DateTime.Now, $"{this.Name} finished the beer and goes home.", mainWindow);
            bar.guestWaitingForTableQueue.TryDequeue(out servedGuest);
            IsInBar = false;
            bar.dirtyGlassesStack.Push(new Glasses());
            bar.emptyChairs.Push(removedChair);
            bar.TotalNumberGuests--;
            mainWindow.Dispatcher.Invoke(() => bar.BarContentInfo(mainWindow, bar.cleanGlassesStack.Count, bar.emptyChairs.Count));
        }

        private void Log(DateTime timestamp, string activity, MainWindow mainWindow)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.GuestsListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {activity}"));
        }
    }
}
