using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Waiter
    {
        public int TimeToPickUpGlasses { get; set; } = 10000;
        public int TimeToDoDishes { get; set; } = 15000;

        public Waiter(Bar bar, MainWindow mainWindow)
        {
            // add properties in the constructor
            Task.Run(() =>
            {
                while (bar.IsOpen)
                {

                    CheckIfGlassesAreEmpty(bar, mainWindow);
                }
                //GoHome(bar, mainWindow);
                //GoHome() after all guests went home:  TotalGuest == 0;

            });
        }



        private void CheckIfGlassesAreEmpty(Bar bar, MainWindow mainWindow)
        {
            int glassesToClean = bar.dirtyGlassesStack.Count;
            if (glassesToClean > 0)
            {
                Log(DateTime.Now, $"Picking up {glassesToClean} empty glasses.", mainWindow);
                Thread.Sleep(TimeToPickUpGlasses);
                DoDishes(bar, mainWindow, glassesToClean);
            }
        }

        private void DoDishes(Bar bar, MainWindow mainWindow, int glassesToClean)
        {
            Log(DateTime.Now, $"Doing dishes.", mainWindow);
            Thread.Sleep(TimeToDoDishes);

            Glasses[] removedGlasses = new Glasses[glassesToClean];
            bar.dirtyGlassesStack.TryPopRange(removedGlasses, 0, glassesToClean);
            PutGlassBack(bar, mainWindow, glassesToClean);
        }

        private void PutGlassBack(Bar bar, MainWindow mainWindow, int glassesToClean)
        {
            int cleanGlassesBefore = bar.cleanGlassesStack.Count;
            Log(DateTime.Now, $"Putting clean glasses in the shelf.", mainWindow);
            for (int i = 0; i < glassesToClean; i++)
            {
                bar.cleanGlassesStack.Push(new Glasses());
            }
            mainWindow.Dispatcher.Invoke(() => bar.BarContentInfo(mainWindow, (cleanGlassesBefore + glassesToClean), bar.emptyChairs.Count));
            GoHome(bar, mainWindow);
        }
        private void GoHome(Bar bar, MainWindow mainWindow)
        {
            if (bar.TotalNumberGuests == 0)
            {
                Log(DateTime.Now, "Goes home.", mainWindow);
            }
        }

        private void Log(DateTime timestamp, string activity, MainWindow mainWindow)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.WaiterListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {activity}"));
        }
    }
}
