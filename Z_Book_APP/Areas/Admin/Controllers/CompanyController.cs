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
    public class CompanyController : Controller
    {
        private readonly IUniteOfWork _uniteOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUniteOfWork uniteOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _uniteOfWork = uniteOfWork;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            List<Company> Companys = _uniteOfWork.Company.GetAll().ToList();
            return View(Companys);
        }


        public IActionResult Create(int? id)
        {

            if (id != null)
            {
                Company Company = _uniteOfWork.Company.Get(u=> u.Id == id);
                return View(Company);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Create(Company obj)
        {
            if (ModelState.IsValid)
            {                            
                if (obj.Id != 0)
                {
                    _uniteOfWork.Company.Update(obj);                 
                }
                else
                {
                    _uniteOfWork.Company.Add(obj);
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
                Company Company = _uniteOfWork.Company.Get(u => u.Id == id);
                return View(Company);
            }
            return View();
        }

        [ActionName("Delete")]
        [HttpPost]
        public IActionResult DeleteCompany(int? id)
        {
            if (id != null)
            {
                Company Company = _uniteOfWork.Company.Get(u => u.Id == id);
                _uniteOfWork.Company.Delete(Company);
                _uniteOfWork.Save();
                return RedirectToAction("Index");
            }
            
            return NotFound();
        }

    }
}
