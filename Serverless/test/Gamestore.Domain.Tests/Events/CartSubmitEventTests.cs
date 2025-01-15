using FluentAssertions;
using Gamestore.Domain.Events;
using Gamestore.Domain.Extensions;
using Newtonsoft.Json.Linq;

namespace Gamestore.Domain.Tests.Events;

public class CartSubmitEventTests
{
    [Test]
    public void CartSubmitEvent_Should_Serialized()
    {
        // Arrange
        var expectedResult =
            JToken.Parse(
                @"{ ""customer_id"": ""2323"", ""first_name"": ""first"", ""last_name"": ""last"", ""email"": ""mail@mail.com"", ""products"": [{ ""product_id"": ""1234"", ""quantity"": 2}, {""product_id"": ""9754"", ""quantity"": 3}]}");
        var sut = new CartSubmitEvent("2323", "first", "last", "mail@mail.com",
            new[]
            {
                new CartSubmitEvent.Product("1234", 2), 
                new CartSubmitEvent.Product("9754", 3)
            });
        
        // Act
        var result = sut.ToJson();

        // Asserts
        JToken.Parse(result).Should().BeEquivalentTo(expectedResult);
    }
}