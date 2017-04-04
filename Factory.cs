/// <summary>
/// Factory to create a specific product object based on the product type
/// </summary>
/// <param name="PhysicalDigital">Indicates if a product is a physical or digital product</param>
/// <returns>a Physical, Digital, or Default project</returns>
public static BaseProduct GetProduct(string PhysicalDigital)
{
	BaseProduct product;
	switch (PhysicalDigital)
	{
		case "Physical":
			product = new PhysicalProduct();
			break;
		case "Digital":
			product = new DigitalProduct();
			break;
		default:
			product = new DefaultProduct();
			break;
	}
	product.PhysicalDigital = PhysicalDigital;

	return product;
}

/// <summary>
/// Interface for all product objects
/// </summary>
public interface IProduct
{
	void FillProperties(ProjectCreationInfo projectInfo);
	void SetKeyDates(ProjectDataSet projectDataSet);
	string ValidateKeyDates(SPListItem item);
	string SetEarlyShips(ProjectDataSet projectDataSet);
}