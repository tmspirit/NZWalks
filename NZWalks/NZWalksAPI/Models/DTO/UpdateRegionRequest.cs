namespace NZWalksAPI.Models.DTO
{
    public class UpdateRegionRequest
    {
        public String Code { get; set; }
        public String Name { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public long Population { get; set; }
        public int Area { get; set; }
    }
}
