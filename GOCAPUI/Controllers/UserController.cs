using GOCAPUI.Models.DTO.Post;
using GOCAPUI.Models.DTO.User;
using GOCAPUI.Models.ResponseJson;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GOCAPUI.Controllers
{
    public class UserController : Controller
    {

        private readonly HttpClient client = null;
        private string UserApiUrl = "";
        private string PostApiUrl = "";

        public UserController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PostApiUrl = "https://localhost:7035/posts";
            UserApiUrl = "https://localhost:7035/users";
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Profile()
        {
            Guid id = Guid.Parse("ed2fba68-f9b8-463e-8062-5436e61e4da9"); 
            UserModel u = await GetUserByID(id);
            if (u == null)
            {
                return NotFound();
            }

            IEnumerable<PostModel> p = await GetAllPost(id);
            ViewBag.Posts = p;
            return View(u);
        }

        //=========================================
        public async Task<UserModel> GetUserByID(Guid? id)
        {
            if (id == null)
            {
                return null;
            }

            HttpResponseMessage response = await client.GetAsync($"{UserApiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                UserModel user = JsonSerializer.Deserialize<UserModel>(strData, options);
                return user;
            }

            return null;
        }

        public async Task<IEnumerable<PostModel>> GetAllPost(Guid? id)
        {
            string filter = "";

            if(id!= null)
            {
                filter = $"$filter=UserId eq {id} &";
            }
            Console.WriteLine(filter);

            HttpResponseMessage response = await client.GetAsync($"{PostApiUrl}?{filter}$expand=Medias");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var jsonResponse = JsonSerializer.Deserialize<PostResponse>(strData, options);
            IEnumerable<PostModel> a = jsonResponse.Value;
            return a;
        }
        //=========================================
    }
}
