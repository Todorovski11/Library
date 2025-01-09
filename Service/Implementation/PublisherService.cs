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
    public class PublisherService : IPublisherService
    {
        private readonly IRepository<Publisher> _publisherRepository;

        public PublisherService(IRepository<Publisher> publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        public IEnumerable<Publisher> GetAllPublishers()
        {
            return _publisherRepository.GetAll();
        }

        public Publisher GetPublisherDetails(Guid publisherId)
        {
            return _publisherRepository.Get(publisherId);
        }

        public bool AddPublisher(Publisher publisher)
        {
            try
            {
                _publisherRepository.Insert(publisher);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdatePublisher(Publisher publisher)
        {
            try
            {
                _publisherRepository.Update(publisher);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeletePublisher(Guid publisherId)
        {
            try
            {
                var publisher = _publisherRepository.Get(publisherId);
                if (publisher != null)
                {
                    _publisherRepository.Delete(publisher);
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
