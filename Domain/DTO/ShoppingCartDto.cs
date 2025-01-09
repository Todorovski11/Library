using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<BookInShoppingCart> Books { get; set; }
        public decimal TotalPrice { get; set; }

        public ShoppingCartDto()
        {
            Books = new List<BookInShoppingCart>();
        }
    }
}
