using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Section01 {
    class Program {

        static void Main(string[] args) {
            var list = new List<string> {
               "Tokyo", "New Delhi", "Bangkok", "London", "Paris", "Berlin", "Canberra", "Hong Kong",
            };

            var names = list.FindAll(s => s.Length <= 5);
            names.ForEach(s => Console.WriteLine(s));

            //foreach(var s in names)
            //Console.WriteLine(s);
            #region
            /*var numbers = new[] { 5, 3, 9, 6, 7, 5, 8, 1, 0, 5, 10, 4,};

            int count = Count(numbers, n => n.ToString().Contains('1'));//ラムダ式を使った処理

            Console.WriteLine("個数:" + count);
        }

        public static int Count(int[] numbers, Predicate<int> judge) {
            int count = 0;
            foreach (var n in numbers) {
                if (judge(n) == true) { 
                    count++;
                }
            }
            return count;
            */
            #endregion
        }
    }
}
