using FlightPathTracker.Domain.Entities;
using MediatR;

namespace FlightPathTracker.Application.Commands
{
    public class CalculateFlightPathCommandHandler : IRequestHandler<CalculateFlightPathCommand, List<string>>
    {
        public Task<List<string>> Handle(CalculateFlightPathCommand request, CancellationToken cancellationToken)
        {
            var flights = request.Flights
                .Select(f => new FlightPath(f[0], f[1]))
                .ToList();

            var inDegrees = new Dictionary<string, int>();
            var outDegrees = new Dictionary<string, int>();

            foreach (var flight in flights)
            {
                // Update out-degree for source
                if (!outDegrees.ContainsKey(flight.Source))
                    outDegrees[flight.Source] = 0;
                outDegrees[flight.Source]++;

                // Update in-degree for destination
                if (!inDegrees.ContainsKey(flight.Destination))
                    inDegrees[flight.Destination] = 0;
                inDegrees[flight.Destination]++;
            }

            // Identify start and end points
            string? start = null, end = null;
            foreach (var airport in outDegrees.Keys)
            {
                if (!inDegrees.ContainsKey(airport) || outDegrees[airport] > inDegrees.GetValueOrDefault(airport, 0))
                    start = airport;
            }

            foreach (var airport in inDegrees.Keys)
            {
                if (!outDegrees.ContainsKey(airport) || inDegrees[airport] > outDegrees.GetValueOrDefault(airport, 0))
                    end = airport;
            }

            // Ensure start and end are not null
            if (start == null || end == null)
            {
                throw new InvalidOperationException("Unable to determine a valid start and end point.");
            }

            return Task.FromResult(new List<string> { start, end });
        }
    }
}