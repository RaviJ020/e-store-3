using ShoppingCartService.Controllers.Models;

namespace ShoppingCartServiceTests.Builders
{
    public class ItemDtoBuilder
    {
        private string _productId = "1a2f3b4d5e6f7a8b9c0d1e2f";
        private string _productName = "product 1";
        private double _price = 1.0;
        private uint _quantity = 1;

        public ItemDtoBuilder WithProductId(string productId)
        {
            _productId = productId;
            return this;
        }

        public ItemDtoBuilder WithProductName(string productName)
        {
            _productName = productName;
            return this;
        }

        public ItemDtoBuilder WithPrice(double price)
        {
            _price = price;
            return this;
        }

        public ItemDtoBuilder WithQuantity(uint quantity)
        {
            _quantity = quantity;
            return this;
        }

        public ItemDto Build()
        {
            return new ItemDto(_productId, _productName, _price, _quantity);
        }
    }
}
