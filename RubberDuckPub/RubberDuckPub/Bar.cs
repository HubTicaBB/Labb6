﻿using System;
using System.Collections.Concurrent;

namespace RubberDuckPub
{
    public class Bar
    {
        static ConcurrentStack<int> cleanGlassesStack = new ConcurrentStack<int>();
        static ConcurrentStack<int> dirtyGlassesStack = new ConcurrentStack<int>();
        static ConcurrentStack<int> emptyChairs = new ConcurrentStack<int>();
        static ConcurrentQueue<Guest> guestQueue = new ConcurrentQueue<Guest>();
        static int numberOfGlasses = 8;
        static int numberOfChairs = 9;
        static int timeOpenBar = 120;

        Action pushGlasses = () =>
        {
            for (int i = 0; i < numberOfGlasses; i++)
            {
                cleanGlassesStack.Push(i);

            }
        };

        Action pushChairs = () =>
         {
             for (int i = 0; i < numberOfChairs; i++)
             {
                 emptyChairs.Push(i);
             }
         };



    }
}