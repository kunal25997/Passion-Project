using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Passion_Project.Models
{
    public class Dishes
    {
        [Key]
        public int DishID { get; set; }

        public string DishName { get; set; }

        public decimal DishPrice { get; set; }
    }

    public class DishesDto
    {
        public int DishID { get; set; }

        public string DishName { get; set; }

        public decimal DishPrice { get; set; }
    }
}