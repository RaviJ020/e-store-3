using System.Collections.Generic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

using static ShoppingCartServiceTests.Builders.AddressBuilder;

namespace ShoppingCartServiceTests.Builders
{
    /// <summary>
    /// This is an example of using the builder pattern
    /// The fluent interface enable us to write easy to read object initialization
    /// </summary>
    public class CartBuilder
    {
        // Initialize the variables with default values
        private string _id = "6d6d5a1d96a2ddb65b090fa2";
        private string _customerId = "5d460b97007f79ac2ee32743";
        private CustomerType _customerType = CustomerType.Standard;
        private ShippingMethod _shippingMethod = ShippingMethod.Standard;
        private Address _shippingAddress = CreateAddress(); // here we use another builder to initialize default value
        private List<Item> _items = new();

        /// <summary>
        /// Create Cart using set values
        /// </summary>
        public Cart Build()
        {
            return new()
            {
                Id = _id,
                CustomerId = _customerId,
                CustomerType = _customerType,
                ShippingMethod = _shippingMethod,
                ShippingAddress = _shippingAddress,
                Items = _items
            };
        }

        // Below are methods to adjust specific values before calling Build()

        public CartBuilder WithId(string id)
        {
            _id = id;

            return this;
        }
        
        public CartBuilder WithCustomerId(string customerId)
        {
            _customerId = customerId;

            return this;
        }

        public CartBuilder WithCustomerType(CustomerType customerType)
        {
            _customerType= customerType;

            return this;
        }

        public CartBuilder WithShippingMethod(ShippingMethod shippingMethod)
        {
            _shippingMethod = shippingMethod;

            return this;
        }

        public CartBuilder WithShippingAddress(Address shippingAddress)
        {
            _shippingAddress = shippingAddress;

            return this;
        }

        public CartBuilder WithItems(List<Item> items)
        {
            _items = items;

            return this;
        }
    }
}