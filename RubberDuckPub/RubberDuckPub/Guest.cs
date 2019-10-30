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
                while (IsInBar) // ?? do we need it ?
                {
                    if (HasBeer)
                    {
                        Log(DateTime.Now, $"{Name} is searching for an available seat.");
                        Thread.Sleep(TimeToGoToTable);
                        while (bar.emptyChairs.Count == 0) { }
                        SearchForEmptyChair();
                    }
                    Thread.Sleep(200);
                }
            });
        }

        private void SearchForEmptyChair(/*Guest nextToBeSeated*/)
        {
            int availableChairs = bar.emptyChairs.Count;  // saved in a separate variable 
            if (availableChairs > 0)  //
            {
                //Thread.Sleep(TimeToGoToTable);
                Log(DateTime.Now, $"{Name} is sitting at the table.");
                /*bool seatSucceeded =*/
                bar.guestWaitingForTableQueue.TryDequeue(out Guest seatedGuest);
                //if (seatSucceeded)
                //{
                bar.seatedGuests.Add(seatedGuest); /////
                bool chairTaken = bar.emptyChairs.TryPop(out Chairs removedChair);
                if (chairTaken)
                {
                    DrinkBeer(removedChair, seatedGuest);
                }
                //}
            }
        }

        private void DrinkBeer(Chairs removedChair, Guest seatedGuest)
        {
            Random r = new Random();
            Log(DateTime.Now, $"{seatedGuest.Name} is drinking beer");
            int secondsToDrinkBeer = r.Next(20, 31);
            if (bar.GuestsStayingDouble)
            {
                secondsToDrinkBeer *= 2;
            }
            Thread.Sleep(secondsToDrinkBeer * 1000);
            GoHome(removedChair, seatedGuest);
        }

        private void GoHome(Chairs removedChair, Guest seatedGuest)
        {
            //bar.seatedGuests.TryTake(out Guest wentHome);
            Log(DateTime.Now, $"{seatedGuest.Name} finished the beer and goes home.");
            bar.seatedGuests.TryTake(out seatedGuest);
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
