using Microsoft.AspNetCore.Mvc;
using ResortEase.Domain.Entities;
using ResortEase.Infrastructure.Data;

namespace ResortEase.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
            return View(villas);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("name", "The description cannot exactly match the Name");
            }


            if (ModelState.IsValid)
            {
                _db.Villas.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "The villa has been created successfully.";

                return RedirectToAction("Index", "Villa");
            }
            return View();
        }

        public IActionResult Update(int id)
        {
            Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid)
            {
                _db.Villas.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "The villa has been updated successfully.";

                return RedirectToAction("Index", "Villa");
            }
            return View();
        }


        public IActionResult Delete(int id)
        {
            Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == id);


            if (obj == null)
            {
                return RedirectToAction("Error", "Home");

            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objfromDb = _db.Villas.FirstOrDefault(u => u.Id == obj.Id);
            if (obj != null)
            {
                _db.Villas.Remove(objfromDb);
                _db.SaveChanges();
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction("Index", "Villa");
            }
            TempData["error"] = "The villa could not be deleted.";

            return View();
        }
    }
}
