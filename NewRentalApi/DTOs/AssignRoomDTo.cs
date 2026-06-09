namespace NewRentalApi.DTOs
{
    public class AssignRoomDto
    {
        public int TenantId { get; set; }

        public List<int> RoomIds { get; set; }

        public DateTime RentStartDate { get; set; }

        public decimal MonthlyRent { get; set; }
    }
}
