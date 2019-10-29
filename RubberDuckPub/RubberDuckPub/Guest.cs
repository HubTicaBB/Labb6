using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Guest
    {
        public string Name { get; set; }
        public MainWindow mainWindow { get; set; }
        public Bar bar { get; set; }
        public int TimeToGoToTable { get; set; } = 4000;
        public bool IsInBar { get; set; } = true;

        public Guest(string name, Bar bar, MainWindow mainWindow)
        {
            Name = name;
            this.mainWindow = mainWindow;
            this.bar = bar;
            StartGuest();
            //Task.Run(() =>
            //{
            //    while (IsInBar)
            //    {
            //        if (bar.guestWaitingForTableQueue.Count > 0)
            //        {
            //            Guest nextToBeServed;
            //            bar.guestWaitingForTableQueue.TryDequeue(out nextToBeServed);
            //            if (nextToBeServed != null)
            //            {
            //                Log(DateTime.Now, $"{nextToBeServed.Name} is searching for an available seat.", mainWindow);
            //                SearchForEmptyChair(bar, mainWindow, nextToBeServed);
            //            }
            //        }
            //    }
            //});
        }

        private void StartGuest()
        {
            Task.Run(() =>
            {
                while (IsInBar)
                {
                    if (bar.guestWaitingForTableQueue.Count > 0)
                    {
                        Guest nextToBeServed;
                        bar.guestWaitingForTableQueue.TryDequeue(out nextToBeServed);
                        if (nextToBeServed != null)
                        {
                            Log(DateTime.Now, $"{nextToBeServed.Name} is searching for an available seat.");
                            SearchForEmptyChair(nextToBeServed);
                        }
                    }
                }
            });
        }

        private void SearchForEmptyChair(Guest nextToBeSeated)
        {
            if (bar.emptyChairs.Count > 0)
            {
                Thread.Sleep(TimeToGoToTable);
                Log(DateTime.Now, $"{nextToBeSeated.Name} is sitting at the table.");
                bar.seatedGuests.Add(nextToBeSeated);
                bar.emptyChairs.TryPop(out Chairs removedChair);
                DrinkBeer(removedChair, nextToBeSeated);
            }
        }

        private void DrinkBeer(Chairs removedChair, Guest drinkingGuest)
        {
            Random r = new Random();
            Log(DateTime.Now, $"{drinkingGuest.Name} is drinking beer");
            int secondsToDrinkBeer = r.Next(20, 31);
            if (bar.GuestsStayingDouble)
                secondsToDrinkBeer *= 2;
            Thread.Sleep(secondsToDrinkBeer * 1000);
            GoHome(removedChair, drinkingGuest);
        }

        private void GoHome(Chairs removedChair, Guest leavingGuest)
        {
            bar.seatedGuests.Remove(this);
            Log(DateTime.Now, $"{leavingGuest.Name} finished the beer and goes home.");
            IsInBar = false;
            bar.dirtyGlassesStack.Push(new Glasses());
            bar.emptyChairs.Push(removedChair);
            bar.TotalNumberGuests--;
        }

        private void Log(DateTime timestamp, string activity)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.GuestsListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {activity}"));
        }
    }
}
