using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class HomeViewModel
    {
        public Product Product { get; set; }
        public PagedList<IProduct> ProductList { get; set; }
    }
}