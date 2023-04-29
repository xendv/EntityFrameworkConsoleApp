using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp1.Models
{
    internal class Sale
    {
        [Key]
        public int Id { get; set; }
        public int Apartment { get; set; }
        public DateOnly Date { get; set; }
        public int Realtor { get; set; }
        public double Price { get; set; }

        [ForeignKey("Apartment")]
        public virtual Apartment Apartment_ { get; set; }

        [ForeignKey("Realtor")]
        public virtual Realtor Realtor_ { get; set; }
    }
}
