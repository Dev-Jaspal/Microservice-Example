using System;
using ECommerce.Api.Orders.Models;

namespace ECommerce.Api.Orders.Interfaces
{
	public interface IOrdersProvider
	{
        Task<(bool IsSuccess, IEnumerable<Order>? Orders, string? ErrorMessage)> GetOrdersAsync(int customerId);
    }
}

