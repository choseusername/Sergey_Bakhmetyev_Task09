using System;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            DynamicArray<int> dynamicArray = new DynamicArray<int>(50);
            for (int i = 0; i < 100; ++i)
                dynamicArray.Add(i);

            foreach(int x in dynamicArray)
                Console.Write("{0} ", x);

            Console.ReadKey();
        }
    }
}
