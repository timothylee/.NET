using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public interface IProduct
    {
        int ProductKey { get; set; }
        int? ProductSubcategoryKey { get; set; }
        string Color { get; set; }
        short? SafetyStockLevel { get; set; }
        short? ReorderPoint { get; set; }
        decimal? DealerPrice { get; set; }
        string ProductName { get; set; }

        void Fill(DimProduct dimProduct);
    }
}