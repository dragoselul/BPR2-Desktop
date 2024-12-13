using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace BPR2_Desktop.Model.ML;

public static class PredictCategory
{
    
    public static async Task<string> PredictButton_Click(string productWidth, string productHeight, string productDepth, string storeName, string department)
    {
        try
        {
            // Retrieve data from input fields
            string width = productWidth;
            string height = productHeight;
            string depth = productDepth;
            string storeNameStr = storeName;
            string departmentStr = department;

            // Validate input
            if (string.IsNullOrWhiteSpace(productWidth) ||
                string.IsNullOrWhiteSpace(productHeight) ||
                string.IsNullOrWhiteSpace(productDepth) ||
                string.IsNullOrWhiteSpace(storeName) ||
                string.IsNullOrWhiteSpace(department))
            {
                return "All fields are required for prediction.";
            }

            // Create an HTTP client and send a POST request
            using var httpClient = new HttpClient();
            var apiUrl = "http://127.0.0.1:5000/predict";

            var payload = new
            {
                Product_Width = double.Parse(width),
                Product_Height = double.Parse(height),
                Product_Depth = double.Parse(depth),
                Store_Name = storeNameStr,
                Department = departmentStr
            };

            var jsonPayload = JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiUrl, httpContent);
            response.EnsureSuccessStatusCode();

            // Parse the response
            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(jsonResponse);

            // Extract prediction and confidence score
            string prediction = result.prediction;
            double? confidence = result.confidence != null ? (double)result.confidence * 100 : null;

            // Update the UI with prediction and confidence score
            return confidence.HasValue
                ? $"Hmm, I think this item belongs to: {prediction}\n My confidence level is: {confidence:F2}%"
                : $"Prediction: {prediction}\nConfidence: Not available";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}