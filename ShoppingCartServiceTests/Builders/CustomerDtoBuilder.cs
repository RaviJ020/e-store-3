using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartServiceTests.Builders
{
    using ShoppingCartService.Controllers.Models;
    using ShoppingCartService.Models;
    using System.ComponentModel.DataAnnotations;

    public class CustomerDtoBuilder
    {
        private string _id = "3d9a5c7b8f2e6a0d1c4b9e8f";
        private Address _address = AddressBuilder.CreateAddress();
        private CustomerType _customerType = CustomerType.Standard;

        public CustomerDtoBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public CustomerDtoBuilder WithAddress(Address address)
        {
            _address = address;
            return this;
        }

        public CustomerDtoBuilder WithCustomerType(CustomerType customerType)
        {
            _customerType = customerType;
            return this;
        }

        public CustomerDto Build()
        {
            return new CustomerDto
            {
                Id = _id,
                Address = _address,
                CustomerType = _customerType
            };
        }
    }

}
