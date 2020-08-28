namespace Order.Core.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;
    using Base;
    public interface IOrderRepository: IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
    }
}
