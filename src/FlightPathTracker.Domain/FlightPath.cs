namespace FlightPathTracker.Domain.Entities;

public class FlightPath
{
    public string Source { get; set; }
    public string Destination { get; set; }

    public FlightPath(string source, string destination)
    {
        Source = source;
        Destination = destination;
    }
}
