using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Bouncer
    {
        MainWindow mainWindow;
        Bar bar;
        static readonly List<string> nameList = new List<string>
        {
            "Bob",
            "Tijana",
            "Andreea",
            "Wilhelm",
            "Simon",
            "Kalle",
            "Sam",
            "Linda",
            "Ellen",
            "Suzy",
            "Jane",
            "Maria",
            "Anna",
            "Margareta",
            "Elisabeth",
            "Eva",
            "Kristina",
            "Birgitta",
            "Karin",
            "Elisabet",
            "Marie",
            "Ingrid",
            "Christina",
            "Sofia",
            "Linnéa",
            "Kerstin",
            "Lena",
            "Marianne",
            "Helena",
            "Emma",
            "Linnea",
            "Johanna",
            "Inger",
            "Sara",
            "Cecilia",
            "Elin",
            "Erik",
            "Lars",
            "Karl",
            "Anders",
            "Johan",
            "Per",
            "Nils",
            "Carl",
            "Mikael",
            "Jan",
            "Hans",
            "Lennart",
            "Peter",
            "Olof",
            "Gunnar",
            "Sven",
            "Fredrik",
            "Bengt",
            "Daniel",
            "Bo",
            "Gustav",
            "Alexander",
            "Göran",
            "Åke",
            "Magnus",
            "Pontus"
        };

        public int NumberOfGuestsAtATime { get; set; }
        public bool BusIsComing { get; set; }
        public double TimeForBusToArrive { get; set; } = 20000;
        public bool CouplesNight { get; }
        public double TimeToGenerateAGuest { get; set; }
        public double TimeForGuestToGoToBar { get; set; } = 1000;

        public Bouncer(Bar bar, MainWindow mainWindow, int numberOfGuestsAtATime, bool busIsComing, bool couplesNight)
        {
            this.mainWindow = mainWindow;
            this.bar = bar;
            NumberOfGuestsAtATime = numberOfGuestsAtATime;
            BusIsComing = busIsComing;
            CouplesNight = couplesNight;

            StartBouncer();
        }

        private void StartBouncer()
        {
            Task.Run(() =>
            {
                while (bar.IsOpen)
                {
                    GenerateGuest();
                }
                GoHome();
            });
        }

        public void GenerateGuest()
        {
            Random random = new Random();
            TimeToGenerateAGuest = random.Next(3, 11) * 1000;

            // Bus is arriving 20 seconds after opening.
            // Check if it's time for bus to arrive, otherwise let in a single guest.
            if (BusIsComing)
            {
                TimeToGenerateAGuest *= 2;
                TimeForBusToArrive -= TimeToGenerateAGuest;
                if (TimeForBusToArrive <= 0)
                {
                    TimeToGenerateAGuest = TimeForBusToArrive + TimeToGenerateAGuest;
                    NumberOfGuestsAtATime = 15;
                    BusIsComing = false;
                }
            }
            Thread.Sleep((int)(TimeToGenerateAGuest / mainWindow.CurrentSpeed()));

            for (int i = 0; i < NumberOfGuestsAtATime; i++)
            {
                int index = random.Next(1, nameList.Count());
                if (!bar.IsOpen) return;
                bar.guestQueue.Enqueue(new Guest(nameList[index], bar, mainWindow));
                Log(DateTime.Now, nameList[index] + " comes in and goes to the bar");
                if (!CouplesNight && NumberOfGuestsAtATime != 15)
                {
                    Thread.Sleep((int)(TimeForGuestToGoToBar / mainWindow.CurrentSpeed()));
                }
                bar.TotalNumberGuests++;
            }

            NumberOfGuestsAtATime = (NumberOfGuestsAtATime == 15) ? 1 : NumberOfGuestsAtATime;
        }

        private void Log(DateTime timestamp, string activity)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.GuestsListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {activity}"));
        }

        private void GoHome()
        {
            Log(DateTime.Now, "Bouncer goes home.");
        }
    }
}
