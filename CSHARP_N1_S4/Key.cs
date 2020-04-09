using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//класс ключа

namespace CSHARP_N1_S4
{
    public class Key
    {
        private int key;
        
        public Key()    //конструктор (пустой)
        {
            key = 0;
        }

        public Key(int _key)    //конструктор (передается табельный номер), нужен для set'а
        {
            key = _key;
        }

        public int getKey()
        {
            return key;
        }

        public int hashFunc1(int _size)  //1-я х. функция (сумма цифр), ненадежная функция
        {
            Console.WriteLine("I am in hash1");
            int nKey = key;
            int nHash = 0;
            while (nKey > 0)
            {
                nHash += nKey % 10; //складываем цифры ключа
                nKey /= 10;
            }
            return nHash;
        }

        public int hashFunc2(int _size)  //2-я х. функция (середина квадрата), дополнительная
        {
            int nHash = key;
            int hash = 2139062143;
            nHash += 37 * hash;
            nHash %= _size - 2;
            return nHash;
        }

        //перегрузка == и != для сравнения объекта Key и введенного с клавиатуры int таб. номера, 
        //чтобы сто раз не вызывать геттер
        public static bool operator ==(Key _key, int _key2)
        {
            bool ok = _key.getKey() == _key2;
            return ok;
        }

        public static bool operator !=(Key _key, int _key2)
        {
            bool ok = _key.getKey() != _key2;
            return ok;
        }

        /*public static bool operator ==(Key _key, Key _key2)
        {
            bool ok = _key.getKey() == _key2.getKey();
            return ok;
        }

        public static bool operator !=(Key _key, Key _key2)
        {
            bool ok = _key.getKey() == _key2.getKey();
            return ok;
        }*/
    }
}
