# Flight Path Tracker API

## Overview

The **Flight Path Tracker API** is a microservice designed to help track and understand flight paths based on provided flight records.

Story: There are over 100,000 flights a day, with millions of people and cargo being transferred worldwide. With so many people and different carrier/agency groups, tracking where a person might be can be hard. To determine a person's flight path, we must sort through their flight records.

Goal: To create a simple microservice API to help us understand and track how a particular person's flight path may be queried. The API should accept a request that includes a list of flights defined by a source and destination airport code. These flights may not be listed in order and must be sorted to find the total flight paths starting and ending at airports.

Required JSON structure:

- [["SFO", "EWR"]] => ["SFO", "EWR"]
- [["ATL", "EWR"], ["SFO", "ATL"]] => ["SFO", "EWR"]
- [["IND", "EWR"], ["SFO", "ATL"], ["GSO", "IND"], ["ATL", "GSO"]] => ["SFO", "EWR"]

## Specifications

- **Port**: The microservice listens on **port 8080**.
- **Endpoint**: The API exposes the flight path tracker under the `FlightPathTracker/calculate` endpoint.

## Installation

### Prerequisites

- .NET SDK (version 8.0 or later)

### Steps to Run the Project

1. Clone the repository:

   ```bash
   git clone https://github.com/khachatur/FlightPathTracker.git
   cd FlightPathTracker

   ```

2. Restore dependencies:

   ```bash
   dotnet restore

   ```

3. Run the application:

   ```bash
   dotnet run --project src/FlightPathTracker.Api

   ```

4. The API will be accessible at http://localhost:8080.

## API Endpoint

### POST /calculate

### Request Body

JSON array of flights:

```json
{
  "flights": [
    ["ATL", "EWR"],
    ["SFO", "ATL"]
  ]
}
```

### Response

- JSON array representing the calculated flight path.

Response body

```json
["SFO", "EWR"]
```

### Testing

You can test the API using Swagger UI or tools like Postman or curl.

### Conclusion

The API provides an efficient way to track flight paths based on airport codes.
