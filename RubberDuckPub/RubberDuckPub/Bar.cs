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
        public ConcurrentQueue<Guest> guestWaitingForTableQueue = new ConcurrentQueue<Guest>();
        public ConcurrentBag<Guest> seatedGuests = new ConcurrentBag<Guest>();
        //public List<Guest> seatedGuests = new List<Guest>();
        public List<string> barContent = new List<string>();
        public int TotalNumberGuests { get; set; } = 0;
        public int NumberOfGlasses { get; set; }
        public int NumberOfChairs { get; set; }
        public bool GuestsStayingDouble { get; set; }
        public int TimeOpenBar { get; set; }
        public bool IsOpen { get; set; }

        public Bar(MainWindow mainWindow,
                   int numberOfGlasses = 8,
                   int numberOfChairs = 9,
                   bool guestsStayingDouble = false,
                   bool waiterTwiceAsFast = false,
                   int openingSeconds = 120,
                   int numberOfGuestsAtATime = 1,
                   bool bouncerHalfAsSlow = false)
        {
            IsOpen = true;
            NumberOfGlasses = numberOfGlasses;
            NumberOfChairs = numberOfChairs;
            GuestsStayingDouble = guestsStayingDouble;
            TimeOpenBar = openingSeconds;

            PushGlasses(NumberOfGlasses);
            PushChairs(NumberOfChairs);

            Bouncer bouncer = new Bouncer(this, mainWindow, numberOfGuestsAtATime, bouncerHalfAsSlow);
            Bartender bartender = new Bartender(this, mainWindow);
            Waiter waiter = new Waiter(this, mainWindow, waiterTwiceAsFast);

            Task.Run(() =>
            {
                while (IsOpen || waiter.IsWorking || bartender.IsWorking)
                {
                    UpdateBarContent(mainWindow);
                    Thread.Sleep(101);
                }
            });
        }

        private void UpdateBarContent(MainWindow mainWindow)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.barContentListBox.Items.Clear());
            mainWindow.Dispatcher.Invoke(() => mainWindow.barContentListBox.Items.Add($"There are {TotalNumberGuests} guests in the bar."));
            mainWindow.Dispatcher.Invoke(() => mainWindow.barContentListBox.Items.Add($"There are {cleanGlassesStack.Count} glasses on the shelf " +
                                                                                      $"({NumberOfGlasses} total)"));
            mainWindow.Dispatcher.Invoke(() => mainWindow.barContentListBox.Items.Add($"There are {emptyChairs.Count} available chairs " +
                                                                                      $"({NumberOfChairs} total)"));
        }


        //public void BarContentInfo(MainWindow mainWindow, int numberCleanGlasses, int numberEmptyChairs) // make it refresh after each thing that happens 
        //{
        //    ////barContent.Clear();

        //    ////barContent.Add($"There are { TotalNumberGuests} guests in the bar.");
        //    ////barContent.Add($"There are {numberCleanGlasses} glasses on the shelf ({NumberOfGlasses} total).");
        //    ////barContent.Add($"There are {numberEmptyChairs} available tables ({NumberOfChairs} total).");
        //    ////mainWindow.barContentListBox.ItemsSource = barContent;
        //    ////mainWindow.barContentListBox.Items.Refresh();
        //    ////mainWindow.Dispatcher.Invoke(() => mainWindow.barContentListBox.Items);
        //}

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
    }
}
