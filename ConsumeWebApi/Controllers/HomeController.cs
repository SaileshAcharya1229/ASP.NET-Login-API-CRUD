﻿using ConsumeWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace ConsumeWebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private string baseURL = "https://localhost:7181/";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            //calling the web api and populating the data in  view  using dataTable

            DataTable dt = new DataTable();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("Contacts");

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    dt = JsonConvert.DeserializeObject<DataTable>(results);
                }
                else
                {
                    Console.WriteLine("Error Calling web API");

                }
                ViewData.Model = dt;
            }

            return View();
        }

        public async Task<IActionResult> Index2()
        {
            //calling the web api and populating the data in  view  using model class

            IList<UserEntity> user = new List<UserEntity>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("Contacts");

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<List<UserEntity>>(results);
                }
                else
                {
                    Console.WriteLine("Error Calling web API");

                }
                ViewData.Model = user;
            }

            return View();
        }

        public async Task<ActionResult<string>> AddUser(UserEntity user)
        {
            UserEntity obj = new UserEntity()
            {
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address
            };

            if (user.FullName != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.PostAsJsonAsync("Contacts", obj);

                    if (getData.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Console.WriteLine("Error Calling web API");

                    }
                }


            }
            return View();
        }


        public async Task<ActionResult<string>> DeleteUser(Guid id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.DeleteAsync($"Contacts/{id}");

                if (getData.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Console.WriteLine("Error Calling web API");
                }
                return View();
            }

        }
        //public async Task<IActionResult> EditUser(Guid id)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(baseURL);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        var response = await client.GetAsync($"Contacts/{id}");
        //        await Console.Out.WriteLineAsync(response.ToString());

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var content = await response.Content.ReadAsStringAsync();
        //            var user = JsonConvert.DeserializeObject<UpdateUser>(content);
        //            return View("EditUser", user);
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //}


        //public async Task<IActionResult> UpdateUser(UpdateUser user)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(baseURL);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        //        var response = await client.PutAsJsonAsync($"Contacts/{user.Id}", user);

        //        if(response.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("Index", "Home");

        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //}

        public async Task<IActionResult> EditUser(Guid id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL + "Contacts/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync($"{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UpdateUser>(content);
                    return View("UpdateData", user);
                }
                else
                {
                    return NotFound();
                }
            }
        }
        public async Task<IActionResult> UpdateUser(UpdateUser user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL + "Contacts/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PutAsJsonAsync($"{user.Id}", user);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return NotFound();
                }
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}