using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Author : BaseEntity
    {
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<Book>? Books { get; set; }
    }
}
