using Microsoft.EntityFrameworkCore;
using RatingService.Data;
using RatingService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RatingDbContext>(opt => { opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")); });

builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<IRatingService, RatingService.Services.RatingService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

try
{
    DbInitializer.InitDb(app);
}
catch (Exception ex)
{
    //TODO: log
}

app.Run();