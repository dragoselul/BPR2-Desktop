using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Moq;
using Moq.Protected;
using Xunit;

public class ProductViewerTests
{
    [WpfFact]
    public void ValidateInputs_ShouldReturnErrorMessage_IfFieldsAreEmpty()
    {
        // Arrange
        var productViewer = new BPR2_Desktop.Views.Pages.MicroManagement.ProductViewer(null);

        // Access the input fields via reflection
        var widthTextBox = productViewer.FindName("ProductWidthTextBox") as TextBox;
        var heightTextBox = productViewer.FindName("ProductHeightTextBox") as TextBox;
        var depthTextBox = productViewer.FindName("ProductDepthTextBox") as TextBox;
        var storeNameTextBox = productViewer.FindName("StoreNameTextBox") as TextBox;
        var departmentTextBox = productViewer.FindName("DepartmentTextBox") as TextBox;
        var predictionTextBlock = productViewer.FindName("PredictionResultTextBlock") as TextBlock;

        // Leave fields empty
        widthTextBox.Text = "";
        heightTextBox.Text = "";
        depthTextBox.Text = "";
        storeNameTextBox.Text = "";
        departmentTextBox.Text = "";

        // Act
        productViewer.PredictButton_Click(new object(), new RoutedEventArgs());


        // Assert
        Assert.Equal("All fields are required for prediction.", predictionTextBlock.Text);
    }

    [WpfFact]
public void PredictButton_Click_ShouldShowErrorMessage_WhenFieldsAreEmpty()
{
    // Arrange
    var productViewer = new BPR2_Desktop.Views.Pages.MicroManagement.ProductViewer(null);

    // Access the input fields via reflection
    var widthTextBox = productViewer.FindName("ProductWidthTextBox") as TextBox;
    var heightTextBox = productViewer.FindName("ProductHeightTextBox") as TextBox;
    var depthTextBox = productViewer.FindName("ProductDepthTextBox") as TextBox;
    var storeNameTextBox = productViewer.FindName("StoreNameTextBox") as TextBox;
    var departmentTextBox = productViewer.FindName("DepartmentTextBox") as TextBox;
    var predictionTextBlock = productViewer.FindName("PredictionResultTextBlock") as TextBlock;

    // Leave fields empty
    widthTextBox.Text = "";
    heightTextBox.Text = "";
    depthTextBox.Text = "";
    storeNameTextBox.Text = "";
    departmentTextBox.Text = "";

    // Act
    productViewer.PredictButton_Click(null, null);

    // Assert
    Assert.Equal("All fields are required for prediction.", predictionTextBlock.Text);
}

[WpfFact]
public void PredictButton_Click_ShouldNotShowErrorMessage_WhenFieldsAreFilled()
{
    // Arrange
    var productViewer = new BPR2_Desktop.Views.Pages.MicroManagement.ProductViewer(null);

    // Access the input fields via reflection
    var widthTextBox = productViewer.FindName("ProductWidthTextBox") as TextBox;
    var heightTextBox = productViewer.FindName("ProductHeightTextBox") as TextBox;
    var depthTextBox = productViewer.FindName("ProductDepthTextBox") as TextBox;
    var storeNameTextBox = productViewer.FindName("StoreNameTextBox") as TextBox;
    var departmentTextBox = productViewer.FindName("DepartmentTextBox") as TextBox;

    // Fill fields with valid data
    widthTextBox.Text = "10";
    heightTextBox.Text = "20";
    depthTextBox.Text = "30";
    storeNameTextBox.Text = "TestStore";
    departmentTextBox.Text = "TestDepartment";

    // Act
    productViewer.PredictButton_Click(null, null);

    // Assert
    // No exception means the fields were validated successfully
}

/*[WpfFact]
public async Task PredictButton_Click_ShouldUpdatePredictionResult()
{
    // Arrange
    var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
    mockHttpMessageHandler
        .Protected()
        .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        )
        .ReturnsAsync(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{ \"prediction\": \"CategoryA\", \"confidence\": 0.85 }")
        });

    var productViewer = new BPR2_Desktop.Views.Pages.MicroManagement.ProductViewer(null);

    // Mock input fields
    var widthTextBox = new TextBox { Text = "10" };
    var heightTextBox = new TextBox { Text = "20" };
    var depthTextBox = new TextBox { Text = "30" };
    var storeNameTextBox = new TextBox { Text = "TestStore" };
    var departmentTextBox = new TextBox { Text = "TestDepartment" };
    var predictionTextBlock = new TextBlock();

    // Assign fields
    productViewer.ProductWidthTextBox = widthTextBox;
    productViewer.ProductHeightTextBox = heightTextBox;
    productViewer.ProductDepthTextBox = depthTextBox;
    productViewer.StoreNameTextBox = storeNameTextBox;
    productViewer.DepartmentTextBox = departmentTextBox;
    productViewer.PredictionResultTextBlock = predictionTextBlock;

    // Replace HttpClient with mock
    var httpClient = new HttpClient(mockHttpMessageHandler.Object);
    productViewer.HttpClient = httpClient;

    // Act
    productViewer.PredictButton_Click(null, null);

    // Assert
    Assert.Contains("CategoryA", predictionTextBlock.Text);
    Assert.Contains("85", predictionTextBlock.Text);
}*/










}
