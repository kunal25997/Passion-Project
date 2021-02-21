using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Passion_Project.Models;
using System.Diagnostics;

namespace Passion_Project.Controllers
{
    public class RecipesController : ApiController
    {
        private WorldCuisineDataContext db = new WorldCuisineDataContext();

        // GET: api/Recipes/GetRecipes
        public IEnumerable<RecipeDto> GetRecipes()
        {
            List<Recipes> Recipes = db.Recipes.ToList();
            List<RecipeDto> RecipeDto = new List<RecipeDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Recipe in Recipes)
            {
                RecipeDto NewRecipe = new RecipeDto
                {
                    RecipeID = Recipe.RecipeID,
                    RecipeType = Recipe.RecipeType,
                    NoOfServings = Recipe.NoOfServings
                };
                RecipeDto.Add(NewRecipe);
            }

            return RecipeDto;
        }

        // GET: api/Recipes/FindRecipe/5
        [ResponseType(typeof(RecipeDto))]
        [HttpGet]
        public IHttpActionResult FindRecipe(int id)
        {
            //Find the data
            Recipes Recipe = db.Recipes.Find(id);
            //if not found, return 404 status code.
            if (Recipe == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            RecipeDto RecipeDto = new RecipeDto
            {
                RecipeID = Recipe.RecipeID,
                RecipeType = Recipe.RecipeType,
                NoOfServings = Recipe.NoOfServings
            };


            //pass along data as 200 status code OK response
            return Ok(RecipeDto);
        }


        // POST: api/Recipes/UpdateRecipe/5
        // FORM DATA: Recipe JSON Object
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateRecipe(int id, [FromBody] Recipes recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != recipe.RecipeID)
            {
                return BadRequest();
            }

            db.Entry(recipe).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Recipes/AddRecipe
        // FORM DATA: Recipe JSON Object
        [ResponseType(typeof(Recipes))]
        [HttpPost]
        public IHttpActionResult AddRecipe([FromBody] Recipes recipe)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Recipes.Add(recipe);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = recipe.RecipeID }, recipe);
        }

        // DELETE: api/Recipes/5
        [ResponseType(typeof(Recipes))]
        public IHttpActionResult DeleteRecipes(int id)
        {
            Recipes recipes = db.Recipes.Find(id);
            if (recipes == null)
            {
                return NotFound();
            }

            db.Recipes.Remove(recipes);
            db.SaveChanges();

            return Ok(recipes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RecipesExists(int id)
        {
            return db.Recipes.Count(e => e.RecipeID == id) > 0;
        }
    }
}