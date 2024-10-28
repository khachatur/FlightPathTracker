using FlightPathTracker.Application.Commands;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CalculateFlightPathCommand>());
// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flight Path Tracker API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

app.UseRouting();
app.MapControllers(); // Top-level route registration

// Set the application to listen on port 8080
app.Urls.Add("http://localhost:8080");

app.Run();