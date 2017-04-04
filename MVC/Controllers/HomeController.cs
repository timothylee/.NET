using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Factories;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string searchString, int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;

            AdventureworksDW2016CTP3Entities dbContext = new AdventureworksDW2016CTP3Entities();
            HomeViewModel viewModel = new HomeViewModel();

            var productList = new List<IProduct>();
            var products = dbContext.DimProducts.Where(p => p.DealerPrice.HasValue).Take(100).OrderBy(p => p.EnglishProductName).ToList();

            foreach (var dimProduct in products)
                productList.Add(ProductFactory.GetProduct(dimProduct));
                
            viewModel.ProductList = new PagedList<IProduct>(productList.AsQueryable(), pageNumber, pageSize);

            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}