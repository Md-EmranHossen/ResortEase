using Microsoft.AspNetCore.Mvc;
using ResortEase.Application.Common.Interfaces;
using ResortEase.Domain.Entities;
using ResortEase.Infrastructure.Data;

namespace ResortEase.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaRepository _villaRepo;

        public VillaController(IVillaRepository vilaRepo)
        {
            _villaRepo = vilaRepo;
        }

        public IActionResult Index()
        {
            var villas = _villaRepo.GetAll();
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
                _villaRepo.Add(obj);
                _villaRepo.Save();
                TempData["success"] = "The villa has been created successfully.";

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _villaRepo.Get(u => u.Id == villaId);
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
                _villaRepo.Update(obj);
                _villaRepo.Save();
                TempData["success"] = "The villa has been updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        public IActionResult Delete(int id)
        {
            Villa? obj = _villaRepo.Get(u => u.Id == id);


            if (obj == null)
            {
                return RedirectToAction("Error", "Home");

            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objfromDb = _villaRepo.Get(u => u.Id == obj.Id);
            if (obj != null)
            {
                _villaRepo.Remove(objfromDb);
                _villaRepo.Save();
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction("Index", "Villa");
            }
            TempData["error"] = "The villa could not be deleted.";

            return View();
        }
    }
}
