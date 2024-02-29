using System.Collections.Generic;
namespace ResumeMatcher.Models

{
    public class instances
    {
        public string mineType { get { return "text/plain"; } }
        public string content { get; set; }
        // Add properties for your model's input data
    }
    public class PredictionResult
    {
        public double Confidence { get; set; }
        public string DisplayName { get; set; }
        public string Id { get; set; }
    }

    public class Prediction
    {
        public List<double> Confidences { get; set; }
        public List<string> DisplayNames { get; set; }
        public List<string> Ids { get; set; }
    }

    public class RootObject
    {
        public List<Prediction> Predictions { get; set; }
        public string DeployedModelId { get; set; }
        public string Model { get; set; }
        public string ModelDisplayName { get; set; }
        public string ModelVersionId { get; set; }
    }

}
