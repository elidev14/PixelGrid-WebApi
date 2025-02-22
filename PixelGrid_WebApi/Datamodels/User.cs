using System.ComponentModel.DataAnnotations;

namespace PixelGrid_WebApi.Datamodels
{
    public class User
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
