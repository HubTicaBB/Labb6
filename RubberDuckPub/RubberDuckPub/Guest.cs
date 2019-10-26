using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Guest
    {
        public string Name { get; set; }
        public bool IsInTheBar { get; set; }

        public Guest(string name, Bar bar, MainWindow mainWindow)
        {
            Name = name;
            IsInTheBar = true;

            Task.Run(() =>
            {
                while (IsInTheBar)
                {                    
                    Act(bar, mainWindow);                    
                }
            });
        }

        private void Act(Bar bar, MainWindow mainWindow)
        {            
            if (WaitingForChair(bar))
            {
                Log(DateTime.Now, $"{this.Name} is looking for a chair", mainWindow);
                if (ThereIsAnAvailableChair(bar))
                {
                    Thread.Sleep(4000);
                    SitDown(bar, mainWindow);                    
                }
            }            
        }

        private bool WaitingForChair(Bar bar)
        {
            foreach (var guest in bar.waitingToBeSeated)
            {
                if (guest.Name == Name)
                {
                    return true;
                }
            }
            return (bar.waitingToBeSeated.Count > 0) ? true : false;
        }

        private bool ThereIsAnAvailableChair(Bar bar)
        {
            return (bar.emptyChairs.Count > 0) ? true : false;
        }

        private void SitDown(Bar bar, MainWindow mainWindow)
        {
            Guest seated;
            if (bar.waitingToBeSeated.TryDequeue(out seated))
            {
                Log(DateTime.Now, $"{Name} set down to drink his beer", mainWindow);
                bar.seatedGuests.Add(seated);
                Drink(bar, mainWindow);
            } 
        }

        private void Drink(Bar bar, MainWindow mainWindow)
        {
            Random random = new Random();
            Thread.Sleep(random.Next(10, 21) * 1000);
            bar.dirtyGlassesStack.Push(bar.glassesInUse[0]);
            bar.glassesInUse.Remove(bar.glassesInUse[0]);            
            Log(DateTime.Now, $"{Name} drank up a beer and left the bar", mainWindow);
            LeaveTheBar(bar, mainWindow);
        }

        private void LeaveTheBar(Bar bar, MainWindow mainWindow)
        {
            bar.seatedGuests.Remove(this);            
            Thread.Sleep(2000);
            IsInTheBar = false;
            if (Name == "Pontus")
            {
                Log(DateTime.Now, $"{Name} decided to drink one more beer", mainWindow);
                bar.guestQueue.Enqueue(this);
                IsInTheBar = true;
            }
        }

        private void Log(DateTime timestamp, string activity, MainWindow mainWindow)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.GuestsListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {activity}"));
        }
    }
}
