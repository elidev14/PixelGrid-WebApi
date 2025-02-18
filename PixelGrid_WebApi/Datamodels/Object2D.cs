namespace PixelGrid_WebApi.Datamodels
{
    public class Object2D
    {
        public required Guid ID { get; set; }
        public required Guid EnvironmentID { get; set; }
        public required char PrefabID { get; set; }
        public required float PosX { get; set; }
        public required float PosY { get; set; }
        public required float ScaleX { get; set; }
        public required float ScaleY { get; set; }
        public required float RotationZ { get; set; }
        public required int SortingLayer { get; set; }
    }
}
