using Newtonsoft.Json;

namespace Ml_Models_In_VS.Models
{
    public class ResumePredictionResult
    {
        [JsonProperty("predicted_categories")]
        public List<PredictedCategoryItem> PredictedCategories { get; set; }
    }

    public class PredictedCategoryItem
    {
        [JsonProperty("name")]
        public string Name { get; set; } 

        [JsonProperty("predicted_category")]
        public string PredictedCategory { get; set; } 

        [JsonProperty("resume_name")]
        public string ResumeName { get; set; } 
        [JsonProperty("confidence_score")]
        public string ConfidenceScore { get; set; } 
    }
}
