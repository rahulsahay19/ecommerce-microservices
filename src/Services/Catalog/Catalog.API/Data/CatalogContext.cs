namespace Catalog.API.Data
{
    using Entities;
    using Settings;
    using MongoDB.Driver;
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(ICatalogDbSettings catalogDbSettings)
        {
            var client = new MongoClient(catalogDbSettings.ConnectionString);
            var database = client.GetDatabase(catalogDbSettings.DatabaseName);
            Products = database.GetCollection<Product>(catalogDbSettings.CollectionName);
            CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; }
    }
}
