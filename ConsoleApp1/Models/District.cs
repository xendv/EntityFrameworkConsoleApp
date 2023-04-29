using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1.Models
{
    internal class District
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
