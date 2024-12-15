using Newtonsoft.Json;

namespace Ml_Models_In_VS.Models
{
    public class ResumeSimilarityResult
    {
        [JsonProperty("similarity_score")]
        public List<SimilarityScoreItem> Top5SimilarResumes { get; set; }
    }

    public class SimilarityScoreItem
    {
        [JsonProperty("resume_name")]
        public string ResumeName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("similarity_score")]
        public double SimilarityScore { get; set; }
    }
}
