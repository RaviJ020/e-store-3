using AutoMapper;
using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess;
using Xunit;
using ShoppingCartService.Mapping;
using Microsoft.Extensions.Logging.Abstractions;
using ShoppingCartService.Controllers;
using ShoppingCartServiceTests.Builders;
using ShoppingCartService.Controllers.Models;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartService.DataAccess.Entities;
using FluentAssertions;
using System.Linq;

namespace ShoppingCartServiceTests.Controllers
{
    [Collection("Database collection")]
    public class ShoppingCartControllerIntegrationTests
    {
        private MongoDbFixture _mongoDbFixture;
        public ShoppingCartControllerIntegrationTests(MongoDbFixture mongoDbFixture)
        {
            _mongoDbFixture = mongoDbFixture;
        }

        [Fact]
        public void CreateCart_HappyPath()
        {
            // Assign
            string customerId = "4f8a7b2c6d9e0a5b3c1d2f8e";
            var controller = CreateController();
            var cartDto = new CreateCartDtoBuilder()
                .WithCustomer(new CustomerDtoBuilder()
                    .WithId(customerId)
                    .Build())
                .Build();

            // Act
            var result = (CreatedAtRouteResult) controller.Create(cartDto).Result;
            var value = (ShoppingCartDto) result.Value;

            // Assert
            Assert.Equal(customerId, value.CustomerId);
        }

        [Fact]
        public void AddItemToCart_HappyPath()
        {
            // Assign
            var controller = CreateController();
            var cartDto = new CreateCartDtoBuilder().Build();
            var createdResult = (CreatedAtRouteResult)controller.Create(cartDto).Result;
            var createdValue = (ShoppingCartDto)createdResult.Value;
            var item = new ItemDtoBuilder().Build();

            // Act
            controller.AddItemToCart(createdValue.Id, item);
            var findResult = controller.FindById(createdValue.Id);
            var findValue = findResult.Value;
            var itemResult = findValue.Items.Single();

            // Assert
            itemResult.Should().BeEquivalentTo(item);
        }

        [Fact]
        public void RemoveCart_HappyPath()
        {
            // Assign
            var controller = CreateController();
            var cartDto = new CreateCartDtoBuilder().Build();
            var createdResult = (CreatedAtRouteResult)controller.Create(cartDto).Result;
            var createdValue = (ShoppingCartDto)createdResult.Value;

            // Act
            controller.DeleteCart(createdValue.Id);
            var findResult = controller.FindById(createdValue.Id);

            // Assert
            Assert.IsType<NotFoundResult>(findResult.Result);
        }

        private ShoppingCartController CreateController()
        {
            var repository = new ShoppingCartRepository(_mongoDbFixture.DataBaseSettings);
            var addressValidator = new AddressValidator();
            var shippingCalculator = new ShippingCalculator();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();
            var checkOutEngine = new CheckOutEngine(shippingCalculator, mapper);
            var logger = new NullLogger<ShoppingCartController>();
            var manager = new ShoppingCartManager(repository, addressValidator, mapper, checkOutEngine);
            return new ShoppingCartController(manager, logger);
        }
    }
}
