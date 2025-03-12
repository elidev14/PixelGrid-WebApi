using System.ComponentModel.DataAnnotations;

namespace PixelGrid_WebApi.Datamodels
{
    public class Environment2D
    {

        //TODO: add seed so that the seed will be saved

        public Guid ID { get; set; }

        public string OwnerUserId { get; set; }

        [Required] public string Name { get; set; }

  
        [Range(10, 100)]
        [Required] public double MaxHeight { get; set; }

 
        [Range(20, 200)]
        [Required] public double MaxLength { get; set; }

        [Required] public int Seed { get; set; }

    }
}
