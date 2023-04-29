using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp1.Models
{
    internal class Score
    {
        [Key]
        public int Id { get; set; }
        public int Apartment { get; set; }
        public DateOnly Date { get; set; }
        public int Criteria { get; set; }
        public double Value { get; set; }

        [ForeignKey("Apartment")]
        public virtual Apartment Apartment_ { get; set; }

        [ForeignKey("Criteria")]
        public virtual Criteria Criteria_ { get; set; }
    }
}
