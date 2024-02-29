using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ResumeMatcher.Common;
using ResumeMatcher.Models;
using SkiaSharp;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using static System.Formats.Asn1.AsnWriter;

namespace ResumeMatcher.Controllers
{
    public class Instance
    {
        public string content { get; set; }
    }

    public class RequestBody
    {
        public Instance[] instances { get; set; }
    }

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _logger = logger;

            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;

        }

        public IActionResult Index()
        {
            return View();
        }



        // Make prediction with API Key
        public async Task<IActionResult> Result(IFormFile con)
        {
            var _projectId = _configuration.GetValue<string>("GoogleCloud:projectId");
            var _region = _configuration.GetValue<string>("GoogleCloud:region");
            var _endpointName = _configuration.GetValue<string>("GoogleCloud:endpointName");
            var _token = _configuration.GetValue<string>("GoogleCloud:token");
            string url = $"https://{_region}-aiplatform.googleapis.com/v1/projects/{_projectId}/locations/{_region}/endpoints/{_endpointName}:predict";
            //var credentials = GoogleCredential.FromFile(Path.Combine(_webHostEnvironment.WebRootPath,"ethereal-art-414519-006418dfb5e3.json"));
            //credentials = credentials.CreateScoped(scope,audience);
            //string accessToken = await (credentials as ITokenAccess).GetAc();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                var res = con.FileToText();
                if (res == null)
                {
                    return View(new PredictionResult());
                }
                // Define the request body
                var requestBody = new RequestBody()
                {
                    instances = new[]
                    {
                new Instance{
                    content = res.ToString()
                }
            }
                };

                // Serialize the request body to JSON
                var json = JsonConvert.SerializeObject(requestBody);
                // string json = JsonConvert.SerializeObject(inputData);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    RootObject responseBody = JsonConvert.DeserializeObject<RootObject>(result);
                    var combined = new List<PredictionResult>();
                    for (var i = 0; i < responseBody.Predictions[0].Confidences.Count; i++)
                    {
                        combined.Add(new PredictionResult() { Confidence = responseBody.Predictions[0].Confidences[i], DisplayName = responseBody.Predictions[0].DisplayNames[i], Id = responseBody.Predictions[0].Ids[i] });
                    }
                    var resu = combined.OrderByDescending(x => x.Confidence).First();
                    return View(resu);
                }
                else
                {
                    return View(new PredictionResult());
                }
            }
        }
    //    public async Task<IActionResult> MultiResult(IList<IFormFile> con)
    //    {
    //        var _projectId = _configuration.GetValue<string>("GoogleCloud:projectId");
    //        var _region = _configuration.GetValue<string>("GoogleCloud:region");
    //        var _endpointName = _configuration.GetValue<string>("GoogleCloud:endpointName");
    //        var _token = _configuration.GetValue<string>("GoogleCloud:token");
    //        string url = $"https://{_region}-aiplatform.googleapis.com/v1/projects/{_projectId}/locations/{_region}/endpoints/{_endpointName}:predict";
    //        //var credentials = GoogleCredential.FromFile(Path.Combine(_webHostEnvironment.WebRootPath,"ethereal-art-414519-006418dfb5e3.json"));
    //        //credentials = credentials.CreateScoped(scope,audience);
    //        //string accessToken = await (credentials as ITokenAccess).GetAc();
    //        using (var client = new HttpClient())
    //        {
    //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

    //            // Define the request body
    //            var requestBody = new RequestBody()
    //            {
    //            };
    //            foreach (var c in con)
    //        {
    //            requestBody.instances.Append(new Instance() { content= c.FileToText().ToString()});
    //        }
    //        // Serialize the request body to JSON
    //        var json = JsonConvert.SerializeObject(requestBody);
    //        // string json = JsonConvert.SerializeObject(inputData);
    //        HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
    //        HttpResponseMessage response = await client.PostAsync(url, content);

    //            if (response.IsSuccessStatusCode)
    //            {
    //                string result = await response.Content.ReadAsStringAsync();
    //                RootObject responseBody = JsonConvert.DeserializeObject<RootObject>(result);
    //                var resu = new List<PredictionResult>();
    //                for (var j = 0; j < responseBody.Predictions.Count; j++)
    //                {
    //                    var combined = new List<PredictionResult>();
    //                    for (var i = 0; i < responseBody.Predictions[j].Confidences.Count; i++)
    //                    {
    //                        combined.Add(new PredictionResult() { Confidence = responseBody.Predictions[0].Confidences[i], DisplayName = responseBody.Predictions[0].DisplayNames[i], Id = responseBody.Predictions[0].Ids[i] });
    //                    }
    //                    var res = combined.OrderByDescending(x => x.Confidence).First();
    //                }
    //            return View(res);
    //        }
    //        else
    //        {
    //            return View(new PredictionResult());
    //        }
    //    }
    //}
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