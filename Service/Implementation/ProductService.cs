using Domain.Models;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Book> _bookRepository;

        public ProductService(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepository.GetAll();
        }

        public Book GetBookDetails(Guid bookId)
        {
            return _bookRepository.Get(bookId);
        }

        public bool AddBook(Book book)
        {
            try
            {
                _bookRepository.Insert(book);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateBook(Book book)
        {
            try
            {
                _bookRepository.Update(book);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteBook(Guid bookId)
        {
            try
            {
                var book = _bookRepository.Get(bookId);
                if (book != null)
                {
                    _bookRepository.Delete(book);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
