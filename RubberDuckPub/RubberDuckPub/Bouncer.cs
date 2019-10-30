using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Bouncer
    {
        public MainWindow mainWindow { get; set; }
        public Bar bar { get; set; }
        static readonly List<string> nameList = new List<string>
        {
            "Bob",
            "Tijana",
            "Andreea",
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
        static Random r = new Random();
        public int Seconds { get; set; }
        public int NumberOfGuestsAtATime { get; set; }
        public bool BusIsComing { get; set; }
        public int TimeForBusToArrive { get; set; } = 20;
        public bool CouplesNight { get; set; }

        public Bouncer(Bar bar, MainWindow mainWindow, int numberOfGuestsAtATime, bool busIsComing, bool couplesNight)
        {
            this.bar = bar;
            this.mainWindow = mainWindow;
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
            Seconds = r.Next(3, 11);
           
            if (BusIsComing)
            {
                Seconds *= 2;
                TimeForBusToArrive -= Seconds;
                if (TimeForBusToArrive <= 0)
                {
                    Seconds = TimeForBusToArrive + Seconds;
                    NumberOfGuestsAtATime = 15;
                    BusIsComing = false;
                }
            }

            Thread.Sleep(Seconds * 1000);

            for (int i = 0; i < NumberOfGuestsAtATime; i++)
            {
                int index = r.Next(1, nameList.Count());
                if (!bar.IsOpen) return;
                bar.guestQueue.Enqueue(new Guest(nameList[index], bar, mainWindow));
                Log(DateTime.Now, nameList[index] + " comes in and goes to the bar");
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
