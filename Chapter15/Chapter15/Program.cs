using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter15 {
    class Program {
        static void Main(string[] args) {

            var books = Library.Books
                .OrderBy(b => b.CategoryId)
                .ThenByDescending(b => b.PublishedYear);

            foreach(var book in books) 
            {
                Console.WriteLine(books);
            }
        }
    }
}
