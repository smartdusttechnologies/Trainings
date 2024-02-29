using Google.Apis.Auth.OAuth2;
using Google.Cloud.AIPlatform.V1;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ResumeMatcher.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class VertexAiController : Controller
{
    private readonly string _projectId;
    private readonly string _region;
    private readonly string _endpointName;
    private readonly string _apiKey; // Or service account JSON path

    // Constructor with authentication details
    public VertexAiController() // Or service account path
    {
        _projectId = "132316917298";
        _region = "us-central1";
        _endpointName = "8197072490325868544";
        _apiKey = "ya29.a0AfB_byDHbdJpJGaFhOG4qHWy63PP-CNrcmoW6SuTenJ3xGlKp8rhrCmmSiUrtWtSBGeIJpczCWR7n5Vt-2arBbbbw9ylk3nfUZ-t6B435Q6vmjQftSLdmML4FUoWGMbjykLu6G84867wcVAHeTEcbx8ER-GIHWDAYpQSaCgYKAasSARESFQHGX2MiY2Kf1IGdDNkHBklxMoeu3Q0171"; // Or service account path
    }

    [HttpPost]
    public async Task<IActionResult> Predict([FromBody] instances inputData)
    {
        // Use either API Key or service account authentication
         await MakePredictionWithApiKeyAsync(inputData);
        

        return Ok(); // Or handle the prediction result appropriately
    }

    // Make prediction with API Key
    private async Task MakePredictionWithApiKeyAsync(instances inputData)
    {
        string url = $"https://{_region}-aiplatform.googleapis.com/v1/projects/{_projectId}/locations/{_region}/endpoints/{_endpointName}:predict";

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            string json = JsonConvert.SerializeObject(inputData);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                // Process the prediction result (e.g., deserialize from JSON)
            }
            else
            {
                // Handle errors (e.g., log, return error response)
            }
        }
    }
   
    // Define your InputData class to match the expected input format of your Vertex AI model
   
}