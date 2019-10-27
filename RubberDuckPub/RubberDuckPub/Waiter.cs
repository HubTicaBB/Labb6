using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Waiter
    {
        public int TimeToPickUpGlasses { get; set; } = 10000;
        public int TimeToDoDishes { get; set; } = 15000;
        public int numberOfPickedUpGlasses = 0;
        public Waiter(Bar bar, MainWindow mainWindow)
        {
            // add properties in the constructor
            Task.Run(() =>
            {
                while (bar.IsOpen)
                {

                    CheckIfGlassesAreEmpty(bar, mainWindow);
                }
                //GoHome() after all guests went home:  mainWindow.GuestsListBox.Items.Count == 0;

            });
        }

        private void CheckIfGlassesAreEmpty(Bar bar, MainWindow mainWindow)
        {
            int glassesToClean = bar.dirtyGlassesStack.Count;
            if (glassesToClean > 0)
            {
                //Glasses[] removedGlasses = new Glasses[bar.dirtyGlassesStack.Count];
                Log(DateTime.Now, $"Picking up {glassesToClean} empty glasses.", mainWindow);
                //bar.dirtyGlassesStack.TryPopRange(removedGlasses, 0, bar.dirtyGlassesStack.Count); 
                //numberOfPickedUpGlasses = removedGlasses.Length;
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
            //numberOfPickedUpGlasses = removedGlasses.Length;
            PutGlassBack(bar, mainWindow, glassesToClean);
        }

        private void PutGlassBack(Bar bar, MainWindow mainWindow, int numberOfPickedUpGlasses)
        {
            Log(DateTime.Now, $"Putting clean glasses in the shelf.", mainWindow);
            for (int i = 0; i < numberOfPickedUpGlasses; i++)
            {
                bar.cleanGlassesStack.Push(new Glasses());
            }
            mainWindow.Dispatcher.Invoke(() => bar.BarContentInfo(mainWindow, numberOfPickedUpGlasses, bar.emptyChairs.Count));
        }

        private void Log(DateTime timestamp, string activity, MainWindow mainWindow)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.WaiterListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {activity}"));
        }
    }
}
