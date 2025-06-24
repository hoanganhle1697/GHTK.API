using GHTK.Repository.Abstractions.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHTK.Repository;

public interface IOrderRepository
{
    Task CreateOrderAsync(Order orderEntity);
    Task<Order> FindOrderAsync(string id, string? partnerId);
}
