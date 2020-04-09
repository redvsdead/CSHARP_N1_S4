﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//МЕТОД -- ДВОЙНОЕ ХЕШИРОВАНИЕ

namespace CSHARP_N1_S4
{

    //класс элемента 
    public class hashItem
    {

        public Key number;  //ключ класса Key (таб. номер)
        public string NSP;  //фио
        public int payment; //з/п

        //сеттеры:
        public void setNumber(int _number)
        {
            number = new Key(_number);
        }

        public void setNSP(string _NSP)
        {
            NSP = _NSP;
        }

        public void setPayment(int _payment)
        {
            payment = _payment;
        }

        //геттеры:
        public int getNumber()
        {
            return number.getKey();
        }

        public string getNSP()
        {
            return NSP;
        }

        public int getPayment()
        {
            return payment;
        }

        //вывод всей информации в строку (для записи в файл)
        public string getInfo()
        {
            string info = getNumber().ToString() + " " + getNSP() + " " + getPayment().ToString();
            return info;
        }

        //заполнение элемента в консоли
        public void setInfo()
        {
            int n = 0;
            Console.WriteLine("Add personnel number:");
            n = Convert.ToInt32(Console.ReadLine());
            setNumber(n);
            Console.WriteLine("Add name and surname (+ patronym):");
            string line = Console.ReadLine();
            setNSP(line);
            Console.WriteLine("Add payment:");
            n = Convert.ToInt32(Console.ReadLine());
            setPayment(n);
        }

        //перегрузка, копирование другого hashItem'а
        public void setInfo(hashItem _item)
        {
            setNumber(_item.getNumber());
            setNSP(_item.getNSP());
            setPayment(_item.getPayment());
        }

        //перегрузка для чтения информации построчно из файла, передается прочтенная строка
        public void setInfo(string _line)
        {
            int num = 0;
            string nsp = "";
            int pay = 0;
            string add = "";
            string[] words = _line.Split(' ');
            //если в строке не больше 5 элементов (таб. номер, фамилия, имя, (отчество), з/п, то пытаемся обработать
            //если 1-й и последний элементы можно ковертировать в номер и з/п (числа), то пытаемся выделить фио/фи
            if (words.Length < 6 && Int32.TryParse(words[0], out num) && Int32.TryParse(words[words.Length - 1], out pay))
            {
                bool ok = true;
                int i = 1;
                int j = 0;
                //если они состоят из букв, то добавляем их в промежуточную add
                while (ok && i < words.Length - 2 && j < words[i].Length)
                {
                    ok = !(words[i][j] < 'A') && !(words[i][j] > 'Z') || !(words[i][j] < 'a') && !(words[i][j] > 'z');
                    add += words[i][j];
                    ++j;
                    //если дошли до конца слова, переходим к следующему и добавляем пробел к add между словами
                    if (j == words[i].Length) {
                        ++i;
                        j = 0;
                        add += " ";
                    }
                   
                }
                //если лишних символов не встретилось, то создаем элемент
                if (ok)
                {
                    nsp += " " + add;
                    setNumber(num);
                    setNSP(nsp);
                    setPayment(pay);
                }
                //иначе предупреждаем о наличии неразрешенных символов
                else
                {
                    Console.WriteLine("Warning: info contains impermissible cgaracters.");
                }
            }
        }

    }


    //////////////////////////////////////////////////////////////////////////////////////////


    //класс хеш таблицы
    public class hashTable
    {
        const int size = 100; //макс. размер табоицы
        public hashItem[] table = new hashItem[size]; //таблица hashItem'ов
        public int count;   //к-во эл. в таблице

        //конструктор
        public hashTable()
        {
            count = 0;
        }

        //первая хеш функция
        public int hashFunction1(int _key)
        {
            Key key = new Key(_key);
            return key.hashFunc1(size);
        }

        //вторая хеш функция
        public int hashFunction2(int _key)
        {
            Key key = new Key(_key);
            return key.hashFunc2(size);
        }

        //проверки на пустоту/заполненность таблицы
        public bool isEmpty()
        {
            return count == 0;
        }

        public bool isFull()
        {
            return count == size;
        }

        //добавление элемента
        public void Add(hashItem _item)
        {
            int key =_item.getNumber(); //запоминаем ключ (таб. номер)
            int hF = hashFunction1(key);
            if (!isFull()) //если таблица не полная, то пытаемся добавить
            {
                if (table[hF] != null) //если по первому хешу уже что-то лежит, применяем второй
                {
                    hF = hashFunction2(key);
                }
                table[hF] = new hashItem(); //выделяем память под ячейку и присваиваем ей переаваемый элемент:
                table[hF].setInfo(_item);
                ++count;
                Console.WriteLine("User record was added successfully.");
            }
            //иначе оповещаем юзера о невозможности добавления
            else
            {
                Console.WriteLine("Warning: unable to add user record. The table is full.");
            }
        }

        //удаление элемента
        public void Delete(hashItem _item)
        {
            if (Search(_item.getNumber(), out _item)) //если такой элемент в таблице существует, то применяем первый хеш
            {
                int key = _item.getNumber();
                int hF = hashFunction1(key);
                if (!isEmpty())
                {
                    if (table[hF] == null || table[hF].getNumber() != key)  //если первый хеш не подошел (ячейка пуста/не совпал номер), применяем второй
                    {
                        hF = hashFunction2(key);
                    }
                    table[hF] = null;  //уничтожаем ячейку
                    --count;
                    Console.WriteLine("User record was removed successfully.");
                }
                //иначе выводим предупреждение
                else
                {
                    Console.WriteLine("Warning: unable to remove user record. The table is empty.");
                }
            }
        }

        //проверка наличия элемента. был вариант возвращать элемент, но я решила передавать его как out аргумент
        public bool Search(int _key, out hashItem _item)
        {
            int hF = hashFunction1(_key);
            if (!isEmpty())
            {
                if (table[hF] == null || table[hF].getNumber() != _key) //если первый хеш не подошел, применяем второй
                {
                    hF = hashFunction2(_key);
                }
                if (table[hF] != null) //если ячейка существует
                {
                    //проверяем соответствие табельных номеров
                    switch (table[hF].getNumber() == _key)
                    {
                        case true:
                            _item = new hashItem();
                            _item = table[hF];
                            Console.WriteLine("User record has been found.");
                            return true;
                        default:
                            _item = null;
                            Console.WriteLine("Warning: Cannot find the user record.");
                            return false;
                    }
                }
            }
            _item = null;
            Console.WriteLine("Warning: the table is empty.");
            return false;
        }

        //вывод таблицы
        public void Print()
        {
            //если таблица не пустая, то выводим
            if (!isEmpty())
            {
                string line;
                int i = 0; 
                int j = 0;  //счетчик выведенных данных
                while (i < size && j < count)   //пока не дошли до конца таблицы и не вывели все элементы (т е счетчик j < count)
                {
                    if (table[i] != null)   //если ячейка не пустая, выводим информацию
                    {
                        ++j;    //увеличение к-ва выведенных данных
                        Console.Write(j + ". ");
                        line = table[i].getInfo();
                        Console.WriteLine(line);
                    }
                    ++i;    //переход к следующей ячейке
                }
            }
        }


        //загрузка из файла
        public void loadFrom()
        {
            hashItem item = new hashItem();
            try
            {
                //каждую строку файла передаем в соответствующий метод set для строки
                using (StreamReader sr = new StreamReader("TestFile.txt"))
                {
                    string line = sr.ReadToEnd();
                    item.setInfo(line);
                    Add(item);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Warning: unable to read the file.");
                Console.WriteLine(e.Message);
            }
        }
    }
}