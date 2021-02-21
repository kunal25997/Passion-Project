using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Passion_Project.Models
{
    public class Recipes
    {
        [Key]
        public int RecipeID { get; set; }

        public string RecipeType { get; set; }

        public int NoOfServings { get; set; }

        //A recipe for one dish
        [ForeignKey("Dishes")]
        public int DishID { get; set; }
        public virtual Dishes Dishes { get; set; }
    }

    public class RecipeDto
    {
        public int RecipeID { get; set; }

        public string RecipeType { get; set; }

        public int NoOfServings { get; set; }


    }
}