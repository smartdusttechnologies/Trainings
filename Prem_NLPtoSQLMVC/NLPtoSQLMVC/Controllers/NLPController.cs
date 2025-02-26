using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace NLPtoSQLMVC.Controllers
{
    public class NLPController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CorrectPrediction(string userQuery, string correctSql)
        {
            if (string.IsNullOrWhiteSpace(userQuery) || string.IsNullOrWhiteSpace(correctSql))
                return Json(new { error = "Both userQuery and correctSql are required." });

            string pythonScript = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/python_model/predict.py");

            // Call predict.py with "feedback" mode
            string arguments = $"\"{userQuery}\" \"{correctSql}\"";

            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"{pythonScript} {arguments}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            string resultJson;
            using (Process process = Process.Start(start))
            {
                if (process == null)
                {
                    return Json(new { error = "Failed to start Python process." });
                }

                resultJson = process.StandardOutput.ReadToEnd().Trim();
                string errorOutput = process.StandardError.ReadToEnd().Trim();

                if (!string.IsNullOrEmpty(errorOutput))
                {
                    Console.WriteLine($"Python Error: {errorOutput}");
                    return Json(new { error = "Python script error", details = errorOutput });
                }
            }

            return Json(new { success = "Feedback saved successfully.", rawData = resultJson });
        }

        [HttpPost]
        public IActionResult TrainModel()
        {
            string pythonScript = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PythonScripts", "train_model.py");

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = pythonScript,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(errors))
            {
                return BadRequest($"❌ Python Script Error:\n{errors}");
            }

            // Extract accuracy from output
            string accuracyLine = output.Split('\n').FirstOrDefault(line => line.Contains("Model Accuracy"));
            string accuracy = accuracyLine != null ? accuracyLine.Replace("Model Accuracy:", "").Trim() : "Unknown";

            return Ok(new { message = "✅ Model Training Completed.", accuracy = accuracy });
        }

        [HttpPost]
        public IActionResult Predict(string userQuery)
        {
            if (string.IsNullOrWhiteSpace(userQuery))
                return Json(new { error = "Query cannot be empty" });

            string pythonScript = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/python_model/predict.py");
            string arguments = $"\"{userQuery}\"";

            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"{pythonScript} {arguments}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            string resultJson;
            string errorOutput;

            using (Process process = Process.Start(start))
            {
                if (process == null)
                {
                    return Json(new { error = "Failed to start Python process." });
                }

                // Read the output and error streams
                resultJson = process.StandardOutput.ReadToEnd().Trim();
                errorOutput = process.StandardError.ReadToEnd().Trim();

                // If there's an error in the Python script, handle it
                if (!string.IsNullOrEmpty(errorOutput))
                {
                    Console.WriteLine($"Python Error: {errorOutput}");
                    return Json(new
                    {
                        sqlQuery = "Script Not Generated",
                        result = "N/A",
                        message = "Failed to generate SQL query. Please provide the correct SQL query.",
                        promptForFeedback = true // Indicate that feedback is needed
                    });
                }
            }

            Console.WriteLine($"Raw JSON Response: {resultJson}");

            try
            {
                // Deserialize the Python script's output
                var result = JsonSerializer.Deserialize<NLPResponse>(resultJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null || result.Result == null)
                {
                    return Json(new { error = "Failed to parse JSON response.", rawData = resultJson });
                }

                return Json(new
                {
                    sqlQuery = result.SqlQuery,
                    result = result.Result,
                    message = "✅ Query Executed Successfully"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ JSON Deserialization Error: {ex.Message}");
                return Json(new { error = "JSON Deserialization Error", details = ex.Message, rawData = resultJson });
            }
        }
        [HttpPost]
        public IActionResult UseByApi(string userQuery)
        {
            if (string.IsNullOrWhiteSpace(userQuery))
                return Json(new { error = "Query cannot be empty" });

            string nlpToSqlScript = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PythonScripts/ApiModel.py");
            string executeSqlScript = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/python_model/execute_apisql.py");

            // 🔹 Step 1: Convert NLP to SQL
            ProcessStartInfo convertStart = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"{nlpToSqlScript} \"{userQuery}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            string sqlQuery;
            using (Process process = Process.Start(convertStart))
            {
                if (process == null)
                    return Json(new { error = "Failed to start Python process for NLP to SQL conversion." });

                sqlQuery = process.StandardOutput.ReadToEnd().Trim();
                string errorOutput = process.StandardError.ReadToEnd().Trim();

                if (!string.IsNullOrEmpty(errorOutput) && !errorOutput.Contains("grpc_wait_for_shutdown_with_timeout"))
                {
                    return Json(new { error = "NLP to SQL Conversion Error", details = errorOutput });
                }
            }

            // 🔹 Step 2: Execute SQL Query
            ProcessStartInfo executeStart = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"{executeSqlScript} \"{sqlQuery}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            string resultJson;
            using (Process process = Process.Start(executeStart))
            {
                if (process == null)
                    return Json(new { error = "Failed to start Python process for SQL execution." });

                resultJson = process.StandardOutput.ReadToEnd().Trim();
                string errorOutput = process.StandardError.ReadToEnd().Trim();

                if (!string.IsNullOrEmpty(errorOutput))
                    return Json(new { error = "SQL Execution Error", details = errorOutput });
            }

            // 🔹 Step 3: Parse JSON into NLPResponse Model
            var result = JsonSerializer.Deserialize<NLPResponse>(resultJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result == null || result.Result == null)
            {
                return Json(new { error = "Failed to parse JSON response.", rawData = resultJson });
            }

            return Json(new
            {
                sqlQuery = result.SqlQuery,
                result = result.Result,
                message = "✅ Query Executed Successfully"
            });
        }
    }

    public class NLPResponse
    {
        [JsonPropertyName("sql_query")]
        public string SqlQuery { get; set; }

        [JsonPropertyName("result")]
        public QueryResult Result { get; set; }
    }

    public class QueryResult
    {
        [JsonPropertyName("columns")]
        public List<string> Columns { get; set; }

        [JsonPropertyName("data")]
        public List<List<object>> Data { get; set; }
    }
}
