using BookApp.Model;
using Microsoft.AspNetCore.Mvc;
using BookApp.DataAccess.Repository.IRepository;

namespace Z_Book_APP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUniteOfWork _uniteOfWork;
        public CategoryController(IUniteOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }
        public IActionResult Index()
        {
            List<Category> categories = _uniteOfWork.Category.GetAll().ToList();
            return View(categories);
        }
        public IActionResult Create(int? id)
        {
            if(id != null)
            {
                Category category = _uniteOfWork.Category.Get(u=> u.Id == id);
                return View(category);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id != 0)
                {
                    _uniteOfWork.Category.Update(obj);
                    return RedirectToAction("Index");
                }
                else
                {
                    _uniteOfWork.Category.Add(obj);
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
                Category category = _uniteOfWork.Category.Get(u => u.Id == id);
                return View(category);
            }
            return View();
        }

        [ActionName("Delete")]
        [HttpPost]
        public IActionResult DeleteCategory(int? id)
        {
            if (id != null)
            {
                Category category = _uniteOfWork.Category.Get(u => u.Id == id);
                _uniteOfWork.Category.Delete(category);
                _uniteOfWork.Save();
                return RedirectToAction("Index");
            }
            
            return NotFound();
        }

    }
}
