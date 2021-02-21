using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Passion_Project.Models
{
    public class WorldCuisineDataContext : DbContext
    {
        public WorldCuisineDataContext() : base("name=WorldCuisineDataContext")
        {
        }

        public DbSet<Recipes> Recipes { get; set; }
        public DbSet<Dishes> Dishes { get; set; }

    }
}