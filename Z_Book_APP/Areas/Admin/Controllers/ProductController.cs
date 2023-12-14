using BookApp.Model;
using Microsoft.AspNetCore.Mvc;
using BookApp.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookApp.Utility;
using Microsoft.AspNetCore.Authorization;

namespace Z_Book_APP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUniteOfWork _uniteOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUniteOfWork uniteOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _uniteOfWork = uniteOfWork;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            List<Product> products = _uniteOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(products);
        }


        public IActionResult Create(int? id)
        {
            IEnumerable<SelectListItem> categoryList = _uniteOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            ViewBag.CategoryList = categoryList;

            if (id != null)
            {
                Product product = _uniteOfWork.Product.Get(u=> u.Id == id);
                return View(product);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj, IFormFile? file)
        {
            IEnumerable<SelectListItem> categoryList = _uniteOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            ViewBag.CategoryList = categoryList;

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file  != null) 
                { 
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\products\");

                    if(!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.ImageUrl = @"\images\products\" + fileName;
                }


                if (obj.Id != 0)
                {
                    _uniteOfWork.Product.Update(obj);                 
                }
                else
                {
                    _uniteOfWork.Product.Add(obj);
                }
                _uniteOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                Product Product = _uniteOfWork.Product.Get(u => u.Id == id);
                return View(Product);
            }
            return View();
        }

        [ActionName("Delete")]
        [HttpPost]
        public IActionResult DeleteProduct(int? id)
        {
            if (id != null)
            {
                Product Product = _uniteOfWork.Product.Get(u => u.Id == id);
                _uniteOfWork.Product.Delete(Product);
                _uniteOfWork.Save();
                return RedirectToAction("Index");
            }
            
            return NotFound();
        }

    }
}
