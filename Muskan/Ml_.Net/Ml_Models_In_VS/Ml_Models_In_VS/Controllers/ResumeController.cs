using Microsoft.AspNetCore.Mvc;
using Ml_Models_In_VS.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Ml_Models_In_VS.Controllers
{
    public class ResumeController : Controller
    {
        private readonly HttpClient _httpClient;

        public ResumeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PredictCategory(IFormFileCollection resumes)
        {
            if (resumes == null || resumes.Count == 0)
            {
                ViewBag.ErrorMessage = "Please upload at least one resume.";
                return View("Index");
            }

            var formData = new MultipartFormDataContent();
            foreach (var resume in resumes)
            {
                var resumeContent = new StreamContent(resume.OpenReadStream());
                resumeContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                formData.Add(resumeContent, "resumes", resume.FileName);
            }

            var response = await _httpClient.PostAsync("http://127.0.0.1:5000/predict-resume", formData);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = $"API Error: {response.ReasonPhrase}";
                return View("Index");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResumePredictionResult>(responseString);

            
            if (result?.PredictedCategories == null || result.PredictedCategories.Count == 0)
            {
                ViewBag.ErrorMessage = "No categories were predicted.";
                return View("Index");
            }
            foreach (var predictedCategory in result.PredictedCategories)
            {
                if (double.TryParse(predictedCategory.ConfidenceScore, out double confidence))
                {                  
                    predictedCategory.ConfidenceScore = (Math.Round(confidence * 100, 2)).ToString();
                }
            }

            var uniqueCategories = result.PredictedCategories
                .Select(c => c.PredictedCategory)
                .Distinct()
                .ToList();

           
            ViewBag.UniqueCategories = uniqueCategories;
            ViewBag.PredictedCategories = result.PredictedCategories;

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetSimilarResumes(IFormFileCollection resumes, string target_role)
        {
            if (resumes == null || resumes.Count == 0)
            {
                ViewBag.ErrorMessage = "Please upload at least one resume.";
                return View("Index");
            }

            if (string.IsNullOrEmpty(target_role))
            {
                ViewBag.ErrorMessage = "Please select a target role.";
                return View("Index");
            }

            var formData = new MultipartFormDataContent();

            
            foreach (var resume in resumes)
            {
                var resumeContent = new StreamContent(resume.OpenReadStream());
                resumeContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                formData.Add(resumeContent, "resumes", resume.FileName);
            }

           
            formData.Add(new StringContent(target_role), "target_role");

            
            var response = await _httpClient.PostAsync("http://127.0.0.1:5000/resume-similarity", formData);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();

               
                //Console.WriteLine("API Response: " + responseString);

                
                var result = JsonConvert.DeserializeObject<ResumeSimilarityResult>(responseString);

               
                if (result?.Top5SimilarResumes == null || !result.Top5SimilarResumes.Any())
                {
                    ViewBag.ErrorMessage = "No similarity results found.";
                    return View("Index");
                }
                foreach (var resume in result.Top5SimilarResumes)
                {
                    resume.SimilarityScore = Math.Round(resume.SimilarityScore * 100, 2); 
                }
              
                ViewBag.SimilarityResults = result.Top5SimilarResumes;

                return View("SimilarityResult");
            }
            else
            {
                ViewBag.ErrorMessage = "Error while fetching similarity results.";
                return View("Index");
            }
        }



    }
}
