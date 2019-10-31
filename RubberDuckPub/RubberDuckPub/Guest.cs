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
        public bool IsInBar { get; set; } = true;
        public bool HasBeer { get; set; }
        public double TimeToGoToTable { get; set; }

        public Guest(string name, Bar bar, MainWindow mainWindow)
        {
            Name = name;
            this.mainWindow = mainWindow;
            this.bar = bar;
            TimeToGoToTable = 4000 / bar.Speed;

            StartGuest();
        }

        private void StartGuest()
        {
            Task.Run(() =>
            {
                while (IsInBar) // ?? do we need it ?
                {
                    if (HasBeer)
                    {
                        Log(DateTime.Now, $"{Name} is searching for an available seat.");
                        Thread.Sleep((int)TimeToGoToTable);
                        while (bar.emptyChairs.Count == 0) { }
                        SearchForEmptyChair();
                    }
                    Thread.Sleep(200); ////////////
                }
            });
        }

        private void SearchForEmptyChair()
        {
            int availableChairs = bar.emptyChairs.Count;
            if (availableChairs > 0)
            {
                Log(DateTime.Now, $"{Name} is sitting at the table.");
                bar.guestWaitingForTableQueue.TryDequeue(out Guest seatedGuest);

                bar.seatedGuests.Add(this);
                bool chairTaken = bar.emptyChairs.TryPop(out Chairs removedChair);
                if (chairTaken)
                {
                    DrinkBeer(removedChair);
                }

            }
        }

        private void DrinkBeer(Chairs removedChair)
        {
            Random r = new Random();
            Log(DateTime.Now, $"{Name} is drinking beer");
            double secondsToDrinkBeer = r.Next(20, 31);
            if (bar.GuestsStayingDouble)
            {
                secondsToDrinkBeer *= 2;
            }
            secondsToDrinkBeer = secondsToDrinkBeer * 1000 / bar.Speed;
            Thread.Sleep((int)secondsToDrinkBeer);
            GoHome(removedChair);
        }

        private void GoHome(Chairs removedChair)
        {
            Log(DateTime.Now, $"{Name} finished the beer and goes home.");
            bar.seatedGuests.TryTake(out Guest wentHome);
            //HasBeer = false;
            //IsInBar = false;
            bar.dirtyGlassesStack.Push(new Glasses());
            bar.emptyChairs.Push(removedChair);
            bar.TotalNumberGuests--;
            HasBeer = false;
            IsInBar = false;
        }

        private void Log(DateTime timestamp, string activity)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.GuestsListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {activity}"));
        }
    }
}
