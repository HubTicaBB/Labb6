﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Waiter
    {
        public MainWindow mainWindow { get; set; }
        public Bar bar { get; set; }
        public int TimeToPickUpGlasses { get; set; } = 10000;
        public int TimeToDoDishes { get; set; } = 15000;
        public int numberOfPickedUpGlasses = 0;
        public bool IsWorking { get; set; }
        public bool TwiceAsFast { get; set; }

        public Waiter(Bar bar, MainWindow mainWindow, bool twiceAsFast)
        {
            this.bar = bar;
            this.mainWindow = mainWindow;
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
            int glassesToClean = bar.dirtyGlassesStack.Count; // save count in a variable so that is static 
            if (glassesToClean > 0)
            {
                Log(DateTime.Now, $"Picking up {glassesToClean} empty glasses.");
                Thread.Sleep(TimeToPickUpGlasses);
                DoDishes(glassesToClean);
            }
        }

        private void DoDishes(int glassesToClean)
        {
            Log(DateTime.Now, $"Doing dishes.");
            Thread.Sleep(TimeToDoDishes);
            Glasses[] removedGlasses = new Glasses[glassesToClean];
            bar.dirtyGlassesStack.TryPopRange(removedGlasses, 0, glassesToClean);  // remove dirty glasses because they are clean now
            PutGlassBack(removedGlasses);
        }

        private void PutGlassBack(Glasses[] removedGlasses)
        {
            Log(DateTime.Now, $"Putting clean glasses in the shelf.");
            bar.cleanGlassesStack.PushRange(removedGlasses);
            Thread.Sleep(100); // för att content listbox har tid att uppdatera sig
        }

        private void GoHome()
        {
            if (bar.TotalNumberGuests == 0)
            {
                Log(DateTime.Now, "Waiter goes home.");
                IsWorking = false;
            }
        }

        private void Log(DateTime timestamp, string activity)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.WaiterListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {activity}"));
        }
    }
}
