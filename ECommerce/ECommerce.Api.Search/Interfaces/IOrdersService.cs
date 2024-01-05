using System;
using ECommerce.Api.Search.Models;

namespace ECommerce.Api.Search.Interfaces
{
	public interface IOrdersService
	{
		Task<(bool IsSuccess,IEnumerable<Order>? orders, string? ErrorMessage)> GetOrdersAsync(int customerId);
	}
}

