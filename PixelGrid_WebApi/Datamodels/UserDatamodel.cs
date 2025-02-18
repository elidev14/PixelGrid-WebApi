using System.ComponentModel.DataAnnotations;

namespace PixelGrid_WebApi.Datamodels
{
    public class UserDatamodel
    {
        public Guid ID { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
