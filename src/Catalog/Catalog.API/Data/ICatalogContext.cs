namespace Catalog.API.Data
{
    using Entities;
    using MongoDB.Driver;
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
