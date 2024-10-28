using Microsoft.AspNetCore.Mvc;
using Moq;
using FlightPathTracker.Api.Controllers;
using FlightPathTracker.Application.Commands;
using MediatR;

namespace FlightPathTracker.Tests.Api
{
    public class FlightPathTrackerControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly FlightPathTrackerController _controller;

        public FlightPathTrackerControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new FlightPathTrackerController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Calculate_NullCommand_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.Calculate(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid command or flights data.", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        public async Task Calculate_EmptyFlights_ReturnsBadRequest()
        {
            // Arrange
            var command = new CalculateFlightPathCommand(new List<List<string>>());

            // Act
            var result = await _controller.Calculate(command);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid command or flights data.", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        public async Task Calculate_InvalidFlightStructure_ReturnsBadRequest()
        {
            // Arrange
            var command = new CalculateFlightPathCommand(new List<List<string>>
        {
            new List<string> { "SFO" } // Invalid flight (only one airport code)
        });

            // Act
            var result = await _controller.Calculate(command);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Each flight must have a source and a destination.", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        public async Task Calculate_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var command = new CalculateFlightPathCommand(new List<List<string>>
        {
            new List<string> { "SFO", "ATL" },
            new List<string> { "ATL", "EWR" }
        });

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CalculateFlightPathCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<string> { "SFO", "EWR" });

            // Act
            var result = await _controller.Calculate(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(new List<string> { "SFO", "EWR" }, okResult.Value);
        }

        [Fact]
        public async Task Calculate_MediatorThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var command = new CalculateFlightPathCommand(new List<List<string>> { new List<string> { "SFO", "EWR" } });
            _mediatorMock.Setup(m => m.Send(It.IsAny<CalculateFlightPathCommand>(), It.IsAny<CancellationToken>()))
                          .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.Calculate(command);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode); 
        }

    }
}