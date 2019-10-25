using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Bar
    {
        public ConcurrentStack<Glasses> cleanGlassesStack = new ConcurrentStack<Glasses>();
        public ConcurrentStack<Glasses> dirtyGlassesStack = new ConcurrentStack<Glasses>();
        public ConcurrentStack<Chairs> emptyChairs = new ConcurrentStack<Chairs>();
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
            mainWindow.Dispatcher.Invoke(() => mainWindow.waitingAtBarLabel.Content = guestQueue.Count.ToString());
            mainWindow.Dispatcher.Invoke(() => mainWindow.waitingForChairLabel.Content = waitingToBeSeated.Count.ToString());
            mainWindow.Dispatcher.Invoke(() => mainWindow.drinkingLabel.Content = seatedGuests.Count.ToString());
            mainWindow.Dispatcher.Invoke(() => mainWindow.glassesOnShelfLabel.Content = cleanGlassesStack.Count.ToString());
            mainWindow.Dispatcher.Invoke(() => mainWindow.glassesTotalLabel.Content = numberOfGlasses.ToString());
            mainWindow.Dispatcher.Invoke(() => mainWindow.availableChairsLabel.Content = emptyChairs.Count.ToString());
            mainWindow.Dispatcher.Invoke(() => mainWindow.chairsTotalLabel.Content = numberOfChairs.ToString());
        }

        public void PushGlasses(int numberOfGlasses)
        {
            for (int i = 0; i < numberOfGlasses; i++)
            {
                cleanGlassesStack.Push(new Glasses());
            }
        }
        public void PushChairs(int numberOfChairs)
        {
            for (int i = 0; i < numberOfChairs; i++)
            {
                emptyChairs.Push(new Chairs());
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
