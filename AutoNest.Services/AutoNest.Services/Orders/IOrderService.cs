using AutoNest.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNest.Services.Orders
{
    public interface IOrderService
    {
        public Task AddOrderAsync(OrderInputModel inputModel, string userId);
    }
}
