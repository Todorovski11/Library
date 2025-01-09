using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IProductService
    {
        IEnumerable<Book> GetAllBooks();
        Book GetBookDetails(Guid productId);
        bool AddBook(Book product);
        bool UpdateBook(Book product);
        bool DeleteBook(Guid productId);
    }
}
