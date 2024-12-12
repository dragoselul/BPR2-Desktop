using System.Net.Http;
using System.Text;
using System.Windows.Controls;
using BPR2_Desktop.Helpers;
using BPR2_Desktop.Views.Components.MicroManagement;
using Newtonsoft.Json;
using Wpf.Ui.Abstractions.Controls;

namespace BPR2_Desktop.Views.Pages.MicroManagement;


public partial class ProductViewer : INavigableView<ViewModels.MicroManagement.ProductViewModel>
{
    public ViewModels.MicroManagement.ProductViewModel ViewModel { get; }
    public ProductViewer(ViewModels.MicroManagement.ProductViewModel vm)
    {
        InitializeComponent();
        ViewModel = vm;
        DataContext = ViewModel;
    }
    
    internal async void PredictButton_Click(object sender, RoutedEventArgs e)
{
    try
    {
        // Retrieve data from input fields
        string productWidth = ProductWidthTextBox.Text;
        string productHeight = ProductHeightTextBox.Text;
        string productDepth = ProductDepthTextBox.Text;
        string storeName = StoreNameTextBox.Text;
        string department = DepartmentTextBox.Text;

        // Validate input
        if (string.IsNullOrWhiteSpace(productWidth) ||
            string.IsNullOrWhiteSpace(productHeight) ||
            string.IsNullOrWhiteSpace(productDepth) ||
            string.IsNullOrWhiteSpace(storeName) ||
            string.IsNullOrWhiteSpace(department))
        {
            PredictionResultTextBlock.Text = "All fields are required for prediction.";
            return;
        }

        // Create an HTTP client and send a POST request
        using var httpClient = new HttpClient();
        var apiUrl = "http://127.0.0.1:5000/predict";

        var payload = new
        {
            Product_Width = double.Parse(productWidth),
            Product_Height = double.Parse(productHeight),
            Product_Depth = double.Parse(productDepth),
            Store_Name = storeName,
            Department = department
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
        PredictionResultTextBlock.Text = confidence.HasValue
            ? $"Hmm, I think this item belongs to: {prediction}\n My confidence level is: {confidence:F2}%"
            : $"Prediction: {prediction}\nConfidence: Not available";
    }
    catch (Exception ex)
    {
        PredictionResultTextBlock.Text = $"Error: {ex.Message}";
    }
}

}





     
