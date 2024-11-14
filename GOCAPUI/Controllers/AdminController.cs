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
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>("users");

                if (response == null)
                {
                    _logger.LogWarning("No users found.");
                    return NotFound("Không tìm thấy người dùng nào.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users.");
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
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
                _logger.LogError(ex, "Error creating user.");
                return StatusCode(500, $"Lỗi máy chủ nội bộ: {ex.Message}");
            }
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
