using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Waiter
    {
        public MainWindow mainWindow { get; set; }
        public Bar bar { get; set; }
        public double TimeToPickUpGlasses { get; set; }
        public double TimeToDoDishes { get; set; }
        public int numberOfPickedUpGlasses = 0;
        public bool IsWorking { get; set; }
        public bool TwiceAsFast { get; set; }

        public Waiter(Bar bar, MainWindow mainWindow, bool twiceAsFast)
        {
            this.bar = bar;
            this.mainWindow = mainWindow;
            TimeToPickUpGlasses = 10000 / bar.Speed;
            TimeToDoDishes = 15000 / bar.Speed;

            TwiceAsFast = twiceAsFast;
            if (TwiceAsFast)
            {
                TimeToPickUpGlasses /= 2;
                TimeToDoDishes /= 2;
            }

            StartWaiter();
        }

        private void StartWaiter()
        {
            Task.Run(() =>
            {
                while (bar.IsOpen || bar.dirtyGlassesStack.Count > 0 || bar.TotalNumberGuests > 0)
                {
                    IsWorking = true;
                    CheckIfGlassesAreEmpty();
                }
                GoHome();
            });
        }

        private void CheckIfGlassesAreEmpty()
        {
            int glassesToClean = bar.dirtyGlassesStack.Count;
            if (glassesToClean > 0)
            {
                Log(DateTime.Now, $"Picking up {glassesToClean} empty glasses.");
                Thread.Sleep((int)TimeToPickUpGlasses);
                DoDishes(glassesToClean);
            }
        }

        private void DoDishes(int glassesToClean)
        {
            Log(DateTime.Now, $"Doing dishes.");
            Thread.Sleep((int)TimeToDoDishes);
            Glasses[] removedGlasses = new Glasses[glassesToClean];
            bar.dirtyGlassesStack.TryPopRange(removedGlasses, 0, glassesToClean);
            PutGlassBack(removedGlasses);
        }

        private void PutGlassBack(Glasses[] removedGlasses)
        {
            Log(DateTime.Now, $"Putting clean glasses in the shelf.");
            bar.cleanGlassesStack.PushRange(removedGlasses);
            Thread.Sleep(1000);
        }

        private void GoHome()
        {
            if (bar.TotalNumberGuests == 0)
            {
                Log(DateTime.Now, "Waiter goes home.");
                IsWorking = false;
                mainWindow.Dispatcher.Invoke(() => mainWindow.openBarButton.IsEnabled = true);
                mainWindow.Dispatcher.Invoke(() => mainWindow.closeBarButton.IsEnabled = false);
            }
        }

        private void Log(DateTime timestamp, string activity)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.WaiterListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {activity}"));
        }
    }
}
