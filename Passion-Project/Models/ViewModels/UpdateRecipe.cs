using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Passion_Project.Models.ViewModels
{
    public class UpdateRecipe
    {
        public RecipeDto recipe { get; set; }
        //Needed for a dropdownlist which presents the player with a choice of teams to play for
        public IEnumerable<DishesDto> alldishes { get; set; }
    }
}