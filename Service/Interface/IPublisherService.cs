using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IPublisherService
    {
        IEnumerable<Publisher> GetAllPublishers();
        Publisher GetPublisherDetails(Guid publisherId);
        bool AddPublisher(Publisher publisher);
        bool UpdatePublisher(Publisher publisher);
        bool DeletePublisher(Guid publisherId);
    }
}
