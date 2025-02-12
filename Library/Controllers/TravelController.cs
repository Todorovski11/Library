using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.Implementation;
using Service.Interface;

namespace Library.Controllers
{
    public class TravelController : Controller
    {
        private readonly ITravelService _travelService;
        public TravelController(ITravelService travelService)
        {
           _travelService = travelService;
        }
        public IActionResult Index()
        {
            return View(_travelService.GetAllTravelItenaries());
        }
    }
}
