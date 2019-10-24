﻿using System;
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

        public Bouncer(Bar bar, MainWindow mainWindow)
        {
            Task.Run(() =>
            {
                while (bar.IsOpen)
                {
                    GenerateGuest(bar, mainWindow);
                }
                GoHome(mainWindow);
            });
        }

        public void GenerateGuest(Bar bar, MainWindow mainWindow)
        {
            int index = r.Next(1, nameList.Count());
            int seconds = r.Next(3, 11);
            Thread.Sleep(seconds * 1000);
            bar.guestQueue.Enqueue(new Guest(nameList[index]));
            Log(DateTime.Now, nameList[index], mainWindow);
        }

        private void Log(DateTime timestamp, string name, MainWindow mainWindow)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.GuestsListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - {name} comes in and goes to the bar"));
        }

        private void Log(DateTime timestamp, MainWindow mainWindow)
        {
            mainWindow.Dispatcher.Invoke(() => mainWindow.GuestsListBox.Items.Insert(0, $"{timestamp.ToString("H:mm:ss")} - Bouncer goes home"));
        }

        private void GoHome(MainWindow mainWindow)
        {
            Log(DateTime.Now, mainWindow);
        }
    }
}
