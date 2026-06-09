namespace NewRentalApi.DTOs
{
    public class RoomDto
    {
        public int FlatId { get; set; }
        public string RoomNo { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public decimal MonthlyRent { get; set; }
        public bool IsOccupied { get; set; }
    }
}
