using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Bar
    {
        MainWindow mainWindow;
        public ConcurrentStack<Glasses> cleanGlasses = new ConcurrentStack<Glasses>();
        public ConcurrentStack<Glasses> dirtyGlasses = new ConcurrentStack<Glasses>();
        public ConcurrentStack<Chairs> emptyChairs = new ConcurrentStack<Chairs>();
        public ConcurrentQueue<Guest> guestQueue = new ConcurrentQueue<Guest>();
        public ConcurrentQueue<Guest> guestsWaitingForTable = new ConcurrentQueue<Guest>();
        public ConcurrentBag<Guest> seatedGuests = new ConcurrentBag<Guest>();
        public bool IsOpen { get; set; }
        public int TotalNumberGuests { get; set; } = 0;
        public int NumberOfGlasses { get; }
        public int NumberOfChairs { get; }
        public bool GuestsStayingDoubleTime { get; }
        public int TimeOpenBar { get; }       

        public Bar(MainWindow mainWindow,
                   int numberOfGlasses = 8,
                   int numberOfChairs = 9,
                   bool guestsStayingDoubleTime = false,
                   bool waiterTwiceAsFast = false,
                   int openingSeconds = 120,
                   int numberOfGuestsAtATime = 1,
                   bool couplesNight = false,
                   bool busIsComing = false)
        {
            this.mainWindow = mainWindow;
            IsOpen = true;
            NumberOfGlasses = numberOfGlasses;
            NumberOfChairs = numberOfChairs;
            GuestsStayingDoubleTime = guestsStayingDoubleTime;
            TimeOpenBar = openingSeconds;

            PushGlasses(NumberOfGlasses);
            PushChairs(NumberOfChairs);

            Bouncer bouncer = new Bouncer(this, mainWindow, numberOfGuestsAtATime, busIsComing, couplesNight);
            Bartender bartender = new Bartender(this, mainWindow);
            Waiter waiter = new Waiter(this, mainWindow, waiterTwiceAsFast);

            Task.Run(() =>
            {
                RunBar(waiter, bartender);
            });
        }

        public void RunBar(Waiter waiter, Bartender bartender)
        {
            while (IsOpen || waiter.IsWorking || bartender.IsWorking)
            {
                UpdateBarContent(mainWindow);
                Thread.Sleep(1000); // to make blinking not so obvious
            }
        }

        private void UpdateBarContent(MainWindow mainWindow)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.barContentListBox.Items.Clear());
            mainWindow.Dispatcher.Invoke(() => mainWindow.barContentListBox.Items.Add($"There are {TotalNumberGuests} guests in the bar."));
            mainWindow.Dispatcher.Invoke(() => mainWindow.barContentListBox.Items.Add($"There are {cleanGlasses.Count} glasses on the shelf " +
                                                                                      $"({NumberOfGlasses} total)"));
            mainWindow.Dispatcher.Invoke(() => mainWindow.barContentListBox.Items.Add($"There are {emptyChairs.Count} available chairs " +
                                                                                      $"({NumberOfChairs} total)"));
        }

        public void PushGlasses(int numberOfGlasses)
        {
            for (int i = 0; i < numberOfGlasses; i++)
            {
                cleanGlasses.Push(new Glasses());
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
