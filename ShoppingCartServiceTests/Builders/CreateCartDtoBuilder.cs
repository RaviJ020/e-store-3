namespace ShoppingCartServiceTests.Builders
{
    using System.Collections.Generic;
    using System.Linq;
    using ShoppingCartService.Controllers.Models;
    using ShoppingCartService.Models;

    public class CreateCartDtoBuilder
    {
        private CustomerDto _customer = new CustomerDtoBuilder().Build();
        private IEnumerable<ItemDto> _items = Enumerable.Empty<ItemDto>();
        private ShippingMethod _shippingMethod = ShippingMethod.Standard;

        public CreateCartDtoBuilder WithCustomer(CustomerDto customer)
        {
            _customer = customer;
            return this;
        }

        public CreateCartDtoBuilder WithItems(IEnumerable<ItemDto> items)
        {
            _items = items;
            return this;
        }

        public CreateCartDtoBuilder AddItem(ItemDto item)
        {
            _items = _items.Concat(new[] { item });
            return this;
        }

        public CreateCartDtoBuilder WithShippingMethod(ShippingMethod shippingMethod)
        {
            _shippingMethod = shippingMethod;
            return this;
        }

        public CreateCartDto Build()
        {
            return new CreateCartDto
            {
                Customer = _customer,
                Items = _items,
                ShippingMethod = _shippingMethod
            };
        }
    }

}
