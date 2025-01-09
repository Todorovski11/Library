using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Publisher : BaseEntity
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public virtual ICollection<Book>? Books { get; set; }
    }
}
