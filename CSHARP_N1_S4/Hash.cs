using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//МЕТОД -- ДВОЙНОЕ ХЕШИРОВАНИЕ

namespace CSHARP_N1_S4
{
    public enum Status {engaged, unengaged, deleted};   //список статусов ячейки: занята, не занята, удалена

    //класс элемента 
    public class hashItem
    {
        public Status status;
        public Key number = new Key();  //ключ класса Key (таб. номер)
        public string NSP;  //фио
        public int payment; //з/п


        //при инстантинации ставим статус "не занято"
        public hashItem()
        {
            status = Status.unengaged;
        }

        //сеттеры:
        public void setEngaged()
        {
            status = Status.engaged;
        }

        public void setDeleted()
        {
            status = Status.deleted;
        }

        public void setNumber(int _number)
        {
            if (_number > 0)
            {
                number = new Key(_number);
            }
        }

        public void setNSP(string _NSP)
        {
            NSP = _NSP;
        }

        public void setPayment(int _payment)
        {
            if (_payment > 0)
            {
                payment = _payment;
            }
        }

        //геттеры:
        public Status getStatus()
        {
            return status;
        }
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
            setEngaged();    //устанавливаем статус занятой ячейки
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
            setEngaged();
            setNumber(_item.getNumber());
            setNSP(_item.getNSP());
            setPayment(_item.getPayment());
        }

        //перегрузка для чтения информации построчно из файла, передается прочтенная строка
        public void setInfo(string _line)
        {
            setEngaged();
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
                while (ok && i < words.Length - 1 && j < words[i].Length)
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
        const int size = 1000; //макс. размер табоицы
        public hashItem[] table = new hashItem[size]; //таблица hashItem'ов
        public int count;   //к-во эл. в таблице

        //конструктор
        public hashTable()
        {
            for (int i = 0; i < size; ++i)
            {
                table[i] = new hashItem();  //теперь все ячейки считаются незанятыми
            }
            count = 0;  //число занятых == 0
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
            bool ok = true;
            int key =_item.getNumber(); //запоминаем ключ (таб. номер)
            int hF = hashFunction1(key);
            if (!isFull()) //если таблица не полная, то пытаемся добавить
            {
                while (table[hF].getStatus() == Status.engaged && ok) //если по первому хешу уже что-то лежит, применяем второй, пока не дойдем до пустой ячейки или конца таблицы
                {
                    ok = table[hF].getNumber() != key;  //
                    hF = (hF + hashFunction2(key)) % (size - 2);
                }
                if (table[hF].getStatus() != Status.engaged && ok)    //если статуc ячейки == не занята или удалена, то добавляем элемент
                {
                    table[hF].setEngaged();
                    table[hF].setInfo(_item);
                    ++count;
                }
                else
                {
                    Console.WriteLine("Warning: user numbers can not be equal.");
                }
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
                    //если первый хеш не подошел (ячейка пуста/не совпал номер), применяем второй (пока не найдем нужную или конец таблицы)
                    while (table[hF].getStatus() != Status.engaged || table[hF].getNumber() != key)                      
                    {
                        hF = (hF + hashFunction2(key)) % (size - 2);
                    }
                    //если в ячейке что-то есть и таб. номера совпадают, то удаляем ее содержимое
                    if (table[hF].getStatus() == Status.engaged && table[hF].getNumber() == _item.getNumber())    
                    {
                        table[hF].setDeleted();  //меняем статус на "удалена"
                        //table[hF] = null;  //уничтожаем ячейку
                        --count;
                        Console.WriteLine("User record was removed successfully.");
                    }
                }
                //иначе выводим предупреждение
                else
                {
                    Console.WriteLine("Warning: unable to remove user record. The table is empty.");
                }
            }
        }

        //очистка таблицы
        public void Clear()
        {
            int i = 0;  //счетчик удаляемых данных
            while (i < size && count > 0)   //пока не дошли до конца таблицы и не удалили все элементы
            {
                if (table[i].getStatus() == Status.engaged)   //если ячейка не пустая, удаляем данные
                {
                    table[i].setDeleted();
                    --count;
                }
                ++i;
            }
            Console.WriteLine("The table is empty now");
        }

        //проверка наличия элемента. был вариант возвращать элемент, но я решила передавать его как out аргумент
        public bool Search(int _key, out hashItem _item)
        {
            int hF = hashFunction1(_key);
            int count = 1;  //счетчик ячеек, которые посетили. тк hF делят на size-2, то повторяться ячейки не будут
            if (!isEmpty())
            {
                while ((table[hF].getStatus() != Status.engaged || table[hF].getNumber() != _key) && count < size) //если первый хеш не подошел, применяем второй
                {
                    hF = (hF + hashFunction2(_key))%(size-2);
                    ++count;    //увеличиваем count, чтобы отследить момент, когда все ячейки будут посещены, если по введенному номеру нет элемента
                }
                if (table[hF].getStatus() == Status.engaged) //если ячейка, на которой остановились, заполнена
                {
                    //проверяем соответствие табельных номеров
                    switch (table[hF].getNumber() == _key)
                    {
                        case true:
                            _item = new hashItem();                            
                            _item = table[hF];
                            return true;
                        default:
                            _item = null;                            
                            return false;
                    };
                }
                else
                {
                    _item = null;
                    return false;
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
                    if (table[i].getStatus() == Status.engaged)   //если ячейка не пустая, выводим информацию
                    {
                        ++j;    //увеличение к-ва выведенных данных
                        line = table[i].getInfo();
                        Console.WriteLine(line);
                    }
                    ++i;    //переход к следующей ячейке
                }
            }
        }

        public void Task (int _number)
        {
            hashItem item;
            if (_number < 1)
            {
                Console.WriteLine("Warning: the number must be positive");
            }
            else
            {
                item = new hashItem();
                if (Search(_number, out item))
                {
                    Console.WriteLine(item.getInfo());
                }
                else
                {
                    Console.WriteLine("Warning: cannot find the user #" + _number);
                }
            }
        }


        //загрузка из файла
        public void loadFrom()
        {
            string line;
            hashItem item = new hashItem();
            try
            {
                //каждую строку файла передаем в соответствующий метод set для строки
                using (StreamReader sr = new StreamReader("TestFile.txt"))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        item.setInfo(line);
                        Add(item);
                    }
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
