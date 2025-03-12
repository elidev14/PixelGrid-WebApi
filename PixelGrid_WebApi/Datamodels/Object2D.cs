using System.ComponentModel.DataAnnotations;

namespace PixelGrid_WebApi.Datamodels
{
    public class Object2D
    {
        public Guid ID { get; set; }
        public Guid EnvironmentID { get; set; }
        [Required]
        public string PrefabID { get; set; }

        [Required]
        [Range(0, 200)]
        public float PosX { get; set; }

        [Required]
        [Range(0, 100)]
        public float PosY { get; set; }
        [Required]
        public float ScaleX { get; set; }
        [Required]
        public float ScaleY { get; set; }
        [Required]
        public float RotationZ { get; set; }
        [Required]
        public int SortingLayer { get; set; }
    }
}
