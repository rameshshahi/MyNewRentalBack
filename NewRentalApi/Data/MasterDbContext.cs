using Microsoft.EntityFrameworkCore;
using NewRentalApi.Models;
namespace NewRentalApi.Data
{


    public class MasterDbContext : DbContext
    {
        public MasterDbContext(DbContextOptions<MasterDbContext> options)
            : base(options)
        {
        }

        public DbSet<OwnerModel> tblOwner { get; set; }
    }
}
