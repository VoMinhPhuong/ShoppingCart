using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DataAccess.Repositories;
using ShoppingCart.DataAccess.ViewModels;

namespace ShoppingCart.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private IUnitWork _unitWork;
       
        public CategoryController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }

        public IActionResult Index()
        {
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.categories = _unitWork.Category.GetAll();
            return View(categoryVM);
        }

        [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            CategoryVM vm = new CategoryVM();
            if(id == null || id == 0)
            {
                return View(vm);
            }
            else
            {
                vm.Category = _unitWork.Category.GetT(x => x.Id == id);
                if(vm.Category == null)
                {
                    return NotFound();
                }
                else {
                    return View(vm);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(CategoryVM vm)
        {
            if (ModelState.IsValid)
            {
                if(vm.Category.Id == 0)
                {
                    _unitWork.Category.Add(vm.Category);
                    TempData["success"] = "Categpry Create Done!";
                }
                else
                {
                    _unitWork.Category.Update(vm.Category);
                    TempData["success"] = "Categpry Update Done!";
                }
                _unitWork.Save();
                return RedirectToAction("Index"); 
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var category = _unitWork.Category.GetT(x => x.Id == id);

            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public IActionResult DeleteCategory(int? id)
        {
            var category = _unitWork.Category.GetT(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _unitWork.Category.Delete(category);
            _unitWork.Save();
            TempData["success"] = "Categpry Delete Done!";
            return RedirectToAction("Index");
        }
    }
}
