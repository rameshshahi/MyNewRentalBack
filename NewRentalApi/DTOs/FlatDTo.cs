namespace NewRentalApi.DTOs
{
    public class FlatDto
    {
        public int FloorId { get; set; }
        public string FlatNo { get; set; }
        public string FlatName { get; set; }
        public decimal FlatRent { get; set; }
        public bool IsOccupied { get; set; }
    }
}
