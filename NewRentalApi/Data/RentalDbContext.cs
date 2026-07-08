using Microsoft.EntityFrameworkCore;
using NewRentalApi.Models;

namespace NewRentalApi.Data
{
    public class RentalDbContext:DbContext
    {
        public RentalDbContext(DbContextOptions<RentalDbContext> options) : base(options)
        {
        }
      public DbSet<HouseModel> tblHouse { get; set; }
        public DbSet<FloorModel> tblFloor { get; set; }
        public DbSet<FlatModel> tblFlat { get; set; }
        public DbSet<RoomModel> tblRoom { get; set; }
        public DbSet<TenantModel> tblTenant { get; set; }
        public DbSet<TenantRoomModel> tblTenantRoom { get; set; }
        public DbSet<RentPaymentModel> tblRentPayment { get; set; }
        public DbSet<UtilityBillModel> tblUtilityBill { get; set; }
        public DbSet<MaintenanceModel> tblMaintenance { get; set; }
        public DbSet<ExpenseModel> tblExpense { get; set; }
        public DbSet<TenantDocumentModel> tblTenantDocument { get; set; }
        public DbSet<TenantBillModel> tblTenantBill { get; set; }
        public DbSet<TenantOtp> TenantOtps { get; set; }

        public DbSet<TenantPaymentModel> tblTenantPayment { get; set; }

        public DbSet<NotificationModel> tblNotification { get; set; }
    }
}
