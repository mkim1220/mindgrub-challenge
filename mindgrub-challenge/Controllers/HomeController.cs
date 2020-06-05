using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mindgrub_challenge.Models;
using mindgrub_challenge.Repositories;

namespace mindgrub_challenge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private GeolocationRepository _repo;

        public HomeController(ILogger<HomeController> logger, GeolocationRepository glRepo)
        {
            _logger = logger;
            _repo = glRepo;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ZipcodeViewModel zipcodeVM)
        {
            if (ModelState.IsValid)
            {
                if (_repo.ZipcodeExists(zipcodeVM.Zipcode))
                {
                    _repo.CalculateLocation(zipcodeVM);
                    return RedirectToAction(nameof(HomeController.Calculated), "Home", zipcodeVM);
                } 
                else
                {
                    ViewBag.Message = "Zipcode does not found in database.";
                }
            }
            return View(zipcodeVM);
        }

        public ActionResult Calculated(ZipcodeViewModel zipcodeVM)
        {
            return View(zipcodeVM);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
