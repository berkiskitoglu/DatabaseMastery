using DatabaseMastery.TransportMongoDb.Dtos.SliderDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransportMongoDb.Tests
{
    public class CreateSliderDtoTests
    {

        // Unit Tests

        [Fact]
        public void CreateSliderDto_Should_Have_Properties()
        {
            // Arrange & Act
            var dto = new CreateSliderDto
            {
                Title = "Test Slider",
                ImageUrl = "https.//example.com/img.jpg"
            };

            // Assert
            Assert.Equal("Test Slider", dto.Title);
            Assert.Equal("https.//example.com/img.jpg", dto.ImageUrl);

        }

        [Fact]
        public void CreateSliderDto_Should_Initialize_With_Empty_Values()
        {
            // Arrange & Act
            var dto = new CreateSliderDto();

            //Assert
            Assert.Null(dto.Title);
            Assert.Null(dto.ImageUrl);
        }
    }
}
