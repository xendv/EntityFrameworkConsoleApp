using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp1.Models
{
    internal class Apartment
    {
        [Key]
        public int Id { get; set; }
        public int District { get; set; }
        public string Address { get; set; }
        public int Floor { get; set; }
        public int Rooms { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int Material { get; set; }
        public double Area { get; set; }
        public DateOnly Date { get; set; }

        [ForeignKey("District")]
        public virtual District District_ { get; set; }

        [ForeignKey("Type")]
        public virtual Type Type_ { get; set; }

        [ForeignKey("Material")]
        public virtual Material Material_ { get; set; }
    }
}
