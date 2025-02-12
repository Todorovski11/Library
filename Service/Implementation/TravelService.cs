using Domain.Models;
using Domain.TravelApp;
using Repository.Implementation;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class TravelService : ITravelService
    {
        private readonly IRepository<TravelItenaries> _travelItenariesRepository;

        public TravelService(IRepository<TravelItenaries> travelItenariesRepository)
        {
            _travelItenariesRepository = travelItenariesRepository;

        }
        public IEnumerable<TravelItenaries> GetAllTravelItenaries()
        {
            return _travelItenariesRepository.GetAll();
        }
    }
}
