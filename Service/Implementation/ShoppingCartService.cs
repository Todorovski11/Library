using Domain.DTO;
using Domain.Models;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<BookInShoppingCart> _productInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<BookInOrder> _productInOrderRepository;
        private readonly IEmailService _emailService;
        private readonly IProductService _productService;

        public ShoppingCartService(
            IRepository<ShoppingCart> shoppingCartRepository,
            IRepository<BookInShoppingCart> productInShoppingCartRepository,
            IUserRepository userRepository,
            IRepository<Order> orderRepository,
            IRepository<BookInOrder> productInOrderRepository,
            IEmailService emailService,
            IProductService productService)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productInShoppingCartRepository = productInShoppingCartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _productInOrderRepository = productInOrderRepository;
            _emailService = emailService;
            _productService = productService;
        }

        public ShoppingCartDto GetShoppingCartInfo(string userId)
        {
            var user = _userRepository.Get(userId);
            var shoppingCart = user?.ShoppingCart;
            var products = shoppingCart?.BooksInShoppingCarts.ToList();

            var totalPrice = products?.Sum(p => p.Book.Price * p.Quantity) ?? 0;

            return new ShoppingCartDto
            {
                Books = products,
                TotalPrice = totalPrice
            };
        }

        public bool AddBookToShoppingCart(BookInShoppingCart model, string userId)
        {
            var user = _userRepository.Get(userId);
            var shoppingCart = user?.ShoppingCart;

            if (shoppingCart == null)
                return false;

            var existingProduct = shoppingCart.BooksInShoppingCarts
                .FirstOrDefault(p => p.BookId == model.BookId);

            if (existingProduct != null)
            {
                existingProduct.Quantity += model.Quantity;
                _productInShoppingCartRepository.Update(existingProduct);
            }
            else
            {
                var newProduct = new BookInShoppingCart
                {
                    BookId = model.BookId,
                    Quantity = model.Quantity,
                    Book = _productService.GetBookDetails(model.BookId)
                };
                shoppingCart.BooksInShoppingCarts.Add(newProduct);

            }

            _shoppingCartRepository.Update(shoppingCart);
            return true;
        }

        public bool RemoveBookFromShoppingCart(Guid bookId, string userId)
        {
            var user = _userRepository.Get(userId);
            var shoppingCart = user?.ShoppingCart;

            if (shoppingCart == null)
                return false;

            var productToRemove = shoppingCart.BooksInShoppingCarts
                .FirstOrDefault(p => p.BookId == bookId);

            if (productToRemove != null)
            {
                shoppingCart.BooksInShoppingCarts.Remove(productToRemove);
                _productInShoppingCartRepository.Delete(productToRemove);
                _shoppingCartRepository.Update(shoppingCart);
                return true;
            }

            return false;
        }

        public bool ConfirmOrder(string userId)
        {
            var user = _userRepository.Get(userId);
            var shoppingCart = user?.ShoppingCart;

            if (shoppingCart == null)
                return false;

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Owner = user
            };

            _orderRepository.Insert(order);

            var productInOrders = shoppingCart.BooksInShoppingCarts.Select(p => new BookInOrder
            {
                Id = Guid.NewGuid(),
                BookId = p.BookId,
                Book = p.Book,
                OrderId = order.Id,
                Order = order,
                Quantity = p.Quantity
            }).ToList();

            foreach (var productInOrder in productInOrders)
            {
                _productInOrderRepository.Insert(productInOrder);
            }

            shoppingCart.BooksInShoppingCarts.Clear();
            _shoppingCartRepository.Update(shoppingCart);

            var emailMessage = new EmailMessage
            {
                Subject = "Order Confirmation",
                MailTo = user.Email,
                Content = "Your order has been placed successfully. Thank you for shopping with us!"
            };

            _emailService.SendEmailAsync(emailMessage);
            return true;
        }
    }
}
