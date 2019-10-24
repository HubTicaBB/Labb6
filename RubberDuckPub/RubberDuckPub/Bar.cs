using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Bar
    {
        public ConcurrentStack<int> cleanGlassesStack = new ConcurrentStack<int>();
        public ConcurrentStack<int> dirtyGlassesStack = new ConcurrentStack<int>();
        public ConcurrentStack<int> emptyChairs = new ConcurrentStack<int>();
        public ConcurrentQueue<Guest> guestQueue = new ConcurrentQueue<Guest>();
        public ConcurrentQueue<Guest> waitingToBeSeated = new ConcurrentQueue<Guest>();
        public List<Guest> seatedGuests = new List<Guest>();
        public int numberOfGlasses { get; set; } = 8;
        public int numberOfChairs = 9;
        public int timeOpenBar = 120;
        public bool IsOpen { get; set; }
        
        public Bar(MainWindow mainWindow)
        {
            // assign allt som behovs
            IsOpen = true;
            PushGlasses(numberOfGlasses);
            PushChairs(numberOfChairs);
            Bouncer bouncer = new Bouncer(this, mainWindow);
            Bartender bartender = new Bartender(this, mainWindow);            
            Waiter waiter = new Waiter(this, mainWindow);
            Task.Run(() =>
            {
                while (IsOpen)
                {
                    UpdateContentLabels(mainWindow);
                    Thread.Sleep(100);
                }
            });
        }

        private void UpdateContentLabels(MainWindow mainWindow)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.waitingAtBarLabel.Content = $"{guestQueue.Count} guests");
            mainWindow.Dispatcher.Invoke(() => mainWindow.waitingForChairLabel.Content = $"{waitingToBeSeated.Count} guests");
            mainWindow.Dispatcher.Invoke(() => mainWindow.drinkingLabel.Content = $"{seatedGuests.Count} guests");
            mainWindow.Dispatcher.Invoke(() => mainWindow.glassesOnShelfLabel.Content = $"{cleanGlassesStack.Count}");
            mainWindow.Dispatcher.Invoke(() => mainWindow.glassesTotalLabel.Content = $"{numberOfGlasses}");
            mainWindow.Dispatcher.Invoke(() => mainWindow.availableChairsLabel.Content = $"{emptyChairs.Count}");
            mainWindow.Dispatcher.Invoke(() => mainWindow.chairsTotalLabel.Content = $"{numberOfChairs}");
        }

        public void PushGlasses(int numberOfGlasses)
        {
            for (int i = 0; i < numberOfGlasses; i++)
            {
                cleanGlassesStack.Push(i);
            }
        }
        public void PushChairs(int numberOfChairs)
        {
            for (int i = 0; i < numberOfChairs; i++)
            {
                emptyChairs.Push(i);
            }
        }

        //public Action pushGlasses = () =>
        //{
        //    for (int i = 0; i < numberOfGlasses; i++)
        //    {
        //        cleanGlassesStack.Push(i);

        //    }
        //};

        //Action pushChairs = () =>
        // {
        //     for (int i = 0; i < numberOfChairs; i++)
        //     {
        //         emptyChairs.Push(i);
        //     }
        // };
    }
}
