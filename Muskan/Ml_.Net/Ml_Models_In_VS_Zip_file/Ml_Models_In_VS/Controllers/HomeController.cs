using Microsoft.AspNetCore.Mvc;
using Ml_Models_In_VS.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Ml_Models_In_VS.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PredictFlower([FromForm] FlowerInput input)
        {
          
            var apiUrl = "http://127.0.0.1:5000/flowers";

            
            var jsonContent = JsonConvert.SerializeObject(input);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

           
            var response = await _httpClient.PostAsync(apiUrl, httpContent);

            if (response.IsSuccessStatusCode)
            {
                
                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<FlowerOutput>(responseString);


                return View("PredictionResult", result);
            }
            else
            {
                ViewBag.ErrorMessage = "Error while making prediction.";
                return View("Error");
            }
        }
    }
}
