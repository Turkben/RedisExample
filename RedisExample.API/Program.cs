using Microsoft.EntityFrameworkCore;
using RedisExample.API;
using RedisExample.API.Repositories;
using RedisExample.API.Services;
using RedisExample.Cache;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("InMemoryDb");
});


builder.Services.AddSingleton<RedisRepository>(sp =>
{
    return new RedisRepository(builder.Configuration["CacheOptions:URL"]);
});

//builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IProductRepository>(sp =>
{
    var appDbContext = sp.GetRequiredService<AppDbContext>();
    var productRepository = new ProductRepository(appDbContext);
    var redisRepository = sp.GetRequiredService<RedisRepository>();
    return new ProductRepositoryWithCache(productRepository, redisRepository);
});

builder.Services.AddScoped<IProductService,ProductService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
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
