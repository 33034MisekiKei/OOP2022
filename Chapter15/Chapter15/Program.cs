using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter15 {
    class Program {
        static void Main(string[] args) {

            var years = new List<int>();
            int year;

            Console.WriteLine("出力したい西暦を入力（終了：-1）");
            year = int.Parse(Console.ReadLine());
            while (year != -1) {
                years.Add(year);
                year = int.Parse(Console.ReadLine());
            }

            int sort;

            Console.WriteLine();
            Console.Write("昇順：1 降順：2 ：");
            sort = int.Parse(Console.ReadLine());

            IEnumerable<Book> books;
            if (sort == 1) 
            { //昇順
                books = Library.Books
                    .Where(b => years.Contains(b.PublishedYear))
                    .OrderBy(b => b.PublishedYear);
            }
            else 
            { //降順
                books = Library.Books
                    .Where(b => years.Contains(b.PublishedYear))
                    .OrderByDescending(b => b.PublishedYear);
            }

            //var years = new int[] { 2013, 2016 };
            foreach (var book in books) 
           　{
                Console.WriteLine(book);
            }

            Console.WriteLine();
            //var groups = Library.Books
            //    .Where(b => years.Contains(b.PublishedYear))
            //    .GroupBy(b => b.PublishedYear)
            //    .OrderBy(g => g.Key);

            var selected = Library.Books
                .OrderBy(b => b.Category)
                .ThenBy(b => b.PublishedYear)
                .Join(Library.Categories        //結合する2番目のシーケンス
                    ,book => book.Category      //対象シーケンスの結合キー
                    ,category => category.Id    //2番目のシーケンスの結合キー
                    ,(book, category) => new {
                        Title = book.Title
                        ,Category = category.Name
                        ,PublishedYear = book.PublishedYear});

            foreach (var book in books) 
            {
                //Console.WriteLine($"{book.PublishedYear}年");
                //var category = Library.Categories.Where(b => b.Id == book.CategoryId).First();
                Console.WriteLine($"{book.Title} {book.Category} {book.PublishedYear}");
            }
        }
    }
}
