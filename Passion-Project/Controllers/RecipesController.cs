using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Passion_Project.Models;
using Passion_Project.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace Passion_Project.Controllers
{
    public class RecipesController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static RecipesController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:64913/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }



        // GET: Player/List
        public ActionResult List()
        {
            string url = "recipedata/getrecipes";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<RecipeDto> SelectedRecipes = response.Content.ReadAsAsync<IEnumerable<RecipeDto>>().Result;
                return View(SelectedRecipes);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Recipes/Details/5
        public ActionResult Details(int id)
        {
            ShowRecipe ViewModel = new ShowRecipe();
            string url = "recipedata/findrecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into recipe data transfer object
                RecipeDto SelectedPlayer = response.Content.ReadAsAsync<RecipeDto>().Result;
                ViewModel.recipe = SelectedPlayer;


                url = "playerdata/findteamforplayer/" + id;
                response = client.GetAsync(url).Result;
                RecipeDto SelectedRecipe = response.Content.ReadAsAsync<RecipeDto>().Result;
                ViewModel.recipe = SelectedRecipe;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Player/Create
        public ActionResult Create()
        {
            UpdateRecipe ViewModel = new UpdateRecipe();
            //get information about teams this player COULD play for.
            string url = "dishdata/getdishes";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DishesDto> PotentialDishes = response.Content.ReadAsAsync<IEnumerable<DishesDto>>().Result;
            ViewModel.alldishes = PotentialDishes;

            return View(ViewModel);
        }

        // POST: Player/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Recipes RecipeInfo)
        {
            Debug.WriteLine(RecipeInfo.RecipeType);
            string url = "recipedata/addrecipe";
            Debug.WriteLine(jss.Serialize(RecipeInfo));
            HttpContent content = new StringContent(jss.Serialize(RecipeInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int RecipeId = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = RecipeId });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Player/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateRecipe ViewModel = new UpdateRecipe();

            string url = "recipedata/findrecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                RecipeDto SelectedRecipes = response.Content.ReadAsAsync<RecipeDto>().Result;
                ViewModel.recipe = SelectedRecipes;

                //get information about teams this player COULD play for.
                url = "dishdata/getdishes";
                response = client.GetAsync(url).Result;
                IEnumerable<DishesDto> PotentialDishes = response.Content.ReadAsAsync<IEnumerable<DishesDto>>().Result;
                ViewModel.alldishes = PotentialDishes;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Player/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Recipes RecipeInfo, HttpPostedFileBase RecipePic)
        {
            Debug.WriteLine(RecipeInfo.RecipeType);
            string url = "recipedata/updaterecipe/" + id;
            Debug.WriteLine(jss.Serialize(RecipeInfo));
            HttpContent content = new StringContent(jss.Serialize(RecipeInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                //Send over image data for player
                url = "recipedata/updaterecipepic/" + id;
                Debug.WriteLine("Received recipe picture " + RecipePic.FileName);

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(RecipePic.InputStream);
                requestcontent.Add(imagecontent, "RecipePic", RecipePic.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Player/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "recipedata/findrecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                RecipeDto SelectedRecipes = response.Content.ReadAsAsync<RecipeDto>().Result;
                return View(SelectedRecipes);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Player/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "recipedata/deleterecipe/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
