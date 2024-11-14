﻿using GOCAPUI.Models.DTO.Post;
using GOCAPUI.Models.DTO.User;
using GOCAPUI.Models.ResponseJson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GOCAPUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient client = null;
        private string PostApiUrl = "";
        private string UserApiUrl = "";
        public AdminController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PostApiUrl = "https://localhost:7035/posts";
            UserApiUrl = "https://localhost:7035/users";
        }
        public async Task<IActionResult> Index()
        {
           
            IEnumerable<PostModel> a = await GetAllPost();
            int? count = a.Count();
            IEnumerable<UserModel> b = await GetAllUser();
            int? countUser = b.Count();
            if (count != null)
            {
                ViewBag.PostCount = count; 
                                                
            }
            if (countUser != null)
            {
                ViewBag.UserCount = countUser;

            }
            return View();
        }

        // ======================== CRUD POST =================================
        public async Task<IActionResult> IndexPost()
        {
            
            IEnumerable<PostModel> a = await GetAllPost();

            IEnumerable<UserModel> b = await GetAllUser();
            ViewBag.Users = b;

            return View(a);
        }
        public async Task<IActionResult> CreatePost()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost([Bind("Content,UserId,MediaFiles")] PostCreationModel postCreation)
        {
            if (ModelState.IsValid)
            {
                // Tạo nội dung multipart form-data
                using (var content = new MultipartFormDataContent())
                {
                    // Thêm các trường văn bản
                    content.Add(new StringContent(postCreation.Content ?? string.Empty), "Content");
                    content.Add(new StringContent(postCreation.UserId.ToString()), "UserId");

                    // Kiểm tra và thêm các tệp
                    if (postCreation.MediaFiles != null && postCreation.MediaFiles.Any())
                    {
                        foreach (var file in postCreation.MediaFiles)
                        {
                            if (file.Length > 0)
                            {
                                var stream = file.OpenReadStream();
                                var fileContent = new StreamContent(stream);
                                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                                content.Add(fileContent, "MediaFiles", file.FileName);
                            }
                        }
                    }

                    // Gửi yêu cầu POST
                    var response = await client.PostAsync(PostApiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(IndexPost));
                    }
                }
            }

            return NoContent();
        }
       
        public async Task<IActionResult> DetailPost(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpResponseMessage response = await client.GetAsync($"{PostApiUrl}/{id}?$expand=Medias");
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var product = JsonSerializer.Deserialize<PostModel>(strData, options);

                IEnumerable<UserModel> b = await GetAllUser();
                ViewBag.Users = b;

                return View(product);
            }
            return NotFound();
        }
      

        public async Task<IActionResult> DeletePost(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = await client.DeleteAsync($"{PostApiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(IndexPost));
            }
            return NotFound();
        }
        //==========================================================================
        public async Task<IEnumerable<UserModel>> GetAllUser()
        {
            HttpResponseMessage response = await client.GetAsync(UserApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var jsonResponse = JsonSerializer.Deserialize<UserRespose>(strData, options);
            IEnumerable<UserModel> a = jsonResponse.Value;


            return a;
        }

        public async Task<IEnumerable<PostModel>> GetAllPost()
        {
            HttpResponseMessage response = await client.GetAsync($"{PostApiUrl}?$expand=Medias");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var jsonResponse = JsonSerializer.Deserialize<PostResponse>(strData, options);
            IEnumerable<PostModel> a = jsonResponse.Value;

            return a;
        }

        //=========================================================================
    }
}