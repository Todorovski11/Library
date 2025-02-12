using Domain.Models;
using Domain.TravelApp;
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
        private readonly TravelAppDbContext travelContext;
        private DbSet<T> entities;
        private DbSet<TravelItenaries> travelItenaries;
        string errorMessage = string.Empty;

        public Repository(ApplicationDbContext context, TravelAppDbContext travelContext)
        {
            this.context = context;
            this.travelContext = travelContext;
            entities = context.Set<T>();
            travelItenaries = travelContext.Set<TravelItenaries>();
        }

        public IEnumerable<T> GetAll()
        {
            if (typeof(T) == typeof(Order))
            {
                return entities
                    .Include("BooksInOrder.Book")
                    .Include("Owner")
                    .AsEnumerable();
            }
            else if (typeof(T) == typeof(Book))
            {
                return entities
                    .Include("Author")
                    .Include("Publisher")
                    .AsEnumerable();
            }
            else if (typeof(T) == typeof(Author))
            {
                return entities
                    .Include("Books")
                    .AsEnumerable();
            }
            else if (typeof(T) == typeof(Publisher))
            {
                return entities
                    .Include("Books")
                    .AsEnumerable();
            }
            else if (typeof(T) == typeof(TravelItenaries))
            {
                return travelItenaries.AsEnumerable().Cast<T>();
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
