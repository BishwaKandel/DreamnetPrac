using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Claims;

namespace HRMSmvc.Controllers
{
    public class BaseController : Controller
    {
        private readonly string _apiBaseUrl;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<BaseController> _logger;



        public BaseController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor , ILogger<BaseController> logger)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        private string GetToken()
        {
            var userClaims = _httpContextAccessor.HttpContext.User.Claims;
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
            .Where(c => c.Type == ClaimTypes.Hash)
            .Select(c => c.Value)
            .FirstOrDefault();
            return userIdClaim ?? "";
        }
        public async Task<T> PostAsync<T>(string endPoint, object model, IFormFile? file)
        
        {
            var client = new RestClient(_apiBaseUrl);
            var request = new RestRequest(endPoint, Method.Post)
                        .AddHeader("Authorization", $"Bearer {GetToken()}");
           

            if (file != null && file.Length > 0)
            {
                foreach (var prop in model.GetType().GetProperties())
                {
                    var value = prop.GetValue(model);
                    if (value != null)
                        request.AddParameter(prop.Name, value, ParameterType.GetOrPost);
                }
                // Save the file to a temporary location or get the path to the file
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream); // Copy the file's content to memory stream
                    byte[] fileBytes = memoryStream.ToArray();  // Get byte array of the file content

                    // Attach the file to the multipart form-data request
                    // "formFile" is the form field name, this needs to match the API's expected parameter name
                    request.AddFile("formFile", fileBytes, file.FileName, file.ContentType);
                }
            }
            else
            {
                request.AddJsonBody(model);
            }
                try
                {
                    var response = await client.ExecuteAsync(request);

                    // Check if the response status is successful (2xx range)
                    if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                    {
                        // Deserialize the response content to the specified type T
                        return JsonConvert.DeserializeObject<T>(response.Content);
                    }

                    // Optionally handle non-success status codes
                    //Console.WriteLine($"Error: {response.StatusCode} - {response.StatusDescription}");
                    _logger.LogError("Error: {StatusCode} - {StatusDescription} while making POST request to {EndPoint}", response.StatusCode, response.StatusDescription, endPoint);
                return default(T);
                }
                catch (Exception ex)
                {
                    // Log the exception or handle as needed
                    //Console.WriteLine($"Exception: {ex.Message}");
                    _logger.LogError(ex, "Exception occurred while making POST request to {EndPoint}", endPoint);
                return default(T);
                }
        }

        public async Task<T> GetAsync<T>(string endPoint)
        {
            var client = new RestClient(_apiBaseUrl);
            var request = new RestRequest(endPoint, Method.Get)
                .AddHeader("Authorization", $"Bearer {GetToken()}");

            try
            {
                var response = await client.ExecuteAsync(request);

                // Check if the response status is successful (2xx range)
                if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                {
                    // Deserialize and return the response content as type T
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }

                // Optionally handle non-success status codes
                //Console.WriteLine($"Error: {response.StatusCode} - {response.StatusDescription}");
                _logger.LogError("Error: {StatusCode} - {StatusDescription} while making GET request to {EndPoint}", response.StatusCode, response.StatusDescription, endPoint);
                return default(T);
            }
            catch (Exception ex)
            {
                // Log the exception or handle as needed
                //Console.WriteLine($"Exception: {ex.Message}");
                _logger.LogError(ex, "Exception occurred while making GET request to {EndPoint}", endPoint);
                return default(T);
            }
        }

    }
}

