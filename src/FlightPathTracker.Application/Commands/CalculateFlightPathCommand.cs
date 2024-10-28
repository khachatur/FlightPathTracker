using MediatR;

namespace FlightPathTracker.Application.Commands
{
    public class CalculateFlightPathCommand : IRequest<List<string>>
    {
        public List<List<string>> Flights { get; }

        public CalculateFlightPathCommand(List<List<string>> flights)
        {
            Flights = flights ?? new List<List<string>>();
        }
    }
}