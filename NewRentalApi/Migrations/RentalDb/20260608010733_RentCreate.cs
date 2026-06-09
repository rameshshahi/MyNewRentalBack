using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewRentalApi.Migrations.RentalDb
{
    /// <inheritdoc />
    public partial class RentCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblExpense",
                columns: table => new
                {
                    ExpenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpenseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblExpense", x => x.ExpenseId);
                });

            migrationBuilder.CreateTable(
                name: "tblHouse",
                columns: table => new
                {
                    HouseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblHouse", x => x.HouseId);
                });

            migrationBuilder.CreateTable(
                name: "tblTenant",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CitizenshipNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermanentAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemporaryAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmergencyContactName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmergencyContactNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTenant", x => x.TenantId);
                });

            migrationBuilder.CreateTable(
                name: "tblUtilityBill",
                columns: table => new
                {
                    UtilityBillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    BillMonth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElectricityCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WaterCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InternetCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GarbageCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUtilityBill", x => x.UtilityBillId);
                });

            migrationBuilder.CreateTable(
                name: "tblFloor",
                columns: table => new
                {
                    FloorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseId = table.Column<int>(type: "int", nullable: false),
                    FloorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FloorNumber = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFloor", x => x.FloorId);
                    table.ForeignKey(
                        name: "FK_tblFloor_tblHouse_HouseId",
                        column: x => x.HouseId,
                        principalTable: "tblHouse",
                        principalColumn: "HouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblMaintenance",
                columns: table => new
                {
                    MaintenanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMaintenance", x => x.MaintenanceId);
                    table.ForeignKey(
                        name: "FK_tblMaintenance_tblHouse_HouseId",
                        column: x => x.HouseId,
                        principalTable: "tblHouse",
                        principalColumn: "HouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblRentPayment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    PaymentMonth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FineAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRentPayment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_tblRentPayment_tblTenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "tblTenant",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblTenantBill",
                columns: table => new
                {
                    BillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    RentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ElectricityCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WaterCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GarbageCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InternetCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreviousDue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingDue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    BillDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTenantBill", x => x.BillId);
                    table.ForeignKey(
                        name: "FK_tblTenantBill_tblTenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "tblTenant",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblTenantDocument",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTenantDocument", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_tblTenantDocument_tblTenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "tblTenant",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblFlat",
                columns: table => new
                {
                    FlatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FloorId = table.Column<int>(type: "int", nullable: false),
                    FlatNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlatName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlatRent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsOccupied = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlat", x => x.FlatId);
                    table.ForeignKey(
                        name: "FK_tblFlat_tblFloor_FloorId",
                        column: x => x.FloorId,
                        principalTable: "tblFloor",
                        principalColumn: "FloorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblRoom",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlatId = table.Column<int>(type: "int", nullable: false),
                    RoomNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MonthlyRent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsOccupied = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRoom", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_tblRoom_tblFlat_FlatId",
                        column: x => x.FlatId,
                        principalTable: "tblFlat",
                        principalColumn: "FlatId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblTenantRoom",
                columns: table => new
                {
                    TenantRoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    RentStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RentEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MonthlyRent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTenantRoom", x => x.TenantRoomId);
                    table.ForeignKey(
                        name: "FK_tblTenantRoom_tblRoom_RoomId",
                        column: x => x.RoomId,
                        principalTable: "tblRoom",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblTenantRoom_tblTenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "tblTenant",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblFlat_FloorId",
                table: "tblFlat",
                column: "FloorId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFloor_HouseId",
                table: "tblFloor",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_tblMaintenance_HouseId",
                table: "tblMaintenance",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_tblRentPayment_TenantId",
                table: "tblRentPayment",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_tblRoom_FlatId",
                table: "tblRoom",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_tblTenantBill_TenantId",
                table: "tblTenantBill",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_tblTenantDocument_TenantId",
                table: "tblTenantDocument",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_tblTenantRoom_RoomId",
                table: "tblTenantRoom",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_tblTenantRoom_TenantId",
                table: "tblTenantRoom",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblExpense");

            migrationBuilder.DropTable(
                name: "tblMaintenance");

            migrationBuilder.DropTable(
                name: "tblRentPayment");

            migrationBuilder.DropTable(
                name: "tblTenantBill");

            migrationBuilder.DropTable(
                name: "tblTenantDocument");

            migrationBuilder.DropTable(
                name: "tblTenantRoom");

            migrationBuilder.DropTable(
                name: "tblUtilityBill");

            migrationBuilder.DropTable(
                name: "tblRoom");

            migrationBuilder.DropTable(
                name: "tblTenant");

            migrationBuilder.DropTable(
                name: "tblFlat");

            migrationBuilder.DropTable(
                name: "tblFloor");

            migrationBuilder.DropTable(
                name: "tblHouse");
        }
    }
}
