﻿using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1.Models
{
    internal class Realtor
    {
        [Key]
        public int Id { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
    }
}
