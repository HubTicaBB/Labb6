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
        public bool HasBeer { get; set; }

        public Guest(string name, Bar bar, MainWindow mainWindow)
        {
            Name = name;
            this.mainWindow = mainWindow;
            this.bar = bar;

            StartGuest();
        }

        private void StartGuest()
        {
            Task.Run(() =>
            {
                while (IsInBar)
                {
                    if (HasBeer)
                    {
                        Log(DateTime.Now, $"{Name} is searching for an available seat.");
                        while (bar.emptyChairs.Count == 0) { }
                        SearchForEmptyChair();
                    }   
                }
            });
        }

        private void SearchForEmptyChair(/*Guest nextToBeSeated*/)
        {
            if (bar.emptyChairs.Count > 0)
            {
                Thread.Sleep(TimeToGoToTable);
                Log(DateTime.Now, $"{Name} is sitting at the table.");
                bool seatSucceeded = bar.guestWaitingForTableQueue.TryDequeue(out Guest seatedGuest);
                if (seatSucceeded)
                {
                    bar.seatedGuests.Add(this);
                    bool chairTaken = bar.emptyChairs.TryPop(out Chairs removedChair);
                    if (chairTaken)
                    {
                        DrinkBeer(removedChair);
                    }                    
                }                
            }                
        }

        private void DrinkBeer(Chairs removedChair)
        {
            Random r = new Random();
            Log(DateTime.Now, $"{Name} is drinking beer");         
            int secondsToDrinkBeer = r.Next(20, 31);
            if (bar.GuestsStayingDouble)
            {
                secondsToDrinkBeer *= 2;
            }               
            Thread.Sleep(secondsToDrinkBeer * 1000);
            GoHome(removedChair);
        }

        private void GoHome(Chairs removedChair)
        {
            bar.seatedGuests.Remove(this);
            Log(DateTime.Now, $"{Name} finished the beer and goes home.");
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
