using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*Класс - однонаправленный список List.Дополнительно
перегрузить следующие операции:
! – инверсия элементов; 
+-объединить два списка; 
= = -проверка на равенство; 
< - добавление одного списка к другому. 
Методы расширения:
1) Усечение строки до заданной длины
2) Сумма элементов списка */

namespace lab4
{
    public class MyList<T> : IEnumerable<T> where T : class, new()
    {
        List<T> _mass;


        public MyList(string name, int id)
        {
            _mass = new List<T>();
            owner = new Owner(name, id);
            date = DateTime.Now; //дата создания
        }


        public MyList(Owner own)
        {
            _mass = new List<T>();
            owner = own;
            date = DateTime.Now;
        }

        public DateTime date { private set; get; } //дата создания
        public void Add(T data)
        {
            _mass.Add(data);
        }
        public Owner owner { get; set; }

        public T Sum() //сумма
        {
            dynamic result = null;
            foreach (var el in _mass)
            {
                if (result == null)
                {
                    result = el;
                    continue;
                }

                result += el;
            }

            return result;
        }

        public static MyList<T> operator +(MyList<T> c1, MyList<T> c2) //объединение
        {
            MyList<T> res = new MyList<T>(c1.owner);
            foreach (var el in c1)
                res.Add(el);
            foreach (var el in c2)
                res.Add(el);

            return res;
        }

        public static MyList<T> operator !(MyList<T> c1) //инверсия
        {
            MyList<T> res = new MyList<T>(c1.owner);
            foreach (dynamic el in c1)
                res.Add(!el);

            return c1;
        }

        public static bool operator <(MyList<T> c1, MyList<T> c2) //добавление одного списка к другому
        {
            //MyList<T> res = new MyList<T>(c1.owner);
            foreach (dynamic el in c2)
                c1.Add(el);

            return true;
        }
        public static bool operator >(MyList<T> c2, MyList<T> c1)
        {
            MyList<T> res = new MyList<T>(c2.owner);
            foreach (dynamic el in c1)
                res.Add(!el);

            return true;
        }

        public static bool operator !=(MyList<T> c1, MyList<T> c2)
        {
            IEnumerator<T> e1 = c1.GetEnumerator();
            IEnumerator<T> e2 = c2.GetEnumerator();
            bool b1 = true;
            bool b2 = true;


            while (b1 && b2)
            {
                if (e1.Current != e2.Current)
                    return true;
            }

            if (b1 != b2)
                return true;

            return false;
        }

        public static bool operator ==(MyList<T> c1, MyList<T> c2) //проверка на равество
        {
            return !(c1 != c2);
        }


        public IEnumerator<T> GetEnumerator() => new Enumerator<T>(_mass.ToArray());

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();


    }


    public class Enumerator<T> : IEnumerator<T>
    {
        T[] _mass;
        int position;
        public Enumerator(T[] mass)
        {
            _mass = mass;
            position = -1;
        }

        public object Current
        {
            get
            {
                if (position == -1 || position >= _mass.Length)
                    throw new InvalidOperationException();
                return _mass[position];
            }
        }

        T IEnumerator<T>.Current => (T)Current;
        //T IEnumerator<T>.Current() { return (T)Current; }

        public bool MoveNext()
        {
            if (position < _mass.Length - 1)
            {
                position++;
                return true;
            }
            else
                return false;
        }

        public void Reset()
        {
            position = -1;
        }
        public void Dispose() { }
    }
    public class Owner
    {
        public string name { private set; get; }
        public int id { private set; get; }

        public Owner(string _name, int _id)
        {
            name = _name;
            id = _id;
        }
        public Owner()
        {

        }
    }

    public static class StaticOperations //усечение строки до заданной длины
    {
        public static string Cut(this string l, int i)
        {
            return l.Remove(i);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string s = "dasadasd";
            Console.WriteLine(s.Cut(3)); //усечение

            MyList<Owner> l = new MyList<Owner>("name", 1);
            MyList<Owner> l2 = new MyList<Owner>("name", 1);

            l.Add(new Owner("name1", 1));
            l.Add(new Owner("name2", 1));
            l2.Add(new Owner("name3", 1));
            l2.Add(new Owner("name4", 1));

            if (l < l2)
                foreach (var t in l)
                    Console.WriteLine(t.name);

            Console.ReadKey();
        }
    }
}

