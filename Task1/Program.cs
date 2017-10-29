using System;
using System.Linq;
using System.Collections.Generic;
using ExtensionMethods;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> list = new List<int>();
            LinkedList<int> linkedList = new LinkedList<int>();

            for (int i = 1; i <= 100; ++i)
            {
                list.Add(i);
                linkedList.AddLast(i);
            }

            list.RemoveEachSecondItem();
            linkedList.RemoveEachSecondItem();

            Console.WriteLine("list {0}", list.First());
            Console.WriteLine("linkedList {0}", linkedList.First());
            Console.ReadKey();
        }
    }
}
