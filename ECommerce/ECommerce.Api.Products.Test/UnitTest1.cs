using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Products.Test;

public class ProductsServiceTest
{
    [Fact]
    public async Task GetProductsReturnAllProductsAsync()
    {
        var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductsReturnAllProductsAsync)).Options;
        var dbContext = new ProductsDbContext(options);
        CreateProducts(dbContext);
        var productProfile = new ProductProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
        var mapper = new Mapper(configuration);
        var productsProvider = new ProductsProvider(dbContext,null,mapper);
        var products = await productsProvider.GetProductsAsync();
        Assert.True(products.IsSuccess);
        Assert.True(products.Products.Any());
        Assert.Null(products.ErrorMessage);
    }

    [Fact]
    public async Task GetProductsReturnProductsUsingValidIdAsync()
    {
        var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductsReturnProductsUsingValidIdAsync)).Options;
        var dbContext = new ProductsDbContext(options);
        CreateProducts(dbContext);
        var productProfile = new ProductProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
        var mapper = new Mapper(configuration);
        var productsProvider = new ProductsProvider(dbContext, null, mapper);
        var products = await productsProvider.GetProductAsync(1);
        Assert.True(products.IsSuccess);
        Assert.NotNull(products.Product);
        Assert.True(products.Product.Id == 1);
        Assert.Null(products.ErrorMessage);
    }


    [Fact]
    public async Task GetProductsReturnProductsUsingInValidIdAsync()
    {
        var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductsReturnProductsUsingInValidIdAsync)).Options;
        var dbContext = new ProductsDbContext(options);
        CreateProducts(dbContext);
        var productProfile = new ProductProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
        var mapper = new Mapper(configuration);
        var productsProvider = new ProductsProvider(dbContext, null, mapper);
        var products = await productsProvider.GetProductAsync(-1);
        Assert.False(products.IsSuccess);
        Assert.Null(products.Product);
        Assert.NotNull(products.ErrorMessage);
    }

    private void CreateProducts(ProductsDbContext dbContext)
    {
        for (int i = 1; i < 10; i++)
        {
            dbContext.Products.Add(new Product()
            {
                Id = i,
                Name = Guid.NewGuid().ToString(),
                Inventory = i + 10,
                Price = (decimal)(i * 3.14)
            });
            
        }

        dbContext.SaveChanges();
    }
}
