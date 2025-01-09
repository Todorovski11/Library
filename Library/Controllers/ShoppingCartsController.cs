using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.Interface;
using Stripe;
using System.Security.Claims;

namespace Library.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartsController(IShoppingCartService _shoppingCartService,
            IOptions<StripeSettings> stripeSettings)
        {
            this._shoppingCartService = _shoppingCartService;
        }

        // GET: ShoppingCarts
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dto = _shoppingCartService.GetShoppingCartInfo(userId);
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFromShoppingCart(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = _shoppingCartService.RemoveBookFromShoppingCart(id, userId);

            if (result)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, error = "Failed to remove the book from the shopping cart." });
        }


        private bool Order()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _shoppingCartService.ConfirmOrder(userId);
        }

        public IActionResult SuccessPayment()
        {
            return Json(new { success = true, message = "Payment was successful!" });
        }


        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            StripeConfiguration.ApiKey = "sk_test_51Io84IHBiOcGzrvu4sxX66rTHq8r5nxIxRiJPbOHB4NwVJOE1jSlxgYe741ITs024uXhtpBFtxm3RoCZc3kafocC00IhvgxkL0";

            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = _shoppingCartService.GetShoppingCartInfo(userId);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(order.TotalPrice) * 100),
                Description = "EShop Application Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                this.Order();

                // Return JSON response for AJAX
                return Json(new { success = true, message = "Payment succeeded!" });
            }
            else
            {
                // Return JSON response for AJAX
                return Json(new { success = false, message = "Payment failed." });
            }
        }
    }
}
