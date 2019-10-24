using System;
using System.Collections.Generic;
using System.Linq;

namespace RubberDuckPub
{
    public class Bouncer
    {
        static List<string> nameList = new List<string> { "Bob", "Tijana", "Andreea", "Kalle", "Sam", "Linda", "Ellen", "Suzy", "Jane" };
        static Random r = new Random();
        int listIndex = r.Next(1, nameList.Count());

        public Bouncer(Bar bar, MainWindow mainWindow)
        {
        }

        public Guest GenerateGuest(int index)
        {
            return new Guest(nameList[index]);
        }



    }
}
