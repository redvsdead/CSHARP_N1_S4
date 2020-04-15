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
            Console.WriteLine("The main task is to search the user record by its number");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("If you want to:");
            Console.WriteLine(" - fill the table from file - press 1");
            Console.WriteLine(" - fulfill the main task - press 2");
            Console.WriteLine(" - clear the table - press 3");
            Console.WriteLine(" - remove one user record - press 4");
            Console.WriteLine(" - quit - press 0");
            Console.WriteLine();
            Console.WriteLine();
        }

        static void Main(string[] args)
        {

            hashTable hash = new hashTable();
            bool quit = false;
            bool full = false; //проверка, пустая ли таблица (можно ли в ней что-то искать)
            string c = "";
            while (!quit)
            {
                menu();
                int code;
                c = Console.ReadLine();
                switch (c)
                {
                    case "0":
                        quit = true;
                        break;
                    case "1":
                        hash.loadFrom();
                        hash.Print();
                        full = true;
                        Console.WriteLine();
                        Console.WriteLine();
                        break;
                    case "2":
                        Console.WriteLine();
                        if (full)
                        {
                            Console.WriteLine("Please, enter the number.");                            
                            if (int.TryParse(Console.ReadLine(), out code))
                            {
                                hash.Task(code);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Warning: the table is empty.");
                        }
                        Console.WriteLine();
                        Console.WriteLine();
                        break;
                    case "3":
                        hash.Clear();
                        full = false;
                        Console.WriteLine();
                        Console.WriteLine();
                        break;
                    case "4":
                        Console.WriteLine("Please, enter the number.");                        
                        hashItem item = new hashItem();
                        if (int.TryParse(Console.ReadLine(), out code))
                        {
                            if (hash.Search(code, out item))
                            {
                                hash.Delete(item);
                            }
                            else
                            {
                                Console.WriteLine("Warning: user number #" + code + " does not exist");
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Warning: cannot interpret command. Try again.");
                        Console.WriteLine();
                        Console.WriteLine();
                        break;
                }

            }
        }
    }
}
