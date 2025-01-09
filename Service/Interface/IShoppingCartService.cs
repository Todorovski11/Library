using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto GetShoppingCartInfo(string userId);
        bool AddBookToShoppingCart(BookInShoppingCart model, string userId);
        bool RemoveBookFromShoppingCart(Guid bookId, string userId);
        bool ConfirmOrder(string userId);
    }
}
