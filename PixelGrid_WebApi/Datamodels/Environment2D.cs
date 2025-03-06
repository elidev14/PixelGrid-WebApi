using System.ComponentModel.DataAnnotations;

namespace PixelGrid_WebApi.Datamodels
{
    public class Environment2D
    {

        public Guid ID { get; set; }

        public string OwnerUserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(10, 100)]
        public double MaxHeight { get; set; }

        [Required]
        [Range(20, 200)]
        public double MaxLength { get; set; }

    }
}
