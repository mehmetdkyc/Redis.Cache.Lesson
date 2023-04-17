using Microsoft.EntityFrameworkCore;
using RedisExampleApp.Cache;
using RedisExampleApp.Models;
using RedisExampleApp.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("myDatabase");
});
builder.Services.AddSingleton<RedisService>(sp =>{
    return new RedisService(builder.Configuration["CacheOptions:Url"]);
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext= scope.ServiceProvider.GetRequiredService<AppDbContext>(); // buras� tamamen elle datalar� yazd���m�z i�in onlar� redise kaydetmek i�in aya�a kalkarken �a��rd�k yoksa mssqle kaydedersek gerek yoktu.
    dbContext.Database.EnsureCreated();
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
