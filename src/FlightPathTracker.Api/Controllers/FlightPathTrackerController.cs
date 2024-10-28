using Microsoft.AspNetCore.Mvc;
using MediatR;
using FlightPathTracker.Application.Commands;

namespace FlightPathTracker.Api.Controllers
{
    [ApiController]
    [Route("calculate")]
    public class FlightPathTrackerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FlightPathTrackerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Calculate([FromBody] CalculateFlightPathCommand command)
        {
            // Validate the command itself
            if (command == null || command.Flights == null || command.Flights.Count == 0)
            {
                return BadRequest("Invalid command or flights data.");
            }

            // Ensure all flights contain both source and destination
            if (command.Flights.Any(flight => flight == null || flight.Count != 2))
            {
                return BadRequest("Each flight must have a source and a destination.");
            }

            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }
}