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

        public Bar(MainWindow mainWindow)
        {
            // assign allt som behovs
            IsOpen = true;
            PushGlasses(numberOfGlasses);
            PushChairs(numberOfChairs);
            BarContentInfo(mainWindow, guestQueue.Count, cleanGlassesStack.Count, emptyChairs.Count);
            Bouncer bouncer = new Bouncer(this, mainWindow);
            Bartender bartender = new Bartender(this, mainWindow, bouncer);
            Waiter waiter = new Waiter(this, mainWindow);
        }


        public void BarContentInfo(MainWindow mainWindow, int numberGuests, int numberCleanGlasses, int numberEmptyChairs) // make it refresh after each thing that happens 
        {
            barContent.Clear();

            barContent.Add($"There are { numberGuests} guests in the bar.");
            barContent.Add($"There are {numberCleanGlasses} glasses on the shelf.");
            barContent.Add($"There are {numberEmptyChairs} available tables.");
            mainWindow.barContentListBox.ItemsSource = barContent;
            mainWindow.barContentListBox.Items.Refresh();
            //mainWindow.barContentListBox.UpdateLayout();
            mainWindow.Dispatcher.Invoke(() => mainWindow.barContentListBox.Items);
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
