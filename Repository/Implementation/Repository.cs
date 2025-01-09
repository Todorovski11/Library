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
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
            //entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            if (typeof(T) == typeof(Order))
            {
                var query = entities
                    .Include("BooksInOrder.Book")
                    .Include("Owner")
                    .AsQueryable();

                return query.AsEnumerable();
            }
            else if (typeof(T) == typeof(Book))
            {
                var query = entities
                    .Include("Author")
                    .Include("Publisher")
                    .AsQueryable();

                return query.AsEnumerable();
            }
            else if (typeof(T) == typeof(Author))
            {
                var query = entities
                    .Include("Books")
                    .AsQueryable();

                return query.AsEnumerable();
            }
            else if (typeof(T) == typeof(Publisher))
            {
                var query = entities
                    .Include("Books")
                    .AsQueryable();

                return query.AsEnumerable();
            }
            else
            {
                return entities.AsEnumerable();
            }
        }


        public T Get(Guid? id)
        {
            // Handle specific entity types with their related data
            if (typeof(T) == typeof(Order))
            {
                return entities
                    .Include("BooksInOrder.Book")
                    .Include("Owner")
                    .SingleOrDefault(e => e.Id == id);
            }
            else if (typeof(T) == typeof(Book))
            {
                return entities
                    .Include("Author")
                    .Include("Publisher")
                    .SingleOrDefault(e => e.Id == id);
            }
            else if (typeof(T) == typeof(Author))
            {
                return entities
                    .Include("Books")
                    .SingleOrDefault(e => e.Id == id);
            }
            else if (typeof(T) == typeof(Publisher))
            {
                return entities
                    .Include("Books")
                    .SingleOrDefault(e => e.Id == id);
            }
            else
            {
                return entities.SingleOrDefault(e => e.Id == id);
            }
        }


        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(T entity)
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
