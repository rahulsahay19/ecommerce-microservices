namespace Catalog.API.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;

    public interface IProductRepo
  {
      Task<IEnumerable<Product>> GetProducts();
      Task<Product> GetProduct(string id);
      Task<IEnumerable<Product>> GetProductByName(string name);
      Task<IEnumerable<Product>> GetProductByCategory(string categoryName);
      Task Create(Product product);
      Task<bool> Update(Product product);
      Task<bool> Delete(string id);
    }
}
