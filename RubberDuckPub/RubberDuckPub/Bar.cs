using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RubberDuckPub
{
    public class Bar
    {
        public ConcurrentStack<int> cleanGlassesStack = new ConcurrentStack<int>();
        public ConcurrentStack<int> dirtyGlassesStack = new ConcurrentStack<int>();
        public ConcurrentStack<int> emptyChairs = new ConcurrentStack<int>();
        public ConcurrentQueue<Guest> guestQueue = new ConcurrentQueue<Guest>();
        public List<string> barContent = new List<string>();
        public int numberOfGlasses { get; set; } = 8;
        public int numberOfChairs { get; set; } = 9;
        public int timeOpenBar { get; set; } = 120;
        public bool IsOpen { get; set; }
        //public MainWindow mainWindow;

        public Bar(MainWindow mainWindow)
        {
            // assign allt som behovs
            IsOpen = true;
            PushGlasses(numberOfGlasses);
            PushChairs(numberOfChairs);
            BarContentInfo(mainWindow);
            Bouncer bouncer = new Bouncer(this, mainWindow);
            Bartender bartender = new Bartender(this, mainWindow, bouncer);
            Waiter waiter = new Waiter(this, mainWindow);
        }

        public void BarContentInfo(MainWindow mainWindow) // make it refresh after each thing that happens 
        {
            barContent.Add($"There are { guestQueue.Count} guests in the bar.");
            barContent.Add($"There are {cleanGlassesStack.Count} glasses on the shelf.");
            barContent.Add($"There are {emptyChairs.Count} available tables.");
            foreach (var item in barContent)
            {
                mainWindow.Dispatcher.Invoke(() => mainWindow.barContentListBox.Items.Insert(0, item));
            }
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
