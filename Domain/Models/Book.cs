using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Book : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? CoverImage { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public double Rating { get; set; }
        public virtual ICollection<BookInShoppingCart>? BooksInShoppingCarts { get; set; }
        public virtual IEnumerable<BookInOrder>? BooksInOrder { get; set; }
        [Required]
        public Guid AuthorId { get; set; }

        public virtual Author? Author { get; set; }

        [Required]
        public Guid PublisherId { get; set; }

        public virtual Publisher? Publisher { get; set; }
    }
}
