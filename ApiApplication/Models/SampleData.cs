using System.Linq;

namespace ApiApplication.Models
{
   public static class SampleData
    {
        
    public static void Initialize(RepositoryContext context)//на будующее делать через RepositoryManager
        {
            Company apple = new Company {  Name = "Apple", Country = "США" };
            Company microsoft = new Company { Name = "Samsung", Country = "Республика Корея" };
            Company google = new Company { Name = "Google", Country = "США" };
            if (!context.Companies.Any())
            {
                context.Companies.AddRange(apple,microsoft,google);
                context.SaveChanges();
            }
            if (!context.Phones.Any())
            {
                context.Phones.AddRange(
                    new Phone
                    {
                        Name = "iPhone X",
                        Company = apple,
                        Price = 600
                    },
                    new Phone
                    {
                        Name = "Samsung Galaxy Edge",
                        Company = microsoft,
                        Price = 550
                    },
                    new Phone
                    {
                        Name = "Pixel 5",
                        Company = google,
                        Price = 550
                    },
                     new Phone
                     {
                         Name = "iPhone XS",
                         Company = apple,
                         Price = 1000 
                     },
                    new Phone
                    {
                        Name = "Samsung Galaxy Edge 5 Note",
                        Company = microsoft,
                        Price = 600
                    },
                    new Phone
                    {
                        Name = "Pixel 3",
                        Company = google,
                        Price = 500
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
