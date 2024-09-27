using appstorm_sql_dotnet_test.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace appstorm_sql_dotnet_test.Infrastructure.Data
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
