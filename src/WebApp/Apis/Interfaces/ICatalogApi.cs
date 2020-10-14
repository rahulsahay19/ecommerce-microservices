using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Apis.Interfaces
{
   public interface ICatalogApi
   {
       Task<IEnumerable<CatalogModel>> GetCatalog();
       Task<IEnumerable<CatalogModel>> GetCatalogByCatagory(string category);
       Task<IEnumerable<CatalogModel>> GetCatalogById(string id);
       Task<IEnumerable<CatalogModel>> CreateCatalog(CatalogModel catalogModel);
   }
}
