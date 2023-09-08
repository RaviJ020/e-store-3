using FluentAssertions;
using ShoppingCartService.DataAccess;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartServiceTests.Builders;
using System.Collections.Generic;
using Xunit;

namespace ShoppingCartServiceTests.DataAccess
{
    [Collection("Database collection")]
    public class ShoppingCartRepositoryIntegrationTests
    {
        private MongoDbFixture _mongoDbFixture;
        public ShoppingCartRepositoryIntegrationTests(MongoDbFixture mongoDbFixture)
        {
            _mongoDbFixture = mongoDbFixture;
        }

        [Fact]
        public void CreateById_HappyPath()
        {
            // Assign
            var cart = new CartBuilder().WithId(null).WithItems(new List<Item> { ItemBuilder.CreateItem() }).Build();
            var repository = new ShoppingCartRepository(_mongoDbFixture.DataBaseSettings);

            // Act
            var result = repository.Create(cart);

            // Assert
            result.Should().BeEquivalentTo(cart);
        }

        [Fact]
        public void GetById_HappyPath()
        {
            // Assign
            var cart = new CartBuilder().WithId(null).WithItems(new List<Item> { ItemBuilder.CreateItem() }).Build();
            var repository = new ShoppingCartRepository(_mongoDbFixture.DataBaseSettings);
            _ = repository.Create(cart);

            // Act
            var result = repository.FindById(cart.Id);

            // Assert
            result.Should().BeEquivalentTo(cart);
        }

        [Fact]
        public void Update_HappyPath()
        {
            // Assign
            var oldCart = new CartBuilder()
                .WithCustomerId("13d79bea2cc32fe5c4037d44")
                .WithCustomerType(ShoppingCartService.Models.CustomerType.Standard)
                .WithShippingMethod(ShoppingCartService.Models.ShippingMethod.Standard)
                .WithShippingAddress(AddressBuilder.CreateAddress("country 1", "city 1", "street 1"))
                .WithItems(new List<Item> { ItemBuilder.CreateItem("3fae944d41908b442dfa04fc", "name 1", 5, 1) })
                .Build();
            var newCart = new CartBuilder()
                .WithCustomerId("52066eb4da0d340d1e857ef0")
                .WithCustomerType(ShoppingCartService.Models.CustomerType.Premium)
                .WithShippingMethod(ShoppingCartService.Models.ShippingMethod.Express)
                .WithShippingAddress(AddressBuilder.CreateAddress("country 2", "city 2", "street 2"))
                .WithItems(new List<Item> 
                { 
                    ItemBuilder.CreateItem("daa2ef0793e2fd43f359c863", "name 2", 3, 2),
                    ItemBuilder.CreateItem("5c36c93b7aaf272e15d5cc32", "name 3", 7, 3)
                })
                .Build();
            var repository = new ShoppingCartRepository(_mongoDbFixture.DataBaseSettings);
            var createdCart = repository.Create(oldCart);

            // Act
            repository.Update(createdCart.Id, newCart);
            var result = repository.FindById(createdCart.Id);

            // Assert
            result.Should().BeEquivalentTo(newCart);
        }

        [Fact]
        public void RemoveById_HappyPath()
        {
            // Assign
            var cart = new CartBuilder().WithId(null).WithItems(new List<Item> { ItemBuilder.CreateItem() }).Build();
            var repository = new ShoppingCartRepository(_mongoDbFixture.DataBaseSettings);
            repository.Create(cart);

            // Act
            repository.Remove(cart.Id);
            var result = repository.FindById(cart.Id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void RemoveByObject_HappyPath()
        {
            // Assign
            var cart = new CartBuilder().WithId(null).WithItems(new List<Item> { ItemBuilder.CreateItem() }).Build();
            var repository = new ShoppingCartRepository(_mongoDbFixture.DataBaseSettings);
            repository.Create(cart);

            // Act
            repository.Remove(cart);
            var result = repository.FindById(cart.Id);

            // Assert
            Assert.Null(result);
        }
    }
}
