using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Bouncer
    {
        static List<string> nameList = new List<string>
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
        public int seconds { get; set; }
        public Bouncer(Bar bar, MainWindow mainWindow)
        {
            Task.Run(() =>
            {
                while (bar.IsOpen)
                {
                    // while loop how many guests are coming inside the bar
                    GenerateGuest(bar, mainWindow);
                    //bar.IsOpen = false; quick check if the bouncer is going home
                }
                GoHome(mainWindow);
            });
        }

        public void GenerateGuest(Bar bar, MainWindow mainWindow)
        {
            int index = r.Next(1, nameList.Count());
            seconds = r.Next(3, 11); // changed this to be able to follow the actions
            Thread.Sleep(seconds * 1000);
            if (!bar.IsOpen) return;
            bar.guestQueue.Enqueue(new Guest(nameList[index], bar, mainWindow));
            Log(DateTime.Now, nameList[index] + " comes in and goes to the bar", mainWindow);
            bar.TotalNumberGuests++;
            mainWindow.Dispatcher.Invoke(() => bar.BarContentInfo(mainWindow, bar.cleanGlassesStack.Count, bar.emptyChairs.Count));
            Thread.Sleep(1000); // time to go to the bar
        }

        private void Log(DateTime timestamp, string activity, MainWindow mainWindow)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.GuestsListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {activity}"));
        }

        private void GoHome(MainWindow mainWindow)
        {
            Log(DateTime.Now, "Bouncer goes home.", mainWindow);
        }
    }
}
