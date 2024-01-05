using System;
using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Customers.Providers
{
	public class CustomersProvider : ICustomersProvider
	{
		private readonly CustomersDbContext dbContext;
		private readonly ILogger<CustomersProvider> logger;
		private readonly IMapper mapper;

		public CustomersProvider(CustomersDbContext dbContext, ILogger<CustomersProvider> logger, IMapper mapper)
		{
			this.dbContext = dbContext;
			this.logger = logger;
			this.mapper = mapper;

			SeedData();
		}

        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer() { Id = 1, Name = "Jaspal", Address = "15281 81A Surrey" });
                dbContext.Customers.Add(new Db.Customer() { Id = 2, Name = "Jasmine", Address = "15281 81A Surrey" });
                dbContext.Customers.Add(new Db.Customer() { Id = 3, Name = "Angel", Address = "15281 81A Newton" });
                dbContext.Customers.Add(new Db.Customer() { Id = 4, Name = "Jacky", Address = "15281 81A Vancouver" });
                dbContext.Customers.Add(new Db.Customer() { Id = 5, Name = "Gagan", Address = "15281 81A England" });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, Models.Customer? Customer, string? ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var customer = await dbContext.Customers.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (customer != null)
                {
                    var result = mapper.Map<Db.Customer, Models.Customer>(customer);
                    return (true, result, null);
                }

                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }

        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer>? Customers, string? ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await dbContext.Customers.ToListAsync();
                if (customers != null && customers.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result, null);
                }

                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

    }
}

