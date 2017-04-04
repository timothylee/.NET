using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Factories
{
    public static class ProductFactory
    {
        public static IProduct GetProduct(DimProduct dimProduct)
        {
            IProduct product;

            switch (dimProduct.ProductKey)
            {
                case 0:
                    product = new DefaultProduct();
                    break;
                default:
                    product = new Product();
                    break;
            }
            
            product.Fill(dimProduct);

            return product;
        }
    }
}