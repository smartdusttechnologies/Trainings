using Newtonsoft.Json;

namespace Ml_Models_In_VS.Models
{
    public class ResumePredictionResult
    {
        [JsonProperty("resume_name")]
        public string ResumeName { get; set; }

        [JsonProperty("predicted_category")]
        public string PredictedCategory { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("confidence_score")]
        public float ConfidenceScore { get; set; }
    }

    public class ResumePredictionResponse
    {
        [JsonProperty("predicted_categories")]
        public List<ResumePredictionResult> PredictedCategories { get; set; }
    }
}
