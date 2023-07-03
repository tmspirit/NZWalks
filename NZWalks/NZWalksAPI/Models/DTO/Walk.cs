using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Models.DTO
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Length { get; set; }
        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }

        //Navigation Property
        //Definimos la conexion entre Walk y Region
        public Region Region { get; set; }
        //Definimos la conexion entre Walk y WalkDifficulty
        public WalkDifficulty WalkDifficulty { get; set; }
    }
}
