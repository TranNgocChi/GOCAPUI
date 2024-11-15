using GOCAPUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;

namespace GOCAPUI.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IHttpClientFactory httpClientFactory, ILogger<AdminController> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7182/odata/");
            _logger = logger;
        }

        // Trang Admin chính (Index)
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Trang User (IndexUser)
        [HttpGet("IndexUser")]
        public IActionResult IndexUser()
        {
            return View();
        }

        // API lấy tất cả người dùng
        [HttpGet("users/data")]
        public async Task<IActionResult> GetAllUsers()
            
            IEnumerable<PostModel> a = await GetAllPost();

            IEnumerable<UserModel> b = await GetAllUser();
            ViewBag.Users = b;

            return View(a);
        }
        public async Task<IActionResult> CreatePost()
        {
            IEnumerable<UserModel> b = await GetAllUser();
            ViewBag.UserId = new SelectList(b, "Id", "Name");
            return View();
        }
        [HttpPost]
        [RequestSizeLimit(104857600)]
      
        public async Task<IActionResult> CreatePost([Bind("Content,UserId,MediaFiles")] PostCreationModel postCreation)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>("users");

                if (response == null)
                {
                    _logger.LogWarning("No users found.");
                    return NotFound("Không tìm thấy người dùng nào.");
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

                                // Kiểm tra loại tệp
                                if (file.ContentType.StartsWith("video/"))
                                {
                                    // Xử lý tệp video
                                    content.Add(fileContent, "MediaFiles", file.FileName);
                                    Console.WriteLine($"Tệp video đã được thêm: {file.FileName}");
                                }
                                else if (file.ContentType.StartsWith("image/"))
                                {
                                    // Xử lý tệp hình ảnh
                                    content.Add(fileContent, "MediaFiles", file.FileName);
                                    Console.WriteLine($"Tệp hình ảnh đã được thêm: {file.FileName}");
                                }
                                else
                                {
                                    // Nếu loại tệp không phải là video hoặc hình ảnh
                                    Console.WriteLine($"Loại tệp không hỗ trợ: {file.FileName}");
                                    ModelState.AddModelError("MediaFiles", "Chỉ hỗ trợ tải lên hình ảnh và video.");
                                    return View(postCreation); // Hoặc xử lý phù hợp
                                }
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

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users.");
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
        }

        public async Task<IActionResult> DetailPost(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

        // API lấy thông tin người dùng theo ID
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<UserDTO>($"users/{id}");

                if (response == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    return NotFound($"Không tìm thấy người dùng với ID {id}.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching user with ID {id}.");
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
        }

        // API tạo người dùng mới
        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync("users", model);

                if (response.IsSuccessStatusCode)
                {
                    var createdUser = await response.Content.ReadFromJsonAsync<UserDTO>();
                    return CreatedAtAction(nameof(GetUserById), new { id = createdUser?.Id }, createdUser);
                }

                _logger.LogWarning("Unable to create user.");
                return BadRequest("Không thể tạo người dùng.");
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
                _logger.LogError(ex, "Error creating user.");
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
=======
                PropertyNameCaseInsensitive = true
            };
            var jsonResponse = JsonSerializer.Deserialize<PostResponse>(strData, options);
            IEnumerable<PostModel> a = jsonResponse.Value;
            return a;
>>>>>>> 6685573664c01ae968c4e0c0a0dd2ba79759dd18
        }

        // API cập nhật thông tin người dùng
        [HttpPatch("users/{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserCreationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            try
            {
                var response = await _httpClient.PatchAsJsonAsync($"users/{id}", model);

                if (response.IsSuccessStatusCode)
                {
                    var updatedUser = await response.Content.ReadFromJsonAsync<UserDTO>();
                    return Ok(updatedUser);
                }

                _logger.LogWarning("Unable to update user.");
                return BadRequest("Không thể cập nhật người dùng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with ID {id}.");
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
        }

        // API xóa người dùng
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"users/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return NoContent();
                }

                _logger.LogWarning("Unable to delete user.");
                return BadRequest("Không thể xóa người dùng.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with ID {id}.");
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
        }

        // API để hiển thị trang người dùng (IndexUser)
        [HttpGet("users")]
        public async Task<IActionResult> DisplayUsers()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>("users");

                if (response == null)
                {
                    _logger.LogWarning("No users found.");
                    return NotFound("Không tìm thấy người dùng nào.");
                }

                return View(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error displaying users.");
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
        }
    }
}
