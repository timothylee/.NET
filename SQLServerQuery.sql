SELECT TOP 100
product.ProductKey as 'Product Key',
product.EnglishProductName as 'Product Name',
product.SafetyStockLevel as 'Safety Stock Level',
product.ReorderPoint as 'Reorder Point',
product.Color as 'Color',
product.Status as 'Status',
product.DealerPrice as 'Dealer Price',
subcategory.EnglishProductSubcategoryName as 'Subcategory Name'
FROM DimProduct product
JOIN DimProductSubcategory subcategory on product.ProductSubcategoryKey = subcategory.ProductCategoryKey
WHERE product.Status is not null
order by [Dealer Price] desc

SELECT
--employee.EmployeeKey as 'Employee Key',
--employee.FirstName as 'First Name',
--employee.LastName as 'Last Name',
--employee.MiddleName as 'Middle Name',
--employee.Title as 'Title',
--employee.BirthDate as 'Birth Date',
--employee.HireDate as 'Hire Date',
--employee.DepartmentName as 'Department',
territory.SalesTerritoryRegion as 'Sales Territory Region',
COUNT(employee.EmployeeKey) as 'Employees'
FROM DimEmployee employee
JOIN DimSalesTerritory territory on employee.SalesTerritoryKey = territory.SalesTerritoryKey
WHERE territory.SalesTerritoryRegion != 'NA'
GROUP BY territory.SalesTerritoryRegion
HAVING COUNT(employee.EmployeeKey) > 1