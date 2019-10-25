using System.Collections.Concurrent;

namespace RubberDuckPub
{
    public class Bar
    {
        public ConcurrentStack<int> cleanGlassesStack = new ConcurrentStack<int>();
        public ConcurrentStack<int> dirtyGlassesStack = new ConcurrentStack<int>();
        public ConcurrentStack<int> emptyChairs = new ConcurrentStack<int>();
        public ConcurrentQueue<Guest> guestQueue = new ConcurrentQueue<Guest>();
        public int numberOfGlasses { get; set; } = 8;
        public int numberOfChairs = 9;
        public int timeOpenBar = 120;
        public bool IsOpen { get; set; }
        public MainWindow mainWindow;

        public Bar(MainWindow mainWindow)
        {
            // assign allt som behovs
            IsOpen = true;
            PushGlasses(numberOfGlasses);
            PushChairs(numberOfChairs);
            Bouncer bouncer = new Bouncer(this, mainWindow);
            Bartender bartender = new Bartender(this, mainWindow, bouncer);
            Waiter waiter = new Waiter(this, mainWindow);
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
