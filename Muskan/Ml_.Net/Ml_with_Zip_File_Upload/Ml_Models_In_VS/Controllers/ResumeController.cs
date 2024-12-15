using Microsoft.AspNetCore.Mvc;
using Ml_Models_In_VS.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;


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
        public async Task<IActionResult> PredictCategory(IFormFile zipfile) 
        {
            if (zipfile == null || zipfile.Length == 0) //It is validation that ensures user upload a valid file
            {
                ViewBag.ErrorMessage = "Please upload a zip file."; //ViewBag :A dynamic object used in ASP.NET Core to pass data from a controller to a view.Data stored in ViewBag is accessible in the view.
                return View("Index");
            }

            using (var memoryStream = new MemoryStream()) //Ensure the MemoryStream object is properly disposed once it is no longer needeeed ,it prevent memory leaks by ensuring the MemoryStream is clean up even if an exception occurs within the block 
            // var memoryStream = new MemoryStream()   : MemoryStream: represent a stream of data stored in memory ,It allows you to store, read, and write data directly to memory, instead of using files or other physical storage.                                       //var memoryStream = new MemoryStream()
          //temporarily store the uploaded file in memmory 
            {
                await zipfile.CopyToAsync(memoryStream); //It copy the contents of uploaded file to the memorystream ,CopyToAsync is used to avoid blocking the thread while the data is being copied.
                memoryStream.Position = 0;  //reset the position to beginning 

                var formData = new MultipartFormDataContent(); //Created an object to hold multipart form data 
                var zipContent = new StreamContent(memoryStream);//wrap the memmory stream in StreamCOntent object which represent binary content of file 
                zipContent.Headers.ContentType = new MediaTypeHeaderValue("application/zip");  //specifies the MIME type ,it indicate that uploaded file is zip archive 
                formData.Add(zipContent, "resumes", zipfile.FileName); //Add the zipcontent to formData object ,"resumes" :name of expected pi 

               
                var response = await _httpClient.PostAsync("http://127.0.0.1:5000/predict-resume", formData); //formdata is sent to API endpoint 

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = $"API Error: {response.ReasonPhrase} - {await response.Content.ReadAsStringAsync()}";
                    return View("Index");
                }

                var responseString = await response.Content.ReadAsStringAsync(); //read the body of http response as a string 
                var result = JsonConvert.DeserializeObject<ResumePredictionResponse>(responseString); //Convert the json strring into an object of type T  ,ResumePredictionResponse: This is the target type representing the structure of the JSON

                if (result?.PredictedCategories == null || result.PredictedCategories.Count == 0) //Check for null refernce 
                {
                    ViewBag.ErrorMessage = "No categories were predicted.";
                    return View("Index");
                }
                foreach (var predictedCategory in result.PredictedCategories) //Round off the confidence score 
                {
                    predictedCategory.ConfidenceScore = (float)Math.Round(predictedCategory.ConfidenceScore ,2);
                }
                

                var uniqueCategories = result.PredictedCategories 
                    .Select(c => c.PredictedCategory) //extract the PreedictCategory from PredictCategories 
                    .Distinct() //remove duplicated Entries 
                    .ToList(); //Converts the resulting sequence into a list

                ViewBag.UniqueCategories = uniqueCategories; //send the uniqueCategories to view 
                ViewBag.PredictedCategories = result.PredictedCategories;

                
                return View("Index");
            }       


           
        }

        [HttpPost]
        public async Task<IActionResult> GetSimilarResumes(IFormFileCollection resumes, string target_role)  //This methods handle the multiple reume along with target role and send them to comapre the similarity and display the result
        {//IformCollection : used to handle multiple resume 
            if (resumes == null || resumes.Count == 0) //Check for null 
            {
                ViewBag.ErrorMessage = "Please upload at least one resume.";
                return View("Index");
            }

            if (string.IsNullOrEmpty(target_role)) //Check target_role is empty or null 
            {
                ViewBag.ErrorMessage = "Please select a target role.";
                return View("Index");
            }

            var formData = new MultipartFormDataContent(); 

            
            foreach (var resume in resumes)
            {
                var resumeContent = new StreamContent(resume.OpenReadStream()); //Open  the uploaded file as stream to send in the request  
                resumeContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf"); //Sets the content type of the uploaded file to application/pdf, indicating the files are PDF resumes. 
                formData.Add(resumeContent, "resumes", resume.FileName); //Add the file content to formDAta with  name of form field to hold the file data and name of Uploaded file 
            }

           
            formData.Add(new StringContent(target_role), "target_role"); //Add the target_role as part of formData 


            var response = await _httpClient.PostAsync("http://127.0.0.1:5000/resume-similarity", formData); //Send request to API 

            if (response.IsSuccessStatusCode) //handle response 
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
//async : allows you to perform tasks that might take time
//await  :await keyword is used to pause execution until the awaited task completes.
//An async method typically returns one of the following:
//Task: If the method performs an operation that does not return a result.
//Task<T>: If the method performs an operation that returns a result of type T.
//void: For event handlers, where no return value is needed.