using GOCAPUI.Models;
using GOCAPUI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace GOCAPUI.Controllers
{
    public class HomeController(HttpClient httpClient) : Controller
    {
        private readonly string api = $"{AppConfiguration.Server}/posts";

        public class OdataResponse
        {
            public User? Value { get; set; }
       
        }
        public async Task<IActionResult> Index()
        
        {
            var response = await httpClient.GetAsync($"{api}/noodata");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("API call failed.");
            }
            var content = await response.Content.ReadAsStringAsync();
            var responseJwt = await httpClient.GetAsync($"{AppConfiguration.Server}/signin/token");
            string jwtStr = "";
            if (responseJwt.IsSuccessStatusCode)
            {
                jwtStr = await responseJwt.Content.ReadAsStringAsync();
            }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtStr) as JwtSecurityToken;
            
            var userId = jsonToken?.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            var responseUser = await httpClient.GetAsync($"{AppConfiguration.Server}/users/{userId}");
            var responseUserStr = await responseUser.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(responseUserStr);
            var posts = JsonConvert.DeserializeObject<List<PostDisplay>>(content);
            var postsJwt = new PostsJwt
            {
                PostDisplays = posts,
                JwtToken = jwtStr,
                User = user
            };
            return View(postsJwt);
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
