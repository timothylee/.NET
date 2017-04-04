using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class DefaultProduct : IProduct
    {
        public int ProductKey { get; set; }
        public int? ProductSubcategoryKey { get; set; }
        public string Color { get; set; }
        public short? SafetyStockLevel { get; set; }
        public short? ReorderPoint { get; set; }
        public decimal? DealerPrice { get; set; }
        public string ProductName { get; set; }

        public void Fill(DimProduct dimProduct)
        {
            Color = dimProduct.Color;
            DealerPrice = dimProduct.DealerPrice;
            ProductKey = dimProduct.ProductKey;
            ProductName = dimProduct.EnglishProductName;
            ProductSubcategoryKey = dimProduct.ProductSubcategoryKey;
            ReorderPoint = dimProduct.ReorderPoint;
            SafetyStockLevel = dimProduct.SafetyStockLevel;

        }
    }
}