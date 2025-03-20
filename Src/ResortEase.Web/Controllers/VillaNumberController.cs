using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResortEase.Domain.Entities;
using ResortEase.Infrastructure.Data;
using ResortEase.Web.ViewModels;

namespace ResortEase.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var VillaNumbers = _db.VillaNumbers.ToList();
            return View(VillaNumbers);
        }


        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumber obj)
        {


            ModelState.Remove("Villa");
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "The Villa Number has been created successfully.";

                return RedirectToAction("Index", "VillaNumber");
            }
            return View();
        }


    }
}
