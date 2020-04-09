using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSHARP_N1_S4
{
    class Program
    {

        static void menu()
        {
            Console.WriteLine("The main task is to add the first odd number to each deque element");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("If you want to:");
            Console.WriteLine(" - fill the table from file - press 1");
            Console.WriteLine(" - fill the textfile from table - press 2");
            Console.WriteLine(" - fulfill the main task - press 3");
            Console.WriteLine(" - clear the table - press 4");
            Console.WriteLine(" - quit - press 0");
            Console.WriteLine();
            Console.WriteLine();
        }

        static void Main(string[] args)
        {

            hashTable hash = new hashTable();
            /*bool quit = false;
            int n = 0;
            while (!quit)
            {
                menu();
                n = Console.Read();
                switch (n)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    default:
                        break;
                }

            }*/
            hashItem item = new hashItem();
            for (int i = 0; i < 3; ++i)
            {
                item.setInfo();
                hash.Add(item);
            }

            hash.Print();
            Console.WriteLine();
            Console.WriteLine();

        }
    }
}
