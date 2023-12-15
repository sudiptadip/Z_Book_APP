using BookApp.DataAccess.Repository.IRepository;
using BookApp.Model;
using BookApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Z_Book_APP.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUniteOfWork _uniteOfWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUniteOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartLIst = _uniteOfWork.ShoppingCart.GetAll(u => 
                    u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            double total = 0;

            if (ShoppingCartVM.ShoppingCartLIst != null)
            {
                foreach (var item in ShoppingCartVM.ShoppingCartLIst)
                {
                    item.Price = GetPriceBasedOnQuantity(item);
                    total += GetPriceBasedOnQuantity(item) * item.Count; 
                }
            }

            ShoppingCartVM.OrderHeader.OrderTotal = total;

            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int id)
        {
            var cartFromDb = _uniteOfWork.ShoppingCart.Get(u => u.Id == id);
            cartFromDb.Count += 1;
            _uniteOfWork.ShoppingCart.Update(cartFromDb);
            _uniteOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Minus(int id)
        {
            var cartFromDb = _uniteOfWork.ShoppingCart.Get(u => u.Id == id);
            
            if(cartFromDb.Count == 1)
            {
                _uniteOfWork.ShoppingCart.Delete(cartFromDb);
                _uniteOfWork.Save();
            }
            else
            {
                cartFromDb.Count -= 1;
                _uniteOfWork.ShoppingCart.Update(cartFromDb);
                _uniteOfWork.Save();
            }                        
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var cartFromDb = _uniteOfWork.ShoppingCart.Get(u => u.Id == id);
            _uniteOfWork.ShoppingCart.Delete(cartFromDb);
            _uniteOfWork.Save();
            return RedirectToAction("Index");
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if(shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else if(shoppingCart.Count <= 100)
            {
                return shoppingCart.Product.Price50;
            }
            else
            {
                return shoppingCart.Product.Price100;
            }
        }


        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartLIst = _uniteOfWork.ShoppingCart.GetAll(u =>
                    u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _uniteOfWork.ApplicationUser.Get(u => u.Id == userId);
            
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.Postalcode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;


            double total = 0;
            if (ShoppingCartVM.ShoppingCartLIst != null)
            {
                foreach (var item in ShoppingCartVM.ShoppingCartLIst)
                {
                    item.Price = GetPriceBasedOnQuantity(item);
                    total += GetPriceBasedOnQuantity(item) * item.Count;
                }
            }

            ShoppingCartVM.OrderHeader.OrderTotal = total;


            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartLIst = _uniteOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");
            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            ShoppingCartVM.OrderHeader.ApplicationUser = _uniteOfWork.ApplicationUser.Get(u => u.Id == userId);


            double total = 0;
            if (ShoppingCartVM.ShoppingCartLIst != null)
            {
                foreach (var item in ShoppingCartVM.ShoppingCartLIst)
                {
                    item.Price = GetPriceBasedOnQuantity(item);
                    total += GetPriceBasedOnQuantity(item) * item.Count;
                }
            }

            ShoppingCartVM.OrderHeader.OrderTotal = total;

            if(ShoppingCartVM.OrderHeader.ApplicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            ShoppingCartVM.OrderHeader.ApplicationUser = null;
            _uniteOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _uniteOfWork.Save();

            foreach (var cart in ShoppingCartVM.ShoppingCartLIst)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count,
                };
                _uniteOfWork.OrderDetail.Add(orderDetail);
                _uniteOfWork.Save();
            }

            ShoppingCartVM.OrderHeader.ApplicationUser = _uniteOfWork.ApplicationUser.Get(u => u.Id == userId);

            if (ShoppingCartVM.OrderHeader.ApplicationUser.CompanyId.GetValueOrDefault() == 0)
            {

            }
            return RedirectToAction("OrderConfirmation", new {id = ShoppingCartVM.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View();
        }

    }
}
