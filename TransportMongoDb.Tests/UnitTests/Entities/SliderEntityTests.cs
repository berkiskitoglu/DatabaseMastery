using DatabaseMastery.TransportMongoDb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransportMongoDb.Tests.UnitTests.Entities
{
    public class SliderEntityTests
    {

        // Unit Test

        [Fact]
        public void Slider_Should_Have_Required_Properties()
        {
            // Arrange & Act
            var slider = new Slider
            {
                Id = "123",
                Title = "Test",
                ImageUrl = "test.jpg",

            };

            // Assert
            Assert.Equal("123", slider.Id);
            Assert.Equal("Test", slider.Title);
            Assert.Equal("test.jpg", slider.ImageUrl);
        }

    }
}
