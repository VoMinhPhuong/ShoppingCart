using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingCart.DataAccess.Repositories;
using ShoppingCart.DataAccess.ViewModels;

namespace ShoppingCart.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IUnitWork _unitWork;
        private IWebHostEnvironment _hostingEnvironment;
        public ProductController(IUnitWork unitWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitWork = unitWork;
            _hostingEnvironment = hostingEnvironment;
        }

        #region API CALL
        public IActionResult AllProducts()
        {
            var products = _unitWork.Product.GetAll(includeProperties: "Category");
            return Json(new {data = products});
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            ProductVM vm = new ProductVM()
            {
                Product = new(),
                Categories = _unitWork.Category.GetAll().Select(x =>
                   new SelectListItem()
                   {
                       Text = x.Name,
                       Value = x.Id.ToString()
                   })
            };

            if(id == null || id == 0)
            {
                return View(vm);
            }
            else
            {
                vm.Product = _unitWork.Product.GetT(x => x.Id == id);
                
                if(vm.Product == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(vm);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpadte(ProductVM vm, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                string fileName = string.Empty;
                if (file != null)
                {
                    string upDir = Path.Combine(_hostingEnvironment.WebRootPath, "ProductImage");
                    fileName = Guid.NewGuid().ToString()+"-"+ file.FileName;
                    string path = Path.Combine(upDir, fileName);

                    if (vm.Product.ImageUrl != null)
                    {
                        var oldImage = Path.Combine(_hostingEnvironment.WebRootPath, 
                            vm.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    vm.Product.ImageUrl = @"\ProductImage\" + fileName;
                }

                if (vm.Product.Id == 0)
                {
                    _unitWork.Product.Add(vm.Product);
                    TempData["success"] = "Product Create Done !";
                }
                else
                {
                    _unitWork.Product.Update(vm.Product);
                    TempData["success"] = "Product Update Done !";

                }
                _unitWork.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        #region DeleteAPICall
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product = _unitWork.Product.GetT(x=> x.Id == id);
            if(product == null)
            {
                return Json(new { success = false, message = "Error" });
            }
            else
            {
                var oldImage = Path.Combine(_hostingEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImage))
                {
                    System.IO.File.Delete(oldImage);
                }
                _unitWork.Product.Delete(product);
                _unitWork.Save();
                return Json(new { success = true, messages = "Delete" });
            }
        }
        #endregion  
    }
}
