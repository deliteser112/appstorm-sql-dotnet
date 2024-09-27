using appstorm_sql_dotnet_test.Infrastructure.Data;
using appstorm_sql_dotnet_test.Core.Interfaces;
using appstorm_sql_dotnet_test.Application.Services;
using Microsoft.EntityFrameworkCore;
using appstorm_sql_dotnet_test.Core.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Entity Framework with In-Memory Database
builder.Services.AddDbContext<ProductsDbContext>(options =>
    options.UseInMemoryDatabase("ProductsDB"));

// Register the repository and service with dependency injection
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
    SeedDatabase(dbContext);
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

static void SeedDatabase(ProductsDbContext context)
{
    if (!context.Products.Any())
    {
        context.Products.AddRange(
            new Product { Name = "Product 1", Price = 150.00M, Stock = 20 },
            new Product { Name = "Product 2", Price = 200.00M, Stock = 10 },
            new Product { Name = "Product 3", Price = 300.00M, Stock = 5 }
        );
        context.SaveChanges();
    }
}
