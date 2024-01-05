using System;
using System.Text.Json;
using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;

namespace ECommerce.Api.Search.Services
{
	public class OrdersService : IOrdersService
	{
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<OrdersService> logger;

		public OrdersService(IHttpClientFactory httpClientFactory, ILogger<OrdersService> logger)
		{
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
		}

        public async Task<(bool IsSuccess, IEnumerable<Order>? orders, string? ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var client = httpClientFactory.CreateClient("Orders");
                var response = await client.GetAsync($"api/orders/{customerId}");
                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<IEnumerable<Order>>(content, options);
                    return (true, result, null);
                }
                return (false, null, response.ReasonPhrase);
            }
            catch(Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}

