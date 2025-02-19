using System.ComponentModel.DataAnnotations;

namespace PixelGrid_WebApi.Datamodels
{
    public class Object2D
    {
        public required Guid ID { get; set; }
        public required Guid EnvironmentID { get; set; }
        [Required]
        public required char PrefabID { get; set; }
        [Required]
        public required float PosX { get; set; }
        [Required]
        public required float PosY { get; set; }
        [Required]
        public required float ScaleX { get; set; }
        [Required]
        public required float ScaleY { get; set; }
        [Required]
        public required float RotationZ { get; set; }
        [Required]
        public required int SortingLayer { get; set; }
    }
}
