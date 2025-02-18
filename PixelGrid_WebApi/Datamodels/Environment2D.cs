using System.ComponentModel.DataAnnotations;

namespace PixelGrid_WebApi.Datamodels
{
    public class Environment2D
    {

        public Guid ID { get; set; }
        public required string Name { get; set; }
        public required string OwnerUserId { get; set; }

        [Range(10, 100)]
        public double MaxHeight { get; set; }
        [Range(20, 200)]
        public double MaxLength { get; set; }

    }
}
