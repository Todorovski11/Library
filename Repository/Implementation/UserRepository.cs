using Domain.Identity;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<BookStoreApplicationUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            context = context;
            entities = context.Set<BookStoreApplicationUser>();
            //entities = context.Set<BookStoreApplicationUser>();
        }

        public IEnumerable<BookStoreApplicationUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public BookStoreApplicationUser Get(string id)
        {
            return entities
               .Include(z => z.ShoppingCart)
               .Include("ShoppingCart.BooksInShoppingCarts")
               .Include("ShoppingCart.BooksInShoppingCarts.Book")
               .SingleOrDefault(s => s.Id == id);
        }

        public void Insert(BookStoreApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(BookStoreApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(BookStoreApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
