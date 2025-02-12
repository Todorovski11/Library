using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.TravelApp
{
    public class TravelItenaries : BaseEntity

    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? DayNumber { get; set; }
        public string Place { get; set; }
        public string Image { get; set; }
        public DateTime? ArivalTime { get; set; }
        public DateTime? DepartutreTime { get; set; }

    }
}
