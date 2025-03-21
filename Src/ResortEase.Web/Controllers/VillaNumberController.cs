using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResortEase.Application.Common.Interfaces;
using ResortEase.Domain.Entities;
using ResortEase.Infrastructure.Data;
using ResortEase.Web.ViewModels;

namespace ResortEase.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var VillaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");

            return View(VillaNumbers);
        }


        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            // ModelState.Remove("Villa");

            bool roomNumberExist = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExist)
            {
                _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been created successfully.";

                return RedirectToAction(nameof(Index));
            }
            if (roomNumberExist)
            {
                TempData["error"] = "The Villa Number already exist";
            }


            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(obj);
        }

        public IActionResult Update(int villaNumberId)
        {

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };


            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumberVM);
        }



        [HttpPost]
        public IActionResult Update(VillaNumberVM obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Update(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been updated successfully.";

                return RedirectToAction(nameof(Index));
            }


            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(obj);
        }



        public IActionResult Delete(int villaNumberId)
        {

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };


            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {

            VillaNumber? objFromDb = _unitOfWork.VillaNumber
                .Get(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            if (objFromDb is not null)
            {
                _unitOfWork.VillaNumber.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been deleted successfully.";

                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa Number could not be deleted";
            return View();
        }
    }
}
