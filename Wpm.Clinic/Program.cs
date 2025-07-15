using Microsoft.EntityFrameworkCore;
using Polly;
using Wpm.Clinic.Application;
using Wpm.Clinic.DataAccess;
using Wpm.Clinic.ExternalServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ManagementService>();
builder.Services.AddScoped<ClinicApplicationService>();
builder.Services.AddDbContext<ClinicDbContext>(options => 
{
    options.UseInMemoryDatabase("WpmClinic");
});

builder.Services.AddHttpClient<ManagementService>(client =>
{
    var uri = builder.Configuration.GetValue<string>("Wpm:Management_Uri");
    Console.WriteLine($"Config URI: {uri}");
    client.BaseAddress = new Uri(uri);
}).AddResilienceHandler("management-pipeline", builder => {

    builder.AddRetry(new Polly.Retry.RetryStrategyOptions<HttpResponseMessage>()
    {
        BackoffType = DelayBackoffType.Exponential,
        MaxRetryAttempts = 3,
        Delay = TimeSpan.FromSeconds(10)
    });

});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ClinicDbContext>();
    await db.Database.EnsureCreatedAsync(); 
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
