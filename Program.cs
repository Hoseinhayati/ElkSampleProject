using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
    .DefaultIndex("books")
    .BasicAuthentication("elastic", "Z6KEcZjMMnccWWAEkh0r"); // Replace "username" and "password" with your actual credentials
var client = new ElasticClient(settings);
builder.Services.AddSingleton<IElasticClient>(client);


var app = builder.Build();

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
