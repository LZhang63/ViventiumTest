using System.ComponentModel.DataAnnotations;

namespace ViventiumTest.Models
{
    public class TestDbFirst
    {
        [Key]
        public uint Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

    }
}
