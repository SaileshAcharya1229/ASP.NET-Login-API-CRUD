using ConsumeWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConsumeWebApi.Controllers
{
   
    public class UserController : Controller
    {
        private string baseURL = "https://localhost:7181/api/Login";
       
        public async Task<IActionResult> Login(UserLogin user)
        {
            UserLogin obj = new UserLogin
            {
                UserName = user.UserName,
                Password = user.Password
            };
            await Console.Out.WriteLineAsync("obj.UserName");
            await Console.Out.WriteLineAsync("obj.Password");


            if(obj.UserName != null && obj.Password != null)
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync($"?UserName={obj.UserName}&Password={obj.Password}");
                    if(response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        return NotFound();
                    }

                }

            }
            return View();
        }
    }
}
