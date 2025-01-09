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
    public class AuthorService : IAuthorService
    {
        private readonly IRepository<Author> _authorRepository;

        public AuthorService(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            return _authorRepository.GetAll();
        }

        public Author GetAuthorDetails(Guid authorId)
        {
            return _authorRepository.Get(authorId);
        }

        public bool AddAuthor(Author author)
        {
            try
            {
                _authorRepository.Insert(author);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateAuthor(Author author)
        {
            try
            {
                _authorRepository.Update(author);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteAuthor(Guid authorId)
        {
            try
            {
                var author = _authorRepository.Get(authorId);
                if (author != null)
                {
                    _authorRepository.Delete(author);
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
