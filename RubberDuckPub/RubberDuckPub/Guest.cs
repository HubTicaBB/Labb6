using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Guest
    {
        public string Name { get; set; }

        public Guest(string name, Bar bar, MainWindow mainWindow)
        {
            Name = name;

            Task.Run(() =>
            {
                while (bar.IsOpen)
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

        private void SitDown(Bar bar, MainWindow mainWindow)
        {
            Guest seated;
            bar.waitingToBeSeated.TryDequeue(out seated);
            Log(DateTime.Now, $"{this.Name} set down to drink his beer", mainWindow);
            bar.seatedGuests.Add(seated);

            Drink(mainWindow);

            LeaveTheBar(bar, mainWindow);
        }

        private bool WaitingForChair(Bar bar)
        {
            return (bar.waitingToBeSeated.Count > 0) ? true : false;
        }

        private bool ThereIsAnAvailableChair(Bar bar)
        {
            return (bar.emptyChairs.Count > 0) ? true : false;
        }

        private void Drink(MainWindow mainWindow)
        {
            Random random = new Random();
            Thread.Sleep(random.Next(10, 21) * 1000);
            Log(DateTime.Now, $"{this.Name} drank up his beer and left the bar", mainWindow);
        }

        private void LeaveTheBar(Bar bar, MainWindow mainWindow)
        {
            bar.seatedGuests.Remove(this);
            Thread.Sleep(2000);
            if (this.Name == "Pontus")
            {
                Log(DateTime.Now, "Pontus decided to drink one more beer", mainWindow);
                bar.guestQueue.Enqueue(this);
            }
        }

        private void Log(DateTime timestamp, string activity, MainWindow mainWindow)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.GuestsListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {activity}"));
        }
    }
}
