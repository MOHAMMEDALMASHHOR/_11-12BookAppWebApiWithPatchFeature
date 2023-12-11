using BookDemo.Models;
namespace BookDemo.Repository
{
    public static class ApplicaionContext
    { 
        public static List<Book> Books { get; set; }
        static ApplicaionContext()
        {
            Books = new List<Book>()
            {
                new Book(){Id=1,Title = "Karagoz",Price =200},
                new Book(){Id=2,Title = "akGog",Price =20},
                new Book(){Id=3,Title = "Karagfsalkfjoz",Price =400}
            };
        }
    }
}
