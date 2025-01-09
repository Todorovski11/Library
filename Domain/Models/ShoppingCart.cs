using Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ShoppingCart : BaseEntity
    {
        public string? OwnerId { get; set; }
        public BookStoreApplicationUser? Owner { get; set; }
        public virtual ICollection<BookInShoppingCart>? BooksInShoppingCarts { get; set; }
    }
}
