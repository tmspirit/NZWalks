namespace NZWalksAPI.Models.Domain
{
    public class Region
    {
        public Guid Id { get; set; }
        public String Code { get; set; }
        public String Name { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public long Population { get; set; }
        public int Area { get; set; }

        //Navigation Property
        // Una region puede tener multiples walks
        public IEnumerable<Walk> Walks { get; set; }
    }
}

