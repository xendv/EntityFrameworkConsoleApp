using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1.Models
{
    internal class Criteria
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
